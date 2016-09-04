using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static GGFanGame.GameProvider;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The title screen for the game.
    /// </summary>
    class TitleScreen : MainMenuScreen
    {
        //The title screen will just feature the logo of the game and a prompt that says "press any button to start".

        private Texture2D _logoTexture = null; //texture of the logo that appears on the screen.

        float _logoAnimation = 20f;
        float _gameTitleAnimation = 1f;

        private SpriteFont _grumpFont = null;

        public TitleScreen() : base()
        {
            _logoTexture = gameInstance.textureManager.load(@"UI\Logos\GameGrumps");
            _grumpFont = gameInstance.fontManager.load(@"CartoonFontLarge");

            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(game.musicManager.load(@"Music\Smash 2"));
        }

        public override void draw()
        {
            base.draw();

            drawTitle();
        }

        private void drawTitle()
        {
            int width = (int)(_logoTexture.Width / _logoAnimation);
            int height = (int)(_logoTexture.Height / _logoAnimation);

            gameInstance.spriteBatch.Draw(_logoTexture, new Rectangle((int)(gameInstance.clientRectangle.Width / 2f), 200, width, height),
                                          null, Color.White,
                                          _logoAnimation - 1f, new Vector2(_logoTexture.Width / 2f, _logoTexture.Height / 2f), SpriteEffects.None, 0f);

            gameInstance.fontBatch.DrawString(_grumpFont, GGGame.GAME_TITLE,
                new Vector2((gameInstance.clientRectangle.Width * _gameTitleAnimation) + gameInstance.clientRectangle.Width / 2 - _grumpFont.MeasureString(GGGame.GAME_TITLE).X / 2 + 5,
                90 + _logoTexture.Height + 5), new Color(122, 141, 235), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            gameInstance.fontBatch.DrawString(_grumpFont, GGGame.GAME_TITLE,
                new Vector2(-(gameInstance.clientRectangle.Width * _gameTitleAnimation) + gameInstance.clientRectangle.Width / 2 - _grumpFont.MeasureString(GGGame.GAME_TITLE).X / 2,
                90 + _logoTexture.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public override void update()
        {
            base.update();

            //Update title animation:
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

            //When a button is pressed, open the next screen:
            if (Input.GamePadHandler.buttonPressed(PlayerIndex.One, Buttons.A))
            {
                ScreenManager.getInstance().setScreen(new LoadSaveScreen(new Vector2(_offsetX, _offsetY)));
            }
        }
    }
}