using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("woodenFloor", "grumpSpace", "main")]
    internal class WoodenFloor : SceneryObject
    {
        public WoodenFloor()
        {
            Size = new Vector3(1, 1 / 32f, 1);
            Collision = false;
            CanLandOn = true;
            GravityAffected = false;

            AddStaticAnimation(0, 0, 16, 16);
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            (_, var size) = dataModel.TryGetArg("size", 1);
            Size = new Vector3(size, 1 / 32f, size);
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(RectangleComposer.Create(Size.X, Size.Z,
                new GeometryTextureMultiplier(new Vector2(Size.X * 2f, Size.Z * 2f))));
        }
    }
}
