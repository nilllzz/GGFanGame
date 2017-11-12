using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("koinobori", "grumpSpace", "main")]
    class Koinobori : SceneryObject
    {
        public Koinobori()
        {
            Collision = false;
            CanLandOn = false;
            GravityAffected = false;
            IsOpaque = false;

            AddStaticAnimation(168, 0, 32, 33);
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        protected override void CreateGeometry()
        {
            var textureRect = new Rectangle(0, 0, 32, 33);
            // pole
            {
                var vertices = CylinderComposer.Create(0.01f, 0.6f, 10,
                    new GeometryTextureRectangle(new Rectangle(0, 32, 32, 1), textureRect),
                    new GeometryTextureRectangle(new Rectangle(0, 0, 1, 1), textureRect));

                VertexTransformer.Rotate(vertices, new Vector3(0, 0, MathHelper.PiOver4));
                VertexTransformer.Rotate(vertices, new Vector3(0, -MathHelper.PiOver2, 0));
                Geometry.AddVertices(vertices);
            }
            // flag 1
            {
                var vertices = RectangleComposer.Create(0.1f, 0.4f,
                    new GeometryTextureRectangle(new Rectangle(0, 0, 8, 32), textureRect));

                VertexTransformer.Rotate(vertices, new Vector3(0, 0, MathHelper.PiOver2));
                VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0, 0));
                VertexTransformer.Offset(vertices, new Vector3(0, -0.25f, 0));
                Geometry.AddVertices(vertices);
            }
            // flag 2
            {
                var vertices = RectangleComposer.Create(0.1f, 0.4f,
                    new GeometryTextureRectangle(new Rectangle(8, 0, 8, 32), textureRect));

                VertexTransformer.Rotate(vertices, new Vector3(0, 0, MathHelper.PiOver2));
                VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0, 0));
                VertexTransformer.Offset(vertices, new Vector3(0, -0.05f, 0.2f));
                Geometry.AddVertices(vertices);
            }
        }
    }
}
