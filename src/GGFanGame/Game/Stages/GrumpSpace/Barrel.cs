using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("barrel", "grumpSpace", "main")]
    class Barrel : SceneryObject
    {
        public Barrel()
        {
            Collision = true;
            CanLandOn = true;
            GravityAffected = false;
            Size = new Vector3(0.3f, 0.4f, 0.3f);

            AddStaticAnimation(96, 0, 32, 16);
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        protected override void CreateGeometry()
        {
            var sideTexture = new GeometryTextureTubeWrapper(new Rectangle(0, 0, 16, 64), new Rectangle(0, 0, 32, 32), 20);
            var topTexture = new GeometryTextureRectangle(new Rectangle(16, 0, 16, 16), new Rectangle(0, 0, 32, 32));
            var vertices = CylinderComposer.Create(0.2f, 0.4f, 20, sideTexture, topTexture);

            VertexTransformer.Rotate(vertices, new Vector3(0, 0, MathHelper.PiOver2));
            VertexTransformer.Offset(vertices, new Vector3(0, 0.2f, 0));

            Geometry.AddVertices(vertices);
        }
    }
}
