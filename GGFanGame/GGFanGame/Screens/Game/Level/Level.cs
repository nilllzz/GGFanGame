using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Screens.Game.Level
{
    class Level
    {
        private GGGame _gameInstance;
        private List<LevelObject> _objects;

        private GrumpSpace.Arin _arin;

        private SpriteFont _grumpFont;

        public Level(GGGame game)
        {
            _gameInstance = game;

            _grumpFont = _gameInstance.Content.Load<SpriteFont>("CartoonFontSmall");

            _objects = new List<LevelObject>();

            _arin = new GrumpSpace.Arin(game, PlayerIndex.One);

            _objects.Add(_arin);
        }

        private int _drawHealthWidth = 86;

        public void draw()
        {
            //_gameInstance.spriteBatch.Draw(_gameInstance.textureManager.getResource("back"), new Rectangle(0, 0, 1440, 640), Color.White);

            foreach (LevelObject obj in _objects)
            {
                obj.draw();
            }

            Texture2D bars = _gameInstance.textureManager.getResource(@"UI\Bars");
            Texture2D arinHead = _gameInstance.textureManager.getResource(@"UI\Arin");

            int healthWidth = (int)(_arin.health / 100d * 86d);
            if (healthWidth < _drawHealthWidth)
            {
                _drawHealthWidth -= 2;
                if (_drawHealthWidth < healthWidth)
                {
                    _drawHealthWidth = healthWidth;
                }
            }

            _gameInstance.spriteBatch.DrawString(_grumpFont, "Arin    x3", new Vector2(120, 56), Color.Black);
            _gameInstance.spriteBatch.Draw(bars, new Rectangle(114, 74, 172, 16), new Rectangle(0, 12, 86, 8), Color.White);
            _gameInstance.spriteBatch.Draw(bars, new Rectangle(114, 74, _drawHealthWidth * 2, 16), new Rectangle(0, 0, _drawHealthWidth, 8), Color.White);
            _gameInstance.spriteBatch.Draw(bars, new Rectangle(114, 90, 172, 8), new Rectangle(0, 8, 86, 4), Color.White);

            if (_arin.state == GrumpSpace.ObjectState.HurtFalling || _arin.state == GrumpSpace.ObjectState.Hurt)
                _gameInstance.spriteBatch.Draw(arinHead, new Rectangle(34, 34, 96, 96), new Rectangle(48, 0, 48, 48), Color.White);
            else if (_arin.state == GrumpSpace.ObjectState.Dead)
                _gameInstance.spriteBatch.Draw(arinHead, new Rectangle(34, 34, 96, 96), new Rectangle(96, 0, 48, 48), Color.White);
            else
                _gameInstance.spriteBatch.Draw(arinHead, new Rectangle(34, 34, 96, 96), new Rectangle(0, 0, 48, 48), Color.White);


        }

        public void update()
        {
            foreach (LevelObject obj in _objects)
            {
                obj.update();
            }
        }
    }
}