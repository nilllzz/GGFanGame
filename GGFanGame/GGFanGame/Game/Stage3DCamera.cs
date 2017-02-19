using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGFanGame.Rendering;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    internal class Stage3DCamera : Camera
    {
        public Stage3DCamera()
        {
            Position = new Vector3(0f, 0f, 4f);
            Yaw = 0f;
            Pitch = 0f;

            CreateView();
            CreateProjection();
        }

        public override void Update()
        {

        }
    }
}
