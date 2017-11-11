using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("steelPipe", "grumpSpace", "main")]
    class SteelPipe : SceneryObject
    {
        private float _height;

        public SteelPipe()
        {
            Collision = true;
            CanLandOn = true;
            GravityAffected = false;

            AddStaticAnimation(96, 16, 16, 16);
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            _height = dataModel.TryGetArg("height", 1f).result;
        }

        protected override void CreateGeometry()
        {
            var sideTexture = new GeometryTextureTubeWrapper(new Rectangle(0, 0, (int)(64 * _height), 16), new Rectangle(0, 0, 32, 32), 20);
            var vertices = TubeComposer.Create(0.2f, _height, 20, sideTexture);

            Geometry.AddVertices(vertices);
        }
    }
}
