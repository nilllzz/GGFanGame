using System.Collections.Generic;
using GGFanGame.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages
{
    /// <summary>
    /// A splat on the ground.
    /// </summary>
    internal class GroundSplat : InteractableStageObject
    {
        private SpriteEffects _effect = SpriteEffects.None;
        private int _alpha = 255;

        public GroundSplat(Color color)
        {
            ObjectColor = color;

            SortLowest = true;
            CanInteract = false;
            
            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(128, 128), 100));
        }

        protected override void LoadInternal()
        {
            var ellipses = new List<Rectangle>();
            var colors = new List<Color>();
            var random = ParentStage.Random;

            for (var i = 0; i < 8; i++)
            {
                var width = random.Next(8, 31);
                var height = random.Next(8, 31);
                var x = random.Next(0, 64 - width);
                var y = random.Next(0, 64 - height);

                ellipses.Add(new Rectangle(x, y, width, height));
                colors.Add(ObjectColor);
            }

            SpriteSheet = SpriteBatchExtensions.CreateJoinedEllipse(
                64,
                64,
                ellipses.ToArray(),
                colors.ToArray()
            );

            if (random.Next(0, 2) == 0)
            {
                _effect = SpriteEffects.FlipHorizontally;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            var frame = GetAnimation().GetFrameRec(AnimationFrame);
            var stageScale = ParentStage.Camera.Scale;

            var shadowWidth = (int)(SpriteSheet.Width);
            var shadowHeight = (int)(SpriteSheet.Height * (1d / 4d));

            batch.Draw(SpriteSheet, new Rectangle((int)((X - shadowWidth / 2d) * stageScale),
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