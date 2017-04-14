using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using GGFanGame.Rendering;
using GGFanGame.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Dojo
{
    [StageObject("wall", "grumpSpace", "dojo")]
    internal class Wall : SceneryObject
    {
        private bool _rotated = false;

        public Wall()
        {
            Size = new Vector3(64, 128, 16);
            DrawShadow = false;
            Collision = true;
            GravityAffected = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 128), 100));
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            _rotated = dataModel.HasArg("rotation");
            if (_rotated)
            {
                Size = new Vector3(16, 128, 64);
            }
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet1 = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.Dojo.Wall1));
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(1f, 2f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            VertexTransformer.Offset(vertices, new Vector3(0, 1f, 0));

            if (_rotated)
            {
                VertexTransformer.Rotate(vertices, new Vector3(0f, MathHelper.PiOver2, 0f));
            }

            Geometry.AddVertices(vertices);
        }
    }
}
