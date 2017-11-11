using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("officeDoor", "grumpSpace", "main")]
    class OfficeDoor : SceneryObject
    {
        public OfficeDoor()
        {
            Collision = false;
            CanLandOn = false;
            GravityAffected = false;

            AddStaticAnimation(48, 0, 32, 39);
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        protected override void CreateGeometry()
        {
            // door
            {
                var vertices = RectangleComposer.Create(0.84375f, 1.21875f,
                    new GeometryTextureRectangle(new Rectangle(0, 0, 27, 39), new Rectangle(0, 0, 32, 39)));

                VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0, 0));
                VertexTransformer.Offset(vertices, new Vector3(0, 1.21875f / 2f, 0));

                Geometry.AddVertices(vertices);
            }
            // box
            {
                var vertices = CuboidComposer.Create(0.3f, 0.05f, 0.05f,
                    new GeometryTextureRectangle(new Rectangle(27, 0, 5, 2), new Rectangle(0, 0, 32, 39)));

                VertexTransformer.Offset(vertices, new Vector3(-0.2f, 1.15f, 0.05f));

                Geometry.AddVertices(vertices);
            }
        }
    }
}
