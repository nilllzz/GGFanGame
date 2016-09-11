using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Scene.Level1_1
{
    internal class BridgeBottom : SceneryObject
    {
        public BridgeBottom()
        {
            spriteSheet = content.Load<Texture2D>(@"Levels\Stage1-1\BridgeBottom");
            size = new Vector3(128, 64, 64);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            sortLowest = true;
            gravityAffected = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(128, 64), 100));

        }
    }
}
