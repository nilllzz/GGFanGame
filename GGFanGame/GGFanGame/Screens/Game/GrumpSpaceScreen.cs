using GGFanGame.Game;
using GGFanGame.Input;
using static Core;

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
        }
    }
}
