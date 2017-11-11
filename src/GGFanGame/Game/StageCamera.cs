using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static Core;

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
            FOV = 60;

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
            var offset = new Vector3(0, 0.75f * ZoomLevel + 0.25f, 2f * ZoomLevel);
            var mat = Matrix.CreateFromYawPitchRoll(Yaw, 0, 0);
            Position = Vector3.Transform(offset, mat) + new Vector3(FollowObject.X, 0, FollowObject.Z);

            //Position = new Vector3(FollowObject.X, 0.75f * ZoomLevel + 0.25f, FollowObject.Z + 2f * ZoomLevel);
        }
    }
}
