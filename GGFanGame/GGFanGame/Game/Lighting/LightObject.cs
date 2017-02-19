using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Lighting
{
    internal class LightObject : StageObject
    {
        public override Point GetDrawingSize() => Point.Zero;

        private readonly Cone _lightCone;

        public LightObject(Color color, Vector3 position, Vector3 target, float spreadAngle)
        {
            ObjectColor = color;
            Position = position;
            _lightCone = new Cone(position, target, spreadAngle);
        }

        public override void Draw(SpriteBatch batch) { }

        public override void Update() { }
    }
}
