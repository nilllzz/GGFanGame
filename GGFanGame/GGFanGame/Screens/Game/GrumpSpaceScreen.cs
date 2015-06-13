using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GGFanGame.Game.Level;

namespace GGFanGame.Screens.Game
{
    /// <summary>
    /// The screen that is active during the Grump Space scenes.
    /// </summary>
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

            //TEST CODE: When pressed P, rendering switches to 3D bounding box test stage:
            if (Input.KeyboardHandler.keyPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                ScreenManager.getInstance().setScreen(new Debug.BoundingBoxTestScreen(gameInstance));
            }
            // Zoom out: [
            if (Input.KeyboardHandler.keyPressed(Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets) && _stage.scale > 0.2)
            {
                _stage.scale -= 0.1;
            }
            // Zoom in: ]
            if (Input.KeyboardHandler.keyPressed(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets))
            {
                _stage.scale += 0.1;
            }
            // Zoom default: \
            if (Input.KeyboardHandler.keyPressed(Microsoft.Xna.Framework.Input.Keys.OemPipe))
            {
                _stage.scale = 2;
            }
        }
    }
}