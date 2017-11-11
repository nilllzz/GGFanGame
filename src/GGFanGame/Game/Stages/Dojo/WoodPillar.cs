using GGFanGame.Content;
using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Dojo
{
    [StageObject("woodPillar", "grumpSpace", "dojo")]
    internal class WoodPillar : SceneryObject
    {
        public WoodPillar()
        {
            Size = new Vector3(16, 64, 16);
            Collision = true;
            GravityAffected = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(16, 64), 100));
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.Dojo.WoodPillar));
        }

        protected override void CreateGeometry()
        {
            var vertices = CuboidComposer.Create(0.25f, 1f, 0.25f);
            VertexTransformer.Offset(vertices, new Vector3(0, 0.5f, 0));
            Geometry.AddVertices(vertices);
        }
    }
}
