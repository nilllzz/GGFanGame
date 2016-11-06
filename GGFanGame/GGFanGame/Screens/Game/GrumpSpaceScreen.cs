using Microsoft.Xna.Framework;
using GGFanGame.Game;
using static Core;
using GGFanGame.Input;

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
            //Drawing.Graphics.DrawRectangle(GameInstance.ClientRectangle, Color.CornflowerBlue);
            _stage.Draw(null);
        }

        public override void Update()
        {
            _stage.Update();

            //TEST CODE: When pressed P, rendering switches to 3D bounding box test stage:
            if (GetComponent<KeyboardHandler>().KeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                GetComponent<ScreenManager>().SetScreen(new Debug.BoundingBoxTestScreen());
            }
            // Zoom out: Y
            if (GetComponent<KeyboardHandler>().KeyDown(Microsoft.Xna.Framework.Input.Keys.Y) && _stage.Camera.Scale > 0.2)
            {
                _stage.Camera.Scale -= 0.01;
            }
            // Zoom in: X
            if (GetComponent<KeyboardHandler>().KeyDown(Microsoft.Xna.Framework.Input.Keys.X))
            {
                _stage.Camera.Scale += 0.01;
            }
            // Zoom default: C
            if (GetComponent<KeyboardHandler>().KeyPressed(Microsoft.Xna.Framework.Input.Keys.OemPipe))
            {
                _stage.Camera.Scale = 2;
            }
        }
    }
}