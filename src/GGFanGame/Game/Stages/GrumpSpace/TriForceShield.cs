using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("triForceShield", "grumpSpace", "main")]
    class TriForceShield : SceneryObject
    {
        public TriForceShield()
        {
            Collision = false;
            CanLandOn = false;
            GravityAffected = false;
            IsOpaque = false;

            AddStaticAnimation(16, 48, 13, 16);
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(0.36f, 0.48f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            Geometry.AddVertices(vertices);
        }
    }
}
