using System;
using GGFanGame.Content;
using GGFanGame.Drawing;
using GGFanGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The title screen for the game.
    /// </summary>
    internal class TitleScreen : Screen
    {
        //The title screen will just feature the logo of the game and a prompt that says "press any button to start".

        private readonly Texture2D _logoTexture,
                                   _arinHead, _dannyHead;

        private float _logoAnimation = 20f;
        private float _flashAnimation = 0f;
        private float _gameTitleAnimation = 1f;
        private float _contentFloat = 0f;

        private readonly SpriteFont _grumpFont;
        private SpriteBatch _batch, _fontBatch;
        private MenuBackgroundRenderer _backgroundRenderer;

        public TitleScreen()
        {
            _backgroundRenderer = new MenuBackgroundRenderer();
            _logoTexture = GameInstance.Content.Load<Texture2D>(Resources.UI.Logos.GameGrumps);
            _grumpFont = GameInstance.Content.Load<SpriteFont>(Resources.Fonts.CartoonFontLarge);
            _arinHead = GameInstance.Content.Load<Texture2D>(Resources.UI.Heads.Arin);
            _dannyHead = GameInstance.Content.Load<Texture2D>(Resources.UI.Heads.Danny);

            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
            _fontBatch = new SpriteBatch(GameInstance.GraphicsDevice);

            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(game.musicManager.load(@"Music\Smash 2"));
        }

        public override void Draw()
        {
            _batch.Begin(SpriteBatchUsage.Default);
            _fontBatch.Begin(SpriteBatchUsage.Font);

            _backgroundRenderer.Draw(_batch, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);

            DrawTitle();

            _batch.End();
            _fontBatch.End();
        }

        private void DrawTitle()
        {
            var logoWidth = (int)(_logoTexture.Width / _logoAnimation);
            var logoHeight = (int)(_logoTexture.Height / _logoAnimation);

            float offset = (float)Math.Sin(_contentFloat);

            if (_logoAnimation == 1f)
            {
                _batch.Draw(_arinHead, new Rectangle((int)(GameInstance.ClientRectangle.Width / 2f + logoWidth / 2f - 100), 
                    (int)(150 + offset * -5f), 240, 240), Color.White);

                _batch.Draw(_dannyHead, new Rectangle((int)(GameInstance.ClientRectangle.Width / 2f - logoWidth / 2f - 150), 
                    (int)(150 + offset * -5f), 240, 240), Color.White);
            }

            _batch.Draw(_logoTexture, new Rectangle((int)(GameInstance.ClientRectangle.Width / 2f), (int)(300 + offset * 10f), logoWidth, logoHeight),
                                          null, Color.White,
                                          _logoAnimation - 1f, new Vector2(_logoTexture.Width / 2f, _logoTexture.Height / 2f), SpriteEffects.None, 0f);

            if (_flashAnimation > 0f)
            {
                _batch.DrawRectangle(GameInstance.ClientRectangle, new Color(255, 255, 255, (int)(_flashAnimation * 255)));
            }

            _fontBatch.DrawString(_grumpFont, GameController.GAME_TITLE,
                new Vector2((GameInstance.ClientRectangle.Width * _gameTitleAnimation) + GameInstance.ClientRectangle.Width / 2 - _grumpFont.MeasureString(GameController.GAME_TITLE).X / 2 + 5,
                190 + _logoTexture.Height + 5), new Color(122, 141, 235), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _fontBatch.DrawString(_grumpFont, GameController.GAME_TITLE,
                new Vector2(-(GameInstance.ClientRectangle.Width * _gameTitleAnimation) + GameInstance.ClientRectangle.Width / 2 - _grumpFont.MeasureString(GameController.GAME_TITLE).X / 2,
                190 + _logoTexture.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
                    _flashAnimation = 1f;
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

            if (_flashAnimation > 0f)
            {
                _flashAnimation -= 0.01f;
                if (_flashAnimation <= 0f)
                {
                    _flashAnimation = 0f;
                }
            }

            _contentFloat += 0.07f;

            //When a button is pressed, open the next screen:
            if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.A))
            {
                GetComponent<ScreenManager>().SetScreen(new LoadSaveScreen(_backgroundRenderer.Clone()));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_batch != null && !_batch.IsDisposed) _batch.Dispose();
                    if (_fontBatch != null && !_fontBatch.IsDisposed) _fontBatch.Dispose();
                    if (_backgroundRenderer != null && !_backgroundRenderer.IsDisposed) _backgroundRenderer.Dispose();
                }

                _batch = null;
                _fontBatch = null;
                _backgroundRenderer = null;
            }

            base.Dispose(disposing);
        }
    }
}
