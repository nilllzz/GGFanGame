using System;
using System.Collections.Generic;
using GGFanGame.Drawing;
using GGFanGame.Game.Playable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Game.HUD
{
    /// <summary>
    /// HUD element to display player status.
    /// </summary>
    internal sealed class PlayerStatus : IDisposable
    {
        private readonly ContentManager _content;

        private readonly PlayerCharacter _player;
        private readonly PlayerIndex _playerIndex;

        private int _drawHealthWidth = 0;
        private int _drawGrumpWidth = 0;
        private double _grumpBarGlowAnimation = 0d;
        
        private readonly Texture2D _barTexture, _fireTexture;
        private readonly SpriteFont _font, _fontLarge;

        private readonly List<Bubble> _bubbles = new List<Bubble>();

        private readonly Random _random = new Random();

        private RenderTarget2D _target;
        private SpriteBatch _comboBatch;

        private string _lastComboDrawn = "";
        private bool _drawGrowingNumber = true;
        private float _growingNumberSize = 1f;

        internal bool IsDisposed { get; private set; }

        // load here because player could either be null or change.
        private Texture2D HeadTexture => _content.Load<Texture2D>(@"UI\HUD\" + _player.Name);

        /// <summary>
        /// Creates a new instance of the PlayerStatus class.
        /// </summary>
        internal PlayerStatus(PlayerCharacter player, PlayerIndex playerIndex, ContentManager content)
        {
            _player = player;
            _playerIndex = playerIndex;
            _content = content;

            _barTexture = _content.Load<Texture2D>(@"UI\HUD\Bars");
            _fireTexture = _content.Load<Texture2D>(@"UI\HUD\Fire");
            _font = _content.Load<SpriteFont>(@"Fonts\CartoonFontSmall");
            _fontLarge = _content.Load<SpriteFont>(@"Fonts\CartoonFont");

            for (var i = 0; i < 42; i++)
            {
                _bubbles.Add(new Bubble(new Vector2(_random.Next(-20, 125), _random.Next(-15, 4)),
                                         _random.Next(15, 45), (int)playerIndex + i));
            }
        }

        /// <summary>
        /// Draws the player status.
        /// </summary>
        internal void Draw(SpriteBatch batch)
        {
            // if no player is created for this status, don't render.
            if (_player == null)
                return;

            // TODO: get rid of hardcoded life count!
            // TODO: create update method

            var healthWidth = (int)(_player.Health / (double)_player.MaxHealth * 86d);
            // animate health bar change:
            if (healthWidth < _drawHealthWidth)
            {
                _drawHealthWidth -= 2;
                if (_drawHealthWidth < healthWidth)
                {
                    _drawHealthWidth = healthWidth;
                }
            }
            else if (healthWidth > _drawHealthWidth)
            {
                _drawHealthWidth += 2;
                if (_drawHealthWidth > healthWidth)
                {
                    _drawHealthWidth = healthWidth;
                }
            }
            
            var grumpWidth = (int)(_player.GrumpPower / (double)_player.MaxGrumpPower * 62d);
            if (grumpWidth < _drawGrumpWidth)
            {
                _drawGrumpWidth -= 2;
                if (_drawGrumpWidth < grumpWidth)
                {
                    _drawGrumpWidth = grumpWidth;
                }
            }
            else if (grumpWidth > _drawGrumpWidth)
            {
                _drawGrumpWidth += 2;
                if (_drawGrumpWidth > grumpWidth)
                {
                    _drawGrumpWidth = grumpWidth;
                }
            }

            //Render bars:
            var xOffset = 34 + 320 * (int)_playerIndex;

            batch.DrawRectangle(new Rectangle(xOffset + 75, 65, 120, 20), Colors.GetColor(_playerIndex));
            foreach (var ell in _bubbles)
            {
                if (_player.GrumpPower == _player.MaxGrumpPower)
                {
                    batch.Draw(_fireTexture,
                        new Rectangle((int)(xOffset + 90 + ell.position.X - ell.size), (int)(71 + ell.position.Y - ell.size), (int)ell.size * 2, (int)ell.size * 2),
                        new Rectangle(((int)ell.size / 9) % 3 * 32, 0, 32, 32),
                        Colors.GetColor(_playerIndex));
                }
                else
                {
                    batch.DrawCircle(new Vector2(xOffset + 90, 71) + ell.position - new Vector2(ell.size / 2), (int)ell.size, Colors.GetColor(_playerIndex), 1d);
                }
                ell.Update();
            }
            _grumpBarGlowAnimation += 0.2d;

            var textColor = Color.White;
            if (_playerIndex == PlayerIndex.One || _playerIndex == PlayerIndex.Two)
            {
                textColor = Color.Black;
            }

            batch.DrawString(_font, _player.Name + "    x3", new Vector2(xOffset + 86, 56), textColor);

            batch.Draw(_barTexture, new Rectangle(xOffset + 80, 74, 172, 16), new Rectangle(0, 12, 86, 8), Color.White);
            batch.Draw(_barTexture, new Rectangle(xOffset + 80, 74, _drawHealthWidth * 2, 16), new Rectangle(0, 0, _drawHealthWidth, 8), Color.White);

            batch.Draw(_barTexture, new Rectangle(xOffset + 80, 90, 124, 8), new Rectangle(0, 20, 62, 4), Color.White);
            batch.Draw(_barTexture, new Rectangle(xOffset + 80, 90, _drawGrumpWidth * 2, 8), new Rectangle(0, 8, _drawGrumpWidth, 4), Color.White);

            if (_player.GrumpPower == _player.MaxGrumpPower)
            {
                var alpha = (int)((Math.Sin(_grumpBarGlowAnimation) + 1d) / 2d * 160);
                batch.Draw(_barTexture, new Rectangle(xOffset + 80, 90, _drawGrumpWidth * 2 - 2, 6), new Rectangle(0, 24, _drawGrumpWidth - 1, 3), new Color(255, 255, 255, alpha));
            }

            //Render face depending on the player's state.
            if (_player.State == ObjectState.HurtFalling || _player.State == ObjectState.Hurt)
                batch.Draw(HeadTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(48, 0, 48, 48), Color.White);
            else if (_player.State == ObjectState.Dead)
                batch.Draw(HeadTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(96, 0, 48, 48), Color.White);
            else
                batch.Draw(HeadTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(0, 0, 48, 48), Color.White);

            DrawCombo(batch, xOffset);
        }

        /// <summary>
        /// Renders the combo meter.
        /// </summary>
        private void DrawCombo(SpriteBatch batch, int xOffset)
        {
            if (_player.ComboChain > 0 && _player.ComboDelay > 0)
            {
                //Initialize things:
                if (_target == null)
                {
                    _target = new RenderTarget2D(GameInstance.GraphicsDevice, 120, 120, true, GameInstance.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                    _comboBatch = new SpriteBatch(GameInstance.GraphicsDevice);
                }

                var playerColor = Colors.GetColor(_playerIndex);

                batch.DrawString(_fontLarge, "x" + _player.ComboChain, new Vector2(xOffset, 100), Color.White, -0.2f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                //We change the render target here to render the colored in combo text to a texture:
                GameInstance.GraphicsDevice.SetRenderTarget(_target);
                GameInstance.GraphicsDevice.Clear(Color.Transparent);

                _comboBatch.Begin();
                _comboBatch.DrawString(_fontLarge, "x" + _player.ComboChain, Vector2.Zero, Color.White);
                _comboBatch.End();

                RenderTargetManager.ResetRenderTarget();

                var fontWidth = _fontLarge.MeasureString("x" + _player.ComboChain).X * (_player.ComboDelay / 100f);

                batch.Draw(_target, new Rectangle(xOffset, 100, (int)(fontWidth * 1f), _target.Height * 1), new Rectangle(0, 0, (int)fontWidth, _target.Height), playerColor, -0.2f, Vector2.Zero, SpriteEffects.None, 0f);

                //When the player adds to the combo, draw a growing number:
                if (_lastComboDrawn != _player.ComboChain.ToString())
                {
                    _lastComboDrawn = _player.ComboChain.ToString();
                    _growingNumberSize = 1f;
                    _drawGrowingNumber = true;
                }
                if (_drawGrowingNumber)
                {
                    _growingNumberSize += 0.1f;

                    var normalSize = _fontLarge.MeasureString("x" + _player.ComboChain);
                    var growingSize = normalSize * _growingNumberSize;
                    var largestSize = normalSize * _growingNumberSize * 2f;

                    batch.DrawString(_fontLarge, "x" + _player.ComboChain, new Vector2(xOffset - (largestSize.X - normalSize.X) / 2f + 5, 100 - (largestSize.Y - normalSize.Y) / 2f + 5), new Color(255, 255, 255, (int)(255 * (2f - _growingNumberSize))), -0.2f, Vector2.Zero, _growingNumberSize  * 2f, SpriteEffects.None, 0f);
                    batch.DrawString(_fontLarge, "x" + _player.ComboChain, new Vector2(xOffset - (growingSize.X - normalSize.X) / 2f + 5, 100 - (growingSize.Y - normalSize.Y) / 2f + 5), new Color(playerColor.R, playerColor.G, playerColor.B, (int)(255 * (2f - _growingNumberSize))), -0.2f, Vector2.Zero, _growingNumberSize, SpriteEffects.None, 0f);

                    if (_growingNumberSize >= 2f)
                        _drawGrowingNumber = false;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~PlayerStatus()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_target != null && !_target.IsDisposed) _target.Dispose();
                    if (_comboBatch != null && !_comboBatch.IsDisposed) _comboBatch.Dispose();
                }

                _target = null;
                _comboBatch = null;

                IsDisposed = true;
            }
        }
    }
}