using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Screens.Game.Level.HUD
{
    /// <summary>
    /// HUD element to display player status.
    /// </summary>
    class PlayerStatus
    {
        private GGGame _gameInstance;

        private GrumpSpace.PlayerCharacter _player;
        private PlayerIndex _playerIndex;

        private int _drawHealthWidth = 0;

        private Texture2D _headTexture;
        private Texture2D _barTexture;

        private SpriteFont _font;

        /// <summary>
        /// Creates a new instance of the PlayerStatus class.
        /// </summary>
        public PlayerStatus(GGGame game, GrumpSpace.PlayerCharacter player, PlayerIndex playerIndex)
        {
            _gameInstance = game;

            _player = player;
            _playerIndex = playerIndex;

            _barTexture = _gameInstance.textureManager.getResource(@"UI\Bars");
            _headTexture = _gameInstance.textureManager.getResource(@"UI\" + _player.name);
            _font = _gameInstance.Content.Load<SpriteFont>("CartoonFontSmall");
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
            _gameInstance.spriteBatch.DrawString(_font, _player.name + "    x3", new Vector2(120, 56), Color.Black);
            _gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(114, 74, 172, 16), new Rectangle(0, 12, 86, 8), Color.White);
            _gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(114, 74, _drawHealthWidth * 2, 16), new Rectangle(0, 0, _drawHealthWidth, 8), Color.White);
            _gameInstance.spriteBatch.Draw(_barTexture, new Rectangle(114, 90, 172, 8), new Rectangle(0, 8, 86, 4), Color.White);

            //Render face depending on the player's state.
            if (_player.state == GrumpSpace.ObjectState.HurtFalling || _player.state == GrumpSpace.ObjectState.Hurt)
                _gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(34, 34, 96, 96), new Rectangle(48, 0, 48, 48), Color.White);
            else if (_player.state == GrumpSpace.ObjectState.Dead)
                _gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(34, 34, 96, 96), new Rectangle(96, 0, 48, 48), Color.White);
            else
                _gameInstance.spriteBatch.Draw(_headTexture, new Rectangle(34, 34, 96, 96), new Rectangle(0, 0, 48, 48), Color.White);
        }
    }
}