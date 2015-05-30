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
    /// The main menu of the game.
    /// </summary>
    class MainMenuScreen : Screen
    {
        //The main menu will just feature the logo of the game and a prompt that says "press any button to start".

        private Texture2D _logoTexture = null; //texture of the logo that appears on the screen.

        //The offset of the dots:
        float _offsetX = 0;
        float _offsetY = 0;

        float _logoAnimation = 20f;
        float _gameTitleAnimation = 1f;

        //The size reference of the dots in the background
        const int DOT_SIZE = 16;
        const string _gameTitle = "HARD DUDES";

        private SpriteFont _grumpFont = null;

        public MainMenuScreen(GGGame game) : base(Identification.MainMenu, game)
        {
            _logoTexture = game.textureManager.getResource("gg_logo");
            _grumpFont = game.Content.Load<SpriteFont>("CartoonFontLarge");

            //MediaPlayer.Play(game.musicManager.getResource(@"Music\Smash 1"));
            //MediaPlayer.IsRepeating = true;
        }

        public override void draw()
        {
            drawBackground();
            drawTitle();
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

        private void drawTitle()
        {
            int width = (int)(_logoTexture.Width / _logoAnimation);
            int height = (int)(_logoTexture.Height / _logoAnimation);

            Rectangle destinationRectangle = new Rectangle((int)(gameInstance.clientRectangle.Width / 2f - width / 2f), (int)(200 - height / 2f), width, height);
            destinationRectangle.X += destinationRectangle.Width / 2;
            destinationRectangle.Y += destinationRectangle.Height / 2;

            gameInstance.spriteBatch.Draw(_logoTexture, destinationRectangle, 
                null, Color.White, _logoAnimation - 1f, new Vector2(width / 2f, height / 2f), SpriteEffects.None, 0f);

            gameInstance.fontBatch.DrawString(_grumpFont, _gameTitle, 
                new Vector2((gameInstance.clientRectangle.Width * _gameTitleAnimation) + gameInstance.clientRectangle.Width / 2 - _grumpFont.MeasureString(_gameTitle).X / 2 + 5, 
                90 + _logoTexture.Height + 5), new Color(122, 141, 235), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            gameInstance.fontBatch.DrawString(_grumpFont, _gameTitle, 
                new Vector2(-(gameInstance.clientRectangle.Width * _gameTitleAnimation) + gameInstance.clientRectangle.Width / 2 - _grumpFont.MeasureString(_gameTitle).X / 2, 
                90 + _logoTexture.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public override void update()
        {
            //Update the dot animation:
            _offsetX -= 0.9f;
            _offsetY += 0.3f;

            if (_logoAnimation > 1f)
            {
                _logoAnimation -= 0.2f;
                if (_logoAnimation <= 1f)
                {
                    _logoAnimation = 1f;
                }
            }
            else
            {
                if (_gameTitleAnimation > 0f)
                {
                    _gameTitleAnimation = MathHelper.Lerp(0f, _gameTitleAnimation, 0.92f);
                    if (_gameTitleAnimation <= 0f)
                    {
                        _gameTitleAnimation = 0f;
                    }
                }
            }

            //Reset it, once it went through a complete cycle:
            if (_offsetX <= (float)-DOT_SIZE * 3)
            {
                _offsetX = 0;
                _offsetY = 0;
            }

            //When a button is pressed, open the next screen:
            if (Input.GamePadHandler.buttonPressed(PlayerIndex.One, Buttons.A))
            {
                ScreenManager.getInstance().setScreen(new Game.GrumpSpaceScreen(gameInstance));
            }
        }
    }
}