using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("couch", "0", "1")]
    internal class Couch : SceneryObject
    {
        public Couch()
        {
            Size = new Vector3(80, 16, 5);
            DrawShadow = true;
            Collision = true;
            CanLandOn = true;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(81, 36), 100));

            AddBoundingBox(new Vector3(12, 28, 12), new Vector3(-34, 14, 0)); //Left arm
            AddBoundingBox(new Vector3(57, 16, 6), new Vector3(0, 8, 3)); //Center seat
            AddBoundingBox(new Vector3(57, 28, 6), new Vector3(0, 14, -3)); //Back arm
            AddBoundingBox(new Vector3(12, 28, 12), new Vector3(34, 14, 0)); //Right arm 

            ZSortingOffset = -6; // -(boundingBox.Z / 2) - offset.Z
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Couch);
        }
    }
}
