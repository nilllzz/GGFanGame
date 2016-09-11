using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GGFanGame.Game.Playable;
using static GameProvider;

namespace GGFanGame.Game.HUD
{
    /// <summary>
    /// HUD element to display player status.
    /// </summary>
    class PlayerStatus
    {
        private PlayerCharacter _player;
        private PlayerIndex _playerIndex;

        private int _drawHealthWidth = 0;

        private Texture2D _headTexture;
        private Texture2D _barTexture;

        private SpriteFont _font;
        private SpriteFont _fontLarge;

        /// <summary>
        /// Class to represent the bubbles in the HUD.
        /// </summary>
        private class Bubble
        {
            public Vector2 position;
            public float size;

            public bool sinking = true;
            public bool growing = true;

            private Random _rnd;

            public Bubble(Vector2 position, float size, int seed)
            {
                this.position = position;
                this.size = size;

                _rnd = new Random(seed);

                if (size > 13)
                    growing = false;

                if (position.X > 80)
                    sinking = false;
            }

            /// <summary>
            /// Update size/position.
            /// </summary>
            public void update()
            {
                if (growing)
                {
                    size += 0.75f;
                    if (size >= 45)
                        growing = false;
                }
                else
                {
                    size -= 0.75f;
                    if (size <= 15)
                        growing = true;
                }

                if (sinking)
                {
                    position.Y += 0.1f;
                    if (position.Y >= -2f)
                        sinking = false;
                }
                else
                {
                    position.Y -= 0.1f;
                    if (position.Y <= -15f)
                        sinking = true;
                }
            }
        }

        private List<Bubble> bubbles = new List<Bubble>();

        /// <summary>
        /// Creates a new instance of the PlayerStatus class.
        /// </summary>
        public PlayerStatus(PlayerCharacter player, PlayerIndex playerIndex)
        {
            _player = player;
            _playerIndex = playerIndex;

            _barTexture = gameInstance.Content.Load<Texture2D>(@"UI\HUD\Bars");
            _headTexture = gameInstance.Content.Load<Texture2D>(@"UI\HUD\" + _player.name);
            _font = gameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFontSmall");
            _fontLarge = gameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFont");

            for (int i = 0; i < 42; i++)
            {
                bubbles.Add(new Bubble(new Vector2(gameInstance.random.Next(-20, 125), gameInstance.random.Next(-15, 4)),
                                         gameInstance.random.Next(15, 45), (int)playerIndex + i));
            }
        }

        /// <summary>
        /// Draws the player status.
        /// </summary>
        public void draw()
        {
            //TODO: get rid of hardcoded max health and live count!

            int healthWidth = (int)(_player.health / 100d * 86d); //Determine bar width with health.
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

            //Render bars:
            //TODO: Implement grump meter correctly.

            int xOffset = 34 + 320 * (int)_playerIndex;

            Drawing.Graphics.drawRectangle(new Rectangle(xOffset + 75, 65, 120, 20), Drawing.Colors.getColor(_playerIndex));
            foreach (var ell in bubbles)
            {
                Drawing.Graphics.drawCircle(new Vector2(xOffset + 75, 56) + ell.position, (int)ell.size, Drawing.Colors.getColor(_playerIndex), 1d);
                ell.update();
            }

            var textColor = Color.White;
            if (_playerIndex == PlayerIndex.One || _playerIndex == PlayerIndex.Two)
            {
                textColor = Color.Black;
            }

            gameInstance.spriteBatch.DrawString(_font, _player.name + "    x3", new Vector2(xOffset + 86, 56), textColor);

            gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(xOffset + 80, 74, 172, 16), new Rectangle(0, 12, 86, 8), Color.White);
            gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(xOffset + 80, 74, _drawHealthWidth * 2, 16), new Rectangle(0, 0, _drawHealthWidth, 8), Color.White);
            gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(xOffset + 80, 90, 172, 8), new Rectangle(0, 8, 86, 4), Color.White);

            //Render face depending on the player's state.
            if (_player.state == ObjectState.HurtFalling || _player.state == ObjectState.Hurt)
                gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(48, 0, 48, 48), Color.White);
            else if (_player.state == ObjectState.Dead)
                gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(96, 0, 48, 48), Color.White);
            else
                gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(0, 0, 48, 48), Color.White);

            drawCombo(xOffset);
        }

        RenderTarget2D _target;
        SpriteBatch _comboBatch;

        string _lastComboDrawn = "";
        bool _drawGrowingNumber = true;
        float _growingNumberSize = 1f;

        /// <summary>
        /// Renders the combo meter.
        /// </summary>
        private void drawCombo(int xOffset)
        {
            if (_player.comboChain > 0 && _player.comboDelay > 0)
            {
                //Initialize things:
                if (_target == null)
                {
                    var pp = gameInstance.GraphicsDevice.PresentationParameters;
                    _target = new RenderTarget2D(gameInstance.GraphicsDevice, 120, 120, true, gameInstance.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                    _comboBatch = new SpriteBatch(gameInstance.GraphicsDevice);
                }

                Color playerColor = Drawing.Colors.getColor(_playerIndex);

                gameInstance.spriteBatch.DrawString(_fontLarge, "x" + _player.comboChain, new Vector2(xOffset, 100), Color.White, -0.2f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                //We change the render target here to render the colored in combo text to a texture:
                gameInstance.GraphicsDevice.SetRenderTarget(_target);
                gameInstance.GraphicsDevice.Clear(Color.Transparent);

                _comboBatch.Begin();
                _comboBatch.DrawString(_fontLarge, "x" + _player.comboChain, Vector2.Zero, Color.White);
                _comboBatch.End();

                RenderTargetManager.resetRenderTarget();

                var fontWidth = _fontLarge.MeasureString("x" + _player.comboChain).X * (_player.comboDelay / 100f);

                gameInstance.spriteBatch.Draw(_target, new Rectangle(xOffset, 100, (int)(fontWidth * 1f), _target.Height * 1), new Rectangle(0, 0, (int)fontWidth, _target.Height), playerColor, -0.2f, Vector2.Zero, SpriteEffects.None, 0f);

                //When the player adds to the combo, draw a growing number:
                if (_lastComboDrawn != _player.comboChain.ToString())
                {
                    _lastComboDrawn = _player.comboChain.ToString();
                    _growingNumberSize = 1f;
                    _drawGrowingNumber = true;
                }
                if (_drawGrowingNumber)
                {
                    _growingNumberSize += 0.1f;

                    var normalSize = _fontLarge.MeasureString("x" + _player.comboChain);
                    var growingSize = normalSize * _growingNumberSize;
                    var largestSize = normalSize * _growingNumberSize * 2f;

                    gameInstance.spriteBatch.DrawString(_fontLarge, "x" + _player.comboChain, new Vector2(xOffset - (largestSize.X - normalSize.X) / 2f + 5, 100 - (largestSize.Y - normalSize.Y) / 2f + 5), new Color(255, 255, 255, (int)(255 * (2f - _growingNumberSize))), -0.2f, Vector2.Zero, _growingNumberSize  * 2f, SpriteEffects.None, 0f);
                    gameInstance.spriteBatch.DrawString(_fontLarge, "x" + _player.comboChain, new Vector2(xOffset - (growingSize.X - normalSize.X) / 2f + 5, 100 - (growingSize.Y - normalSize.Y) / 2f + 5), new Color(playerColor.R, playerColor.G, playerColor.B, (int)(255 * (2f - _growingNumberSize))), -0.2f, Vector2.Zero, _growingNumberSize, SpriteEffects.None, 0f);

                    if (_growingNumberSize >= 2f)
                        _drawGrowingNumber = false;
                }
            }
        }
    }
}