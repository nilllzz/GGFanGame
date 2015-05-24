using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The main menu of the game.
    /// </summary>
    class MainMenuScreen : Screen
    {
        //The main menu will just feature the logo of the game and a prompt that says "press any button to start".

        private Texture2D _logoTexture = null; //texture of the logo that appears on the screen.

        //The offset of the dots:
        float ggoffsetX = 0;
        float ggoffsetY = 0;

        //The size reference of the dots in the background
        const int DOT_SIZE = 16;

        public MainMenuScreen(GGGame game) : base(Identification.MainMenu, game)
        {
            _logoTexture = game.Content.Load<Texture2D>("gg_logo");
        }

        public override void draw(GameTime gametime)
        {
            //Draws the black-orange background gradient:
            UI.Graphics.drawGradient(gameInstance.clientRectangle, Color.Black, new Color(164, 108, 46), false);

            //Draw the background dots:
            for (int x = -6; x < 32; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    int posX = (int)(x * DOT_SIZE * 3 + y * DOT_SIZE + ggoffsetX);
                    int posY = (int)(y * DOT_SIZE * 3 - (x * DOT_SIZE) + ggoffsetY);

                    //We shift their color from top to bottom, so we take the different between the height of the screen and the dot's position:
                    double colorShift = (double)posY / gameInstance.clientRectangle.Height;
                    double cR = 4 * colorShift;
                    double cG = 35 * colorShift;
                    double cB = 20 * colorShift;

                    //When they approach the sides of the screen, make them fade out:
                    double cA = 255; //alpha value
                    if (posX > gameInstance.clientRectangle.Width - 90)
                    {
                        cA = 255 - ((posX - (gameInstance.clientRectangle.Width - 90)) * 3);
                        if (cA < 0)
                        {
                            cA = 0;
                        }
                    }
                    else if (posX < 90)
                    {
                        cA = 255 - ((90 - posX) * 3);
                        if (cA < 0)
                        {
                            cA = 0;
                        }
                    }

                    //Also, make them fade out on the top of the screen, cause the orange-black contrast would be a bit jarring.
                    cA *= colorShift;

                    //When the dot is inside the rendering area, draw it.
                    if (posX + DOT_SIZE * 2 >= 0 && posX < gameInstance.clientRectangle.Width && posY + DOT_SIZE * 2 >= 0 && posY < gameInstance.clientRectangle.Height)
                    {
                        UI.Graphics.drawCircle(new Vector2(posX, posY), DOT_SIZE * 2, new Color((int)(241 + cR), (int)(118 + cG), (int)(50 + cB), (int)cA));
                    }
                }
            }

        }

        public override void update(GameTime gametime)
        {
            //Update the dot animation:
            ggoffsetX -= 0.9f;
            ggoffsetY += 0.3f;

            //Reset it, once it went through a complete cycle:
            if (ggoffsetX <= (float)-DOT_SIZE * 3)
            {
                ggoffsetX = 0;
                ggoffsetY = 0;
            }

            //When a button is pressed, open the next screen:
            if (Input.GamePadHandler.buttonPressed(PlayerIndex.One, Buttons.A))
            {
                ScreenManager.getInstance().setScreen(new PlayerSelectScreen(gameInstance));
            }
        }
    }
}