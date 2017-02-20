using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Lighting
{
    internal class LightObject : StageObject
    {
        private readonly Cone _lightCone;

        public LightObject(Color color, Vector3 position, Vector3 target, float spreadAngle)
        {
            ObjectColor = color;
            Position = position;
            _lightCone = new Cone(position, target, spreadAngle);
        }

        public void Draw(SpriteBatch batch) { }

        public override void Update() { }
    }
}
