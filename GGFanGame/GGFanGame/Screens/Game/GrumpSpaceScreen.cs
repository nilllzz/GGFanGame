using Microsoft.Xna.Framework;
using GGFanGame.Game;
using static GameProvider;

namespace GGFanGame.Screens.Game
{
    /// <summary>
    /// The screen that is active during the Grump Space scenes.
    /// </summary>
    internal class GrumpSpaceScreen : Screen
    {
        private readonly Stage _stage;

        public GrumpSpaceScreen()
        {
            _stage = new Stage(Content, null, null);
            _stage.SetActiveStage();
        }

        public override void Draw()
        {
            Drawing.Graphics.DrawRectangle(GameInstance.ClientRectangle, Color.CornflowerBlue);
            _stage.Draw();
        }

        public override void Update()
        {
            _stage.Update();

            //TEST CODE: When pressed P, rendering switches to 3D bounding box test stage:
            if (Input.KeyboardHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                ScreenManager.GetInstance().SetScreen(new Debug.BoundingBoxTestScreen());
            }
            // Zoom out: Y
            if (Input.KeyboardHandler.KeyDown(Microsoft.Xna.Framework.Input.Keys.Y) && _stage.Camera.Scale > 0.2)
            {
                _stage.Camera.Scale -= 0.01;
            }
            // Zoom in: X
            if (Input.KeyboardHandler.KeyDown(Microsoft.Xna.Framework.Input.Keys.X))
            {
                _stage.Camera.Scale += 0.01;
            }
            // Zoom default: C
            if (Input.KeyboardHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.OemPipe))
            {
                _stage.Camera.Scale = 2;
            }
        }
    }
}