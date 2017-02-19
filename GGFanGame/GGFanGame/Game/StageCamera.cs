using GGFanGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Game
{
    internal class StageCamera : Camera
    {
        private readonly StageObject _followObject;
        
        public StageCamera(StageObject followObject)
        {
            _followObject = followObject;

            Yaw = 0f;
            Pitch = -0.2f;
            CreatePosition();

            CreateView();
            CreateProjection();
        }

        public override void Update()
        {
            CreatePosition();

            var gState = GamePad.GetState(PlayerIndex.One);
            Yaw += gState.ThumbSticks.Right.X * 0.1f;

            CreateView();
        }

        private void CreatePosition()
        {
            Position = new Vector3(_followObject.X, 64f, _followObject.Z + 150f);
        }
    }
}
