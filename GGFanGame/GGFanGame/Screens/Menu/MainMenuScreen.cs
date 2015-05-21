using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The main menu of the game.
    /// </summary>
    class MainMenuScreen : Screen
    {
        public MainMenuScreen() : base(Identification.MainMenu)
        {

        }

        public override void draw(GameTime gametime)
        {
            //TESTS FOR THE DRAWING CLASS:
            UI.Graphics.drawGradient(new Rectangle(0, 0, 400, 480), new Color(244, 131, 55), new Color(244, 170, 73), false, -1);

            for (int x = -6; x < 15; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    int posX = x * 24 + y * 8;
                    int posY = y * 24 - (x * 8);

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
            UI.Graphics.drawGradient(new Rectangle(400, 0, 400, 480), new Color(78, 143, 249), new Color(151, 186, 251), false, -1);
        }

        public override void update(GameTime gametime)
        {

        }
    }
}