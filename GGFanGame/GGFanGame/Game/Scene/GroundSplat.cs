using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game.Scene
{
    /// <summary>
    /// A splat on the ground.
    /// </summary>
    class GroundSplat : InteractableStageObject
    {
        private SpriteEffects _effect = SpriteEffects.None;
        private int alpha = 255;

        public GroundSplat(Color color)
        {
            List<Rectangle> ellipses = new List<Rectangle>();
            List<Color> colors = new List<Color>();

            for (int i = 0; i < 8; i++)
            {
                int width = gameInstance.random.Next(8, 31);
                int height = gameInstance.random.Next(8, 31);
                int x = gameInstance.random.Next(0, 64 - width);
                int y = gameInstance.random.Next(0, 64 - height);

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
            Rectangle frame = getAnimation().getFrameRec(animationFrame);
            double stageScale = Stage.activeStage().camera.scale;

            int shadowWidth = (int)(spriteSheet.Width);
            int shadowHeight = (int)(spriteSheet.Height * (1d / 4d));

            gameInstance.spriteBatch.Draw(spriteSheet, new Rectangle((int)((X - shadowWidth / 2d) * stageScale),
                            (int)((Z - shadowHeight / 2d - Y) * stageScale),
                            (int)(shadowWidth * stageScale),
                            (int)(shadowHeight * stageScale)), null, new Color(255, 255, 255, alpha), 0f, Vector2.Zero, _effect, 0f);
        }

        public override void update()
        {
            base.update();

            alpha--;
            if (alpha < 0)
            {
                alpha = 0;
                canBeRemoved = true;
            }
        }
    }
}