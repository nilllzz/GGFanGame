using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Scene.Level1_1
{
    internal class Street : SceneryObject
    {
        public Street()
        {
            spriteSheet = content.Load<Texture2D>(@"Levels\Stage1-1\Street");
            size = new Vector3(64, 1, 32);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            gravityAffected = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 32), 100));
            groundRelation = GroundRelation.Flat;
            //sortLowest = true;
        }
    }
}
