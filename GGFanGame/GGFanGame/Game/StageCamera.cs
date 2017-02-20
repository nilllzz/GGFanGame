using GGFanGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Game
{
    internal class StageCamera : Camera
    {
        internal StageObject FollowObject { get; set; }
        internal float ZoomLevel { get; set; } = 1f;

        public StageCamera(StageObject followObject)
        {
            FollowObject = followObject;

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
            Position = new Vector3(FollowObject.X, 48f * ZoomLevel + 16, FollowObject.Z + 128f * ZoomLevel);
        }
    }
}
