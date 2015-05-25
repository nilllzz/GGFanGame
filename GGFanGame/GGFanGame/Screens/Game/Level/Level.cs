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

        public Level(GGGame game)
        {
            _gameInstance = game;
            _objects = new List<LevelObject>();

            _objects.Add(new GrumpSpace.Danny(game));
        }

        public void draw()
        {
            foreach (LevelObject obj in _objects)
            {
                obj.draw();
            }
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