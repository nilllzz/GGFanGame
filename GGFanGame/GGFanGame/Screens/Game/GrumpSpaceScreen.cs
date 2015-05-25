using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGFanGame.Screens.Game
{
    class GrumpSpaceScreen : Screen
    {

        private Level.Level _level;

        public GrumpSpaceScreen(GGGame game) : base(Identification.InGame, game)
        {
            _level = new Level.Level(game);
        }

        public override void draw()
        {
            _level.draw();
        }

        public override void update()
        {
            _level.update();
        }
    }
}