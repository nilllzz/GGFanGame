using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Stage1_1
{
    [StageObject("street", "1", "1")]
    internal class Street : SceneryObject
    {
        public Street()
        {
            Size = new Vector3(64, 1, 32);
            DrawShadow = false;
            Collision = true;
            CanLandOn = true;
            GravityAffected = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 32), 100));
            GroundRelation = GroundRelation.Flat;
            //sortLowest = true;
        }

        protected override void LoadInternal()
        {
            SpriteSheet = Content.Load<Texture2D>(@"Levels\Stage1_1\Street");
        }
    }
}
