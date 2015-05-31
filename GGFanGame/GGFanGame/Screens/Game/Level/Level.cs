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
        private HUD.PlayerStatus _oneStatus;

        public Level(GGGame game)
        {
            _gameInstance = game;

            _objects = new List<LevelObject>();

            _arin = new GrumpSpace.Arin(game, PlayerIndex.One);
            _oneStatus = new HUD.PlayerStatus(game, _arin, PlayerIndex.One);

            _objects.Add(_arin);
        }

        public void draw()
        {
            foreach (LevelObject obj in _objects)
            {
                obj.draw();
            }

            _oneStatus.draw();
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