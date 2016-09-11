﻿using GGFanGame.Drawing;
using GGFanGame.Game;
using GGFanGame.Game.HUD;
using GGFanGame.Input;
using GGFanGame.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static GameProvider;

namespace GGFanGame.Screens.Game
{
    /// <summary>
    /// Main screen to play stages in.
    /// </summary>
    internal class StageScreen : Screen
    {
        private Stage _stage;

        private PlayerStatus _oneStatus, _twoStatus, _threeStatus, _fourStatus;

        public StageScreen()
        {
            _stage = new Stage();
            _stage.setActiveStage();

            _oneStatus = new PlayerStatus(_stage.onePlayer, PlayerIndex.One);
            _twoStatus = new PlayerStatus(_stage.twoPlayer, PlayerIndex.Two);
            _threeStatus = new PlayerStatus(_stage.threePlayer, PlayerIndex.Three);
            _fourStatus = new PlayerStatus(_stage.fourPlayer, PlayerIndex.Four);
        }

        public override void draw()
        {
            // seperate these so they can be called seperately from the pause screen.
            drawStage();
            drawHUD();
        }

        internal void drawStage()
        {
            Graphics.drawRectangle(gameInstance.clientRectangle, Color.CornflowerBlue);
            _stage.draw();
        }

        internal void drawHUD()
        {
            _oneStatus.draw();
            _twoStatus.draw();
            _threeStatus.draw();
            _fourStatus.draw();
        }

        public override void update()
        {
            _stage.update();
            
            if (GamePadHandler.buttonPressed(PlayerIndex.One, Buttons.Start))
            {
                ScreenManager.getInstance().setScreen(new PauseScreen(this));
            }

            //TEST CODE: When pressed P, rendering switches to 3D bounding box test stage:
            if (KeyboardHandler.keyPressed(Keys.P))
            {
                ScreenManager.getInstance().setScreen(new Debug.BoundingBoxTestScreen());
            }
            // Zoom out: Y
            if (KeyboardHandler.keyDown(Keys.Y) && _stage.camera.scale > 0.2)
            {
                _stage.camera.scale -= 0.01;
            }
            // Zoom in: X
            if (KeyboardHandler.keyDown(Keys.X))
            {
                _stage.camera.scale += 0.01;
            }
            // Zoom default: C
            if (KeyboardHandler.keyPressed(Keys.OemPipe))
            {
                _stage.camera.scale = 2;
            }
        }
    }
}
