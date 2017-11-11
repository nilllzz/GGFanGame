using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("knotPillar", "grumpSpace", "main")]
    class KnotPillar : SceneryObject
    {
        private float _height;

        public KnotPillar()
        {
            Collision = false;
            CanLandOn = false;
            GravityAffected = false;

            AddStaticAnimation(0, 40, 16, 8);
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            _height = dataModel.TryGetArg("height", 1f).result;
            Rotation = dataModel.TryGetArg("rotation", Vector3.Zero).result * (MathHelper.TwoPi / 360);
        }

        protected override void CreateGeometry()
        {
            var textures = new GeometryTextureCuboidWrapper();
            textures.AddSide(new[] { CuboidSide.Top, CuboidSide.Bottom },
                new GeometryTextureRectangle(new Rectangle(0, 0, 8, 8), new Rectangle(0, 0, 16, 8)));
            textures.AddSide(new[] { CuboidSide.Left, CuboidSide.Right, CuboidSide.Front, CuboidSide.Back },
                new GeometryTextureMultiplier(new Vector2(1f, _height * 20)));
            var vertices = CuboidComposer.Create(0.15f, _height, 0.15f, textures);

            Geometry.AddVertices(vertices);
        }
    }
}
