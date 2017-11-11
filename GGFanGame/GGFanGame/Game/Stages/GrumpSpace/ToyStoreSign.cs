using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("toyStoreSign", "grumpSpace", "main")]
    class ToyStoreSign : SceneryObject
    {
        public ToyStoreSign()
        {
            Collision = true;
            CanLandOn = false;
            GravityAffected = false;

            Size = new Vector3(0.5f, 2f, 0.2f);
            Rotation = new Vector3(0, -0.2f, 0);
            AddStaticAnimation(80, 0, 16, 35);

            Tag = "Bloom";
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
            ParentStage.AddPointLight(new Drawing.PointLight(new Vector3(0, 0.6f, 0.2f) + Position, new Color(64, 64, 255), 3, 5));
        }

        protected override void CreateGeometry()
        {
            var texture = new GeometryTextureCuboidWrapper();
            var textureRect = new Rectangle(0, 0, 16, 35);
            texture.AddSide(new[] { CuboidSide.Front, CuboidSide.Back },
                DefaultGeometryTextureDefinition.Instance);
            texture.AddSide(new[] { CuboidSide.Left, CuboidSide.Right, CuboidSide.Top, CuboidSide.Bottom },
                new GeometryTextureRectangle(new Rectangle(0, 0, 1, 1), textureRect));
            var vertices = CuboidComposer.Create(0.5f, 1.2f, 0.1f, texture);
            GameDevCommon.Rendering.VertexTransformer.Offset(vertices, new Vector3(0, 0.6f, 0));
            Geometry.AddVertices(vertices);
        }
    }
}
