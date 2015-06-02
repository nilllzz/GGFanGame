using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GGFanGame.Game.Level;

namespace GGFanGame.Screens.Game
{
    class GrumpSpaceScreen : Screen
    {

        private Stage _level;

        public GrumpSpaceScreen(GGGame game) : base(Identification.InGame, game)
        {
            _level = new Stage(game);
            _level.setActiveStage();
        }

        public override void draw()
        {
            Drawing.Graphics.drawRectangle(gameInstance.clientRectangle, Color.CornflowerBlue);
            _level.draw();
        }

        public override void update()
        {
            _level.update();
        }
    }
}