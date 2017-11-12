using GameDevCommon.Rendering;
using GGFanGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Screens.Game
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
            Position = CreatePosition();

            var gState = GamePad.GetState(PlayerIndex.One);
            Yaw += gState.ThumbSticks.Right.X * 0.1f;

            CreateView();
        }

        protected virtual Vector3 CreatePosition()
        {
            var offset = new Vector3(0, 0.75f * ZoomLevel + 0.25f, 2f * ZoomLevel);
            var mat = Matrix.CreateFromYawPitchRoll(Yaw, 0, 0);
            return Vector3.Transform(offset, mat) + new Vector3(FollowObject.X, 0, FollowObject.Z);
        }
    }
}
