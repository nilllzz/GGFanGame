using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Screens.Menu
{
    class TestScreen : Screen
    {
        public TestScreen(GGGame game) : base(Identification.Test, game) { }

        float ggoffsetX = 0;
        float ggoffsetY = 0;
        float stoffsetX = 0;
        float stoffsetY = 0;

        public override void draw()
        {
            //TESTS FOR THE DRAWING CLASS:
            //Game grumps:

            //Steam train:

            if (fadeLeft >= 0.5)
            {
                drawSteamTrain();
                drawGrumps();
            }
            else
            {
                drawGrumps();
                drawSteamTrain();
            }


        }

        private void drawGrumps()
        {
            int extraOffsetX = (int)((100 * fadeLeft) - (100 * fadeRight));

            Drawing.Graphics.drawGradient(new Rectangle(0, 0, 400 + extraOffsetX, 480), new Color(244, 131, 55), new Color(244, 170, 73), false, -1, 1d);

            for (int x = -6; x < 19; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    int posX = (int)(x * 24 + y * 8 + ggoffsetX);
                    int posY = (int)(y * 24 - (x * 8) + ggoffsetY);

                    double colorShift = (double)posY / 480;
                    double cR = 4 * colorShift;
                    double cG = 35 * colorShift;
                    double cB = 20 * colorShift;

                    double cA = 255;
                    if (posX > 270 + extraOffsetX)
                    {
                        cA = 255 - ((posX - (270 + extraOffsetX)) * 2);
                        if (cA < 0)
                        {
                            cA = 0;
                        }
                    }

                    if (posX + 16 >= 0 && posX < 400 + extraOffsetX && posY + 16 >= 0 && posY < 480)
                    {
                        Drawing.Graphics.drawCircle(new Vector2(posX, posY), 16, new Color((int)(241 + cR), (int)(118 + cG), (int)(50 + cB), (int)cA));
                    }
                }
            }
            gameInstance.spriteBatch.Draw(gameInstance.textureManager.getResource("gg_logo"), new Rectangle(0 + extraOffsetX / 2, 100, 400, 225), Color.White);
            Drawing.Graphics.drawRectangle(new Rectangle(0, 0, 400 + extraOffsetX, 480), new Color(0, 0, 0, (int)(130 * fadeRight)));
            Drawing.Graphics.drawRectangle(new Rectangle(388 + extraOffsetX, 0, 12, 480), new Color(0, 0, 0, (int)(100 * fadeRight)));
        }

        private void drawSteamTrain()
        {
            int extraOffsetX = (int)((100 * fadeLeft) - (100 * fadeRight));

            Drawing.Graphics.drawGradient(new Rectangle(400 + extraOffsetX, 0, 400 - extraOffsetX, 480), new Color(78, 143, 249), new Color(151, 186, 251), false, -1, 1d);

            for (int x = -9; x < 17; x++)
            {
                for (int y = -1; y < 24; y++)
                {
                    int posX = (int)(x * 24 + y * 8 + stoffsetX) + 400;
                    int posY = (int)(y * 24 - (x * 8) + stoffsetY);

                    double colorShift = (double)posY / 480;
                    double cR = 79 * colorShift;
                    double cG = 50 * colorShift;
                    double cB = 6 * colorShift;

                    double cA = 255;

                    if (posX < 400 + extraOffsetX + 90)
                    {
                        cA = 255 - ((400 + extraOffsetX + 90 - posX) * 3);
                        if (cA < 0)
                        {
                            cA = 0;
                        }
                    }

                    if (posX + 16 >= 400 + extraOffsetX && posX < 800  && posY + 16 >= 0 && posY < 480)
                    {
                        Drawing.Graphics.drawCircle(new Vector2(posX, posY), 16, new Color((int)(71 + cR), (int)(133 + cG), (int)(244 + cB), (int)cA));
                    }
                }
            }
            Drawing.Graphics.drawCircle(new Vector2(620 + extraOffsetX / 2, 345), 150, new Color(255, 255, 255, 130));

            Drawing.Graphics.drawCircle(new Vector2(400 + extraOffsetX / 2, 350), 400, Color.White);


            Drawing.Graphics.drawCircle(new Vector2(430 + extraOffsetX / 2, 360), 90, Color.White, 1d);
            Drawing.Graphics.drawCircle(new Vector2(415 + extraOffsetX / 2, 345), 120, new Color(255, 255, 255, 130));
            Drawing.Graphics.drawCircle(new Vector2(445 + extraOffsetX / 2, 375), 60, new Color(204, 218, 247));
            Drawing.Graphics.drawCircle(new Vector2(452 + extraOffsetX / 2, 385), 40, Color.White);

            Drawing.Graphics.drawCircle(new Vector2(630 + extraOffsetX / 2, 410), 20, new Color(204, 218, 247));

            Drawing.Graphics.drawCircle(new Vector2(670 + extraOffsetX / 2, 390), 30, new Color(204, 218, 247));

            Drawing.Graphics.drawCircle(new Vector2(540 + extraOffsetX / 2, 375), 40, new Color(204, 218, 247));

            Drawing.Graphics.drawCircle(new Vector2(580 + extraOffsetX / 2, 405), 60, new Color(204, 218, 247));
            Drawing.Graphics.drawCircle(new Vector2(588 + extraOffsetX / 2, 415), 40, Color.White);

            gameInstance.spriteBatch.Draw(gameInstance.textureManager.getResource("st_logo"), new Rectangle(400 + extraOffsetX / 2, 100, 400, 225), Color.White);
            Drawing.Graphics.drawRectangle(new Rectangle(400 + extraOffsetX, 0, 400 - extraOffsetX, 480), new Color(0, 0, 0, (int)(130 * fadeLeft)));
            Drawing.Graphics.drawRectangle(new Rectangle(400 + extraOffsetX, 0, 12, 480), new Color(0, 0, 0, (int)(100 * fadeLeft)));
        }

        float fadeLeft = 1f;
        float fadeRight = 0f;
        bool selection = true;

        public override void update()
        {
            if (Input.GamePadHandler.buttonPressed(PlayerIndex.One, Buttons.DPadLeft) || Input.KeyboardHandler.keyPressed(Keys.Left))
            {
                selection = true;
            }
            if (Input.GamePadHandler.buttonPressed(PlayerIndex.One, Buttons.DPadRight) || Input.KeyboardHandler.keyPressed(Keys.Right))
            {
                selection = false;
            }

            if (selection)
            {
                ggoffsetX -= 0.9f;
                ggoffsetY += 0.3f;

                if (ggoffsetX <= -24f)
                {
                    ggoffsetX = 0;
                    ggoffsetY = 0;
                }

                if (fadeRight > 0f)
                {
                    fadeRight = MathHelper.Lerp(0f, fadeRight, 0.9f);
                }
                if (fadeLeft < 1f)
                {
                    fadeLeft = MathHelper.Lerp(1f, fadeLeft, 0.9f);
                }
            }
            else
            {
                stoffsetX -= 0.9f;
                stoffsetY += 0.3f;

                if (stoffsetX <= -24f)
                {
                    stoffsetX = 0;
                    stoffsetY = 0;
                }
                if (fadeLeft > 0f)
                {
                    fadeLeft = MathHelper.Lerp(0f, fadeLeft, 0.9f);
                }
                if (fadeRight < 1f)
                {
                    fadeRight = MathHelper.Lerp(1f, fadeRight, 0.9f);
                }
            }
        }
    }
}