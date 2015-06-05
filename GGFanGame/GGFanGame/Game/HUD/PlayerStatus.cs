using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GGFanGame.Game.Level.Playable;

namespace GGFanGame.Game.Level.HUD
{
    /// <summary>
    /// HUD element to display player status.
    /// </summary>
    class PlayerStatus
    {
        private GGGame _gameInstance;

        private PlayerCharacter _player;
        private PlayerIndex _playerIndex;

        private int _drawHealthWidth = 0;

        private Texture2D _headTexture;
        private Texture2D _barTexture;

        private SpriteFont _font;

        private class Ellipse
        {
            public Vector2 position;
            public float size;

            public bool sinking = true;
            public bool growing = true;

            Random rnd = new Random();

            public Ellipse(Vector2 position, float size)
            {
                this.position = position;
                this.size = size;

                if (size > 13)
                    growing = false;

                if (position.X > 80)
                    sinking = false;
            }

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

        private List<Ellipse> ellipses = new List<Ellipse>();

        /// <summary>
        /// Creates a new instance of the PlayerStatus class.
        /// </summary>
        public PlayerStatus(GGGame game, PlayerCharacter player, PlayerIndex playerIndex)
        {
            _gameInstance = game;

            _player = player;
            _playerIndex = playerIndex;

            _barTexture = _gameInstance.textureManager.getResource(@"UI\Bars");
            _headTexture = _gameInstance.textureManager.getResource(@"UI\" + _player.name);
            _font = _gameInstance.Content.Load<SpriteFont>("CartoonFontSmall");

            for (int i = 0; i < 42; i++)
            {
                ellipses.Add(new Ellipse(new Vector2(_gameInstance.random.Next(-20, 125), _gameInstance.random.Next(-15, 4)),
                                         _gameInstance.random.Next(15, 45)));
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

            Drawing.Graphics.drawRectangle(new Rectangle(xOffset + 75, 65, 130, 20), Drawing.Colors.getColor(_playerIndex));
            foreach (var ell in ellipses)
            {
                Drawing.Graphics.drawCircle(new Vector2(xOffset + 75, 56) + ell.position, (int)ell.size, Drawing.Colors.getColor(_playerIndex), 1d);
                ell.update();
            }

            _gameInstance.spriteBatch.DrawString(_font, _player.name + "    x3", new Vector2(xOffset + 86, 56), Color.White);

            _gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(xOffset + 80, 74, 172, 16), new Rectangle(0, 12, 86, 8), Color.White);
            _gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(xOffset + 80, 74, _drawHealthWidth * 2, 16), new Rectangle(0, 0, _drawHealthWidth, 8), Color.White);
            _gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(xOffset + 80, 90, 172, 8), new Rectangle(0, 8, 86, 4), Color.White);

            //Render face depending on the player's state.
            if (_player.state == ObjectState.HurtFalling || _player.state == ObjectState.Hurt)
                _gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(48, 0, 48, 48), Color.White);
            else if (_player.state == ObjectState.Dead)
                _gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(96, 0, 48, 48), Color.White);
            else
                _gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(xOffset, 34, 96, 96), new Rectangle(0, 0, 48, 48), Color.White);
        }
    }
}