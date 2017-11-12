using GGFanGame.Game;
using Microsoft.Xna.Framework;

namespace GGFanGame.Screens.Game
{
    class GrumpSpaceCamera : StageCamera
    {
        public GrumpSpaceCamera(StageObject followObject)
            : base(followObject)
        { }

        protected override Vector3 CreatePosition()
        {
            var pos = base.CreatePosition();

            if (pos.X < -3.5f)
                pos.X = -3.5f;

            return pos;
        }
    }
}
