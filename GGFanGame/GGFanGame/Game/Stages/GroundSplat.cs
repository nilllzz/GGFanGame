using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game.Stages
{
    /// <summary>
    /// A splat on the ground.
    /// </summary>
    internal class GroundSplat : InteractableStageObject
    {
        private readonly SpriteEffects _effect = SpriteEffects.None;
        private int _alpha = 255;

        public GroundSplat(Color color)
        {
            var ellipses = new List<Rectangle>();
            var colors = new List<Color>();

            for (var i = 0; i < 8; i++)
            {
                var width = GameInstance.Random.Next(8, 31);
                var height = GameInstance.Random.Next(8, 31);
                var x = GameInstance.Random.Next(0, 64 - width);
                var y = GameInstance.Random.Next(0, 64 - height);

                ellipses.Add(new Rectangle(x, y, width, height));
                colors.Add(color);
            }

            SpriteSheet = Drawing.Graphics.CreateJoinedEllipse(
                64,
                64,
                ellipses.ToArray(),
                colors.ToArray()
            );

            ObjectColor = color;
            SortLowest = true;
            CanInteract = false;

            if (GameInstance.Random.Next(0, 2) == 0)
            {
                _effect = SpriteEffects.FlipHorizontally;
            }

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(128, 128), 100));
        }

        public override void Draw()
        {
            var frame = GetAnimation().GetFrameRec(AnimationFrame);
            var stageScale = Stage.ActiveStage.Camera.Scale;

            var shadowWidth = (int)(SpriteSheet.Width);
            var shadowHeight = (int)(SpriteSheet.Height * (1d / 4d));

            GameInstance.SpriteBatch.Draw(SpriteSheet, new Rectangle((int)((X - shadowWidth / 2d) * stageScale),
                            (int)((Z - shadowHeight / 2d - Y) * stageScale),
                            (int)(shadowWidth * stageScale),
                            (int)(shadowHeight * stageScale)), null, new Color(255, 255, 255, _alpha), 0f, Vector2.Zero, _effect, 0f);
        }

        public override void Update()
        {
            base.Update();

            _alpha--;
            if (_alpha < 0)
            {
                _alpha = 0;
                CanBeRemoved = true;
            }
        }
    }
}