using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game.Scene
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
                var width = gameInstance.random.Next(8, 31);
                var height = gameInstance.random.Next(8, 31);
                var x = gameInstance.random.Next(0, 64 - width);
                var y = gameInstance.random.Next(0, 64 - height);

                ellipses.Add(new Rectangle(x, y, width, height));
                colors.Add(color);
            }

            spriteSheet = Drawing.Graphics.createJoinedEllipse(
                64,
                64,
                ellipses.ToArray(),
                colors.ToArray()
            );

            objectColor = color;
            sortLowest = true;
            canInteract = false;

            if (gameInstance.random.Next(0, 2) == 0)
            {
                _effect = SpriteEffects.FlipHorizontally;
            }

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(128, 128), 100));
        }

        public override void draw()
        {
            var frame = getAnimation().getFrameRec(animationFrame);
            var stageScale = Stage.activeStage.camera.scale;

            var shadowWidth = (int)(spriteSheet.Width);
            var shadowHeight = (int)(spriteSheet.Height * (1d / 4d));

            gameInstance.spriteBatch.Draw(spriteSheet, new Rectangle((int)((X - shadowWidth / 2d) * stageScale),
                            (int)((Z - shadowHeight / 2d - Y) * stageScale),
                            (int)(shadowWidth * stageScale),
                            (int)(shadowHeight * stageScale)), null, new Color(255, 255, 255, _alpha), 0f, Vector2.Zero, _effect, 0f);
        }

        public override void update()
        {
            base.update();

            _alpha--;
            if (_alpha < 0)
            {
                _alpha = 0;
                canBeRemoved = true;
            }
        }
    }
}