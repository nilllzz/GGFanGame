using GGFanGame.Content;
using GameDevCommon.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Dojo
{
    [StageObject("floorCenter", "grumpSpace", "dojo")]
    internal class FloorCenter : SceneryObject
    {
        public FloorCenter()
        {
            Size = new Vector3(64, 1, 64);
            Collision = true;
            CanLandOn = true;
            GravityAffected = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 64), 100));
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.Dojo.Floor1));
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(RectangleComposer.Create(1f, 1f));
        }
    }
}
