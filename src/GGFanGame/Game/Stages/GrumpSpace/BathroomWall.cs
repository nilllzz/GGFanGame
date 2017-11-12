using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameDevCommon.Rendering.Texture;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("bathroomWall", "grumpSpace", "main")]
    internal class BathroomWall : SceneryObject
    {
        private bool _rotated;
        private float _size;

        public BathroomWall()
        {
            Size = new Vector3(1, 2f, 0.25f);
            Collision = true;
            GravityAffected = false;

            AddStaticAnimation(184, 0, 8, 8);
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            _size = dataModel.TryGetArg("size", 1f).result;
            Size = new Vector3(_size, Size.Y, Size.Z);
            _rotated = dataModel.HasArg("rotation");
            if (_rotated)
            {
                Size = new Vector3(Size.Z, Size.Y, Size.X);
            }
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(_size, 1.5f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            VertexTransformer.Offset(vertices, new Vector3(0, 0.75f, 0));

            if (_rotated)
            {
                VertexTransformer.Rotate(vertices, new Vector3(0f, MathHelper.PiOver2, 0f));
            }

            Geometry.AddVertices(vertices);
        }
    }
}
