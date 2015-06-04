using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// An abstract screen class that renders the background of the main menu.
    /// </summary>
    abstract class MainMenuScreen : Screen
    {
        //The offset of the dots:
        protected float _offsetX = 0;
        protected float _offsetY = 0;

        //The size reference of the dots in the background
        const int DOT_SIZE = 16;

        public MainMenuScreen(Identification identification, GGGame game) : this(identification, game, Vector2.Zero)
        {  /* Empty constructor */ }

        public MainMenuScreen(Identification identification, GGGame game, Vector2 initialDotOffset) : base(identification ,game)
        {
            _offsetX = initialDotOffset.X;
            _offsetY = initialDotOffset.Y;
        }

        public override void draw()
        {
            drawBackground();
        }

        private void drawBackground()
        {
            //Draws the black-orange background gradient:
            Drawing.Graphics.drawGradient(gameInstance.clientRectangle, Color.Black, new Color(164, 108, 46), false);

            //Draw the background dots:
            for (int x = -6; x < 32; x++)
            {
                for (int y = 0; y < 21; y++)
                {
                    int posX = (int)(x * DOT_SIZE * 3 + y * DOT_SIZE + _offsetX);
                    int posY = (int)(y * DOT_SIZE * 3 - (x * DOT_SIZE) + _offsetY);

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
                        Drawing.Graphics.drawCircle(new Vector2(posX, posY), DOT_SIZE * 2, new Color((int)(241 + cR), (int)(118 + cG), (int)(50 + cB), (int)cA));
                    }
                }
            }
        }

        public override void update()
        {
            //Update the dot animation:
            _offsetX -= 0.9f;
            _offsetY += 0.3f;

            //Reset it, once it went through a complete cycle:
            if (_offsetX <= (float)-DOT_SIZE * 3)
            {
                _offsetX = 0;
                _offsetY = 0;
            }
        }
    }
}