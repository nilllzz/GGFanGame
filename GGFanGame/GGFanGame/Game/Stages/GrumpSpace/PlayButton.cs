using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("playButton", "grumpSpace", "main")]
    class PlayButton : SceneryObject
    {
        public PlayButton()
        {
            Collision = false;
            CanLandOn = false;
            GravityAffected = false;

            AddStaticAnimation(0, 16, 16, 24);
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.GrumpSpace.Main));
        }

        protected override void CreateGeometry()
        {
            var texture = new GeometryTextureCuboidWrapper();
            var textureRect = new Rectangle(0, 0, 16, 24);
            texture.AddSide(new[] { CuboidSide.Front, CuboidSide.Back },
                DefaultGeometryTextureDefinition.Instance);
            texture.AddSide(new[] { CuboidSide.Left },
                new GeometryTextureRectangle(new Rectangle(0, 0, 1, 24), textureRect));
            texture.AddSide(new[] { CuboidSide.Right },
                new GeometryTextureRectangle(new Rectangle(15, 0, 1, 24), textureRect));
            texture.AddSide(new[] { CuboidSide.Top },
                new GeometryTextureRectangle(new Rectangle(0, 0, 16, 1), textureRect));
            texture.AddSide(new[] { CuboidSide.Bottom },
                new GeometryTextureRectangle(new Rectangle(0, 23, 16, 1), textureRect));
            var vertices = CuboidComposer.Create(0.5f, 0.7f, 0.1f, texture);
            Geometry.AddVertices(vertices);
        }
    }
}
