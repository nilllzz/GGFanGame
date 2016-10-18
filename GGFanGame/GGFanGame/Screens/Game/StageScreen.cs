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
        private readonly Stage _stage;
        private readonly PlayerStatus _oneStatus, _twoStatus, _threeStatus, _fourStatus;

        public StageScreen()
        {
            _stage = StageFactory.Create(Content, "1", "1");
            _stage.Load();

            _oneStatus = new PlayerStatus(_stage.OnePlayer, PlayerIndex.One, Content);
            _twoStatus = new PlayerStatus(_stage.TwoPlayer, PlayerIndex.Two, Content);
            _threeStatus = new PlayerStatus(_stage.ThreePlayer, PlayerIndex.Three, Content);
            _fourStatus = new PlayerStatus(_stage.FourPlayer, PlayerIndex.Four, Content);
        }

        public override void Draw()
        {
            // seperate these so they can be called seperately from the pause screen.
            DrawStage();
            DrawHUD();
        }

        internal void DrawStage()
        {
            Graphics.DrawRectangle(GameInstance.ClientRectangle, _stage.BackColor);

            _stage.Draw();
        }

        internal void DrawHUD()
        {
            _oneStatus.Draw();
            _twoStatus.Draw();
            _threeStatus.Draw();
            _fourStatus.Draw();
        }

        public override void Update()
        {
            _stage.Update();
            
            if (GamePadHandler.ButtonPressed(PlayerIndex.One, Buttons.Start))
            {
                ScreenManager.GetInstance().SetScreen(new PauseScreen(this));
            }

            //TEST CODE: When pressed P, rendering switches to 3D bounding box test stage:
            if (KeyboardHandler.KeyPressed(Keys.P))
            {
                ScreenManager.GetInstance().SetScreen(new Debug.BoundingBoxTestScreen());
            }
            // Zoom out: Y
            if (KeyboardHandler.KeyDown(Keys.Y) && _stage.Camera.Scale > 0.2)
            {
                _stage.Camera.Scale -= 0.01;
            }
            // Zoom in: X
            if (KeyboardHandler.KeyDown(Keys.X))
            {
                _stage.Camera.Scale += 0.01;
            }
            // Zoom default: C
            if (KeyboardHandler.KeyPressed(Keys.OemPipe))
            {
                _stage.Camera.Scale = 2;
            }
        }
    }
}
