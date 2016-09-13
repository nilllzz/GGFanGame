using GGFanGame.DataModel.Json.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Stage1_1
{
    [StageObject("street", "1", "1")]
    internal class Street : SceneryObject
    {
        public Street()
        {
            size = new Vector3(64, 1, 32);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            gravityAffected = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 32), 100));
            groundRelation = GroundRelation.Flat;
            //sortLowest = true;
        }

        protected override void loadInternal()
        {
            spriteSheet = content.Load<Texture2D>(@"Levels\Stage1_1\Street");
        }
    }
}
