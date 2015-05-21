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
        public TestScreen(Game1 game) : base(Identification.Test, game)
        { }

        float offsetX = 0;
        float offsetY = 0;

        public override void draw(GameTime gametime)
        {
            //TESTS FOR THE DRAWING CLASS:
            //Game grumps:
            UI.Graphics.drawGradient(new Rectangle(0, 0, 400, 480), new Color(244, 131, 55), new Color(244, 170, 73), false, -1);

            for (int x = -6; x < 16; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    int posX = (int)(x * 24 + y * 8 + offsetX);
                    int posY = (int)(y * 24 - (x * 8) + offsetY);

                    double colorShift = (double)posY / 480;
                    double cR = 4 * colorShift;
                    double cG = 35 * colorShift;
                    double cB = 20 * colorShift;

                    if (posX + 16 >= 0 && posX < 400 && posY + 16 >= 0 && posY < 480)
                    {
                        UI.Graphics.drawCircle(new Vector2(posX, posY), 16, new Color((int)(241 + cR), (int)(118 + cG), (int)(50 + cB)));
                    }
                }
            }

            //Steam train:
            UI.Graphics.drawGradient(new Rectangle(400, 0, 400, 480), new Color(78, 143, 249), new Color(151, 186, 251), false, -1);

            UI.Graphics.drawCircle(new Vector2(620, 345), 150, new Color(255, 255, 255, 130));

            UI.Graphics.drawCircle(new Vector2(400, 350), 400, Color.White);


            UI.Graphics.drawCircle(new Vector2(430, 360), 90, Color.White);
            UI.Graphics.drawCircle(new Vector2(415, 345), 120, new Color(255, 255, 255, 130));
            UI.Graphics.drawCircle(new Vector2(445, 375), 60, new Color(204, 218, 247));
            UI.Graphics.drawCircle(new Vector2(452, 385), 40, Color.White);

            UI.Graphics.drawCircle(new Vector2(630, 410), 20, new Color(204, 218, 247));

            UI.Graphics.drawCircle(new Vector2(670, 390), 30, new Color(204, 218, 247));

            UI.Graphics.drawCircle(new Vector2(540, 375), 40, new Color(204, 218, 247));

            UI.Graphics.drawCircle(new Vector2(580, 405), 60, new Color(204, 218, 247));
            UI.Graphics.drawCircle(new Vector2(588, 415), 40, Color.White);

            gameInstance.spriteBatch.Draw(gameInstance.Content.Load<Texture2D>("gg_logo"), new Rectangle(0, 100, 400, 225), Color.White);
            gameInstance.spriteBatch.Draw(gameInstance.Content.Load<Texture2D>("st_logo"), new Rectangle(400, 100, 400, 225), Color.White);

            UI.Graphics.drawRectangle(new Rectangle(400, 0, 400, 480), new Color(0, 0, 0, (int)(130 * fadeRight)));
            UI.Graphics.drawRectangle(new Rectangle(400, 0, 12, 480), new Color(0, 0, 0, (int)(100 * fadeRight)));
            UI.Graphics.drawRectangle(new Rectangle(0, 0, 400, 480), new Color(0, 0, 0, (int)(130 * fadeLeft)));
            UI.Graphics.drawRectangle(new Rectangle(388, 0, 12, 480), new Color(0, 0, 0, (int)(100 * fadeLeft)));
        }

        float fadeRight = 1f;
        float fadeLeft = 0f;
        bool selection = true;

        public override void update(GameTime gametime)
        {
            GamePadState gState = GamePad.GetState(0);

            if (gState.IsButtonDown(Buttons.DPadLeft))
            {
                selection = true;
            }
            if (gState.IsButtonDown(Buttons.DPadRight))
            {
                selection = false;
            }

            if (selection)
            {
                offsetX -= 0.9f;
                offsetY += 0.3f;

                if (offsetX <= -24f)
                {
                    offsetX = 0;
                    offsetY = 0;
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
            else
            {
                if (fadeRight > 0f)
                {
                    fadeRight = MathHelper.Lerp(0f, fadeRight, 0.9f);
                }
                if (fadeLeft < 1f)
                {
                    fadeLeft = MathHelper.Lerp(1f, fadeLeft, 0.9f);
                }
            }
        }
    }
}