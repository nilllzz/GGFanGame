using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GGFanGame.Game;
using static GameProvider;

namespace GGFanGame.Screens.Game
{
    /// <summary>
    /// The screen that is active during the Grump Space scenes.
    /// </summary>
    class GrumpSpaceScreen : Screen
    {
        private Stage _stage;

        public GrumpSpaceScreen()
        {
            initializeContentManager();

            _stage = new Stage(content);
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
                ScreenManager.getInstance().setScreen(new Debug.BoundingBoxTestScreen());
            }
            // Zoom out: Y
            if (Input.KeyboardHandler.keyDown(Microsoft.Xna.Framework.Input.Keys.Y) && _stage.camera.scale > 0.2)
            {
                _stage.camera.scale -= 0.01;
            }
            // Zoom in: X
            if (Input.KeyboardHandler.keyDown(Microsoft.Xna.Framework.Input.Keys.X))
            {
                _stage.camera.scale += 0.01;
            }
            // Zoom default: C
            if (Input.KeyboardHandler.keyPressed(Microsoft.Xna.Framework.Input.Keys.OemPipe))
            {
                _stage.camera.scale = 2;
            }
        }
    }
}