using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Stage1_1
{
    [StageObject("bridgeRailing", "1", "1")]
    internal class BridgeRailing : SceneryObject
    {
        public BridgeRailing()
        {
            size = new Vector3(64, 32, 5);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            gravityAffected = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 32), 100));

        }

        protected override void loadInternal()
        {
            spriteSheet = content.Load<Texture2D>(@"Levels\Stage1_1\BridgeRailing");
        }
    }
}
