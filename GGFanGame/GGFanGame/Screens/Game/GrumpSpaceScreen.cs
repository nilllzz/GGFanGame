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
        private Stage _stage;

        public GrumpSpaceScreen(GGGame game) : base(Identification.InGame, game)
        {
            _stage = new Stage(game);
            _stage.setActiveStage();
        }

        public override void draw()
        {
            Drawing.Graphics.drawRectangle(gameInstance.clientRectangle, Color.CornflowerBlue);
            _stage.draw();
        }

        public override void update()
        {
            _stage.update();
        }
    }
}