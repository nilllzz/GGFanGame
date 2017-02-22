using GGFanGame.Content;
using GGFanGame.Rendering;
using GGFanGame.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Dojo
{
    [StageObject("lightBeam", "grumpSpace", "dojo")]
    internal class LightBeam : SceneryObject
    {
        public LightBeam()
        {
            Size = new Vector3(256, 128, 1);
            DrawShadow = false;
            Collision = false;
            GravityAffected = false;
            BlendState = BlendState.Additive;
            IsOpaque = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(256, 128), 100));
        }
        
        protected override void LoadContentInternal()
        {
            SpriteSheet1 = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.Dojo.LightBeam));
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(256f, 128f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            VertexTransformer.Offset(vertices, new Vector3(0, 64, 0));
            Geometry.AddVertices(vertices);
        }
    }
}
