using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("albumAward", "grumpSpace", "main")]
    class AlbumAward : SceneryObject
    {
        public AlbumAward()
        {
            Collision = false;
            CanLandOn = false;
            GravityAffected = false;
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            if (dataModel.HasArg("2"))
            {
                AddStaticAnimation(148, 0, 20, 34);
            }
            else
            {
                AddStaticAnimation(128, 0, 20, 34);
            }
        }

        protected override void CreateGeometry()
        {
            var texture = new GeometryTextureCuboidWrapper();
            var textureRect = new Rectangle(0, 0, 20, 34);
            texture.AddSide(new[] { CuboidSide.Front, CuboidSide.Back },
                DefaultGeometryTextureDefinition.Instance);
            texture.AddSide(new[] { CuboidSide.Left, CuboidSide.Right, CuboidSide.Top, CuboidSide.Bottom },
                new GeometryTextureRectangle(new Rectangle(0, 0, 1, 1), textureRect));
            var vertices = CuboidComposer.Create(0.2f, 0.3f, 0.02f, texture);
            VertexTransformer.Rotate(vertices, new Vector3(0, MathHelper.PiOver2, 0));
            Geometry.AddVertices(vertices);
        }
    }
}
