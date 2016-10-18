using GGFanGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static GameProvider;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The title screen for the game.
    /// </summary>
    internal class TitleScreen : Screen
    {
        //The title screen will just feature the logo of the game and a prompt that says "press any button to start".

        private readonly Texture2D _logoTexture; //texture of the logo that appears on the screen.

        private float _logoAnimation = 20f;
        private float _gameTitleAnimation = 1f;

        private readonly SpriteFont _grumpFont;

        private readonly MenuBackgroundRenderer _backgroundRenderer;

        public TitleScreen()
        {
            _backgroundRenderer = new MenuBackgroundRenderer();
            _logoTexture = GameInstance.Content.Load<Texture2D>(@"UI\Logos\GameGrumps");
            _grumpFont = GameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFontLarge");

            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(game.musicManager.load(@"Music\Smash 2"));
        }

        public override void Draw()
        {
            _backgroundRenderer.Draw(GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);

            DrawTitle();
        }

        private void DrawTitle()
        {
            var width = (int)(_logoTexture.Width / _logoAnimation);
            var height = (int)(_logoTexture.Height / _logoAnimation);

            GameInstance.SpriteBatch.Draw(_logoTexture, new Rectangle((int)(GameInstance.ClientRectangle.Width / 2f), 200, width, height),
                                          null, Color.White,
                                          _logoAnimation - 1f, new Vector2(_logoTexture.Width / 2f, _logoTexture.Height / 2f), SpriteEffects.None, 0f);

            GameInstance.FontBatch.DrawString(_grumpFont, GameController.GAME_TITLE,
                new Vector2((GameInstance.ClientRectangle.Width * _gameTitleAnimation) + GameInstance.ClientRectangle.Width / 2 - _grumpFont.MeasureString(GameController.GAME_TITLE).X / 2 + 5,
                90 + _logoTexture.Height + 5), new Color(122, 141, 235), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            GameInstance.FontBatch.DrawString(_grumpFont, GameController.GAME_TITLE,
                new Vector2(-(GameInstance.ClientRectangle.Width * _gameTitleAnimation) + GameInstance.ClientRectangle.Width / 2 - _grumpFont.MeasureString(GameController.GAME_TITLE).X / 2,
                90 + _logoTexture.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public override void Update()
        {
            _backgroundRenderer.Update();

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
            if (GamePadHandler.ButtonPressed(PlayerIndex.One, Buttons.A))
            {
                ScreenManager.GetInstance().SetScreen(new LoadSaveScreen(_backgroundRenderer));
            }
        }
    }
}