using GGFanGame.Content;
using GGFanGame.Drawing;
using GGFanGame.Input;
using GGFanGame.Screens.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Threading.Tasks;
using static Core;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The title screen for the game.
    /// </summary>
    internal class TitleScreen : Screen
    {
        private const int ARIN_HEAD_SIZE = 100;
        private const int DANNY_HEAD_SIZE = 100;

        // The title screen will just feature the logo of the game and a prompt that says "press any button to start".

        private readonly Texture2D _logoTexture,
                                   _arinHead, _dannyHead;

        private int _stage = 0;
        private DateTime _mainSoundStarted;

        private float _arinHeadIntro = 0f;
        private float _dannyHeadIntro = 0f;
        private float _intermediateIntro = 0f;
        private float _mainIntro = 0f;
        private int _hardDudesDelay = 20;
        private float _dannyLaughAnimation = 0f;
        private float _dannyLaughAnimationDelay = 1f;
        private float _dannyLaughAnimationBack = 1f;

        private float _contentFloat = 0f;

        private readonly SpriteFont _grumpFont;
        private SoundEffectInstance _mainIntroSound, _hardDudesSound;
        private SpriteBatch _batch, _fontBatch;
        private MenuBackgroundRenderer _backgroundRenderer;

        public TitleScreen()
        {
            _backgroundRenderer = new MenuBackgroundRenderer();
            _logoTexture = GameInstance.Content.Load<Texture2D>(Resources.UI.Logos.GameGrumps);
            _grumpFont = GameInstance.Content.Load<SpriteFont>(Resources.Fonts.CartoonFontLarge);
            _arinHead = GameInstance.Content.Load<Texture2D>(Resources.UI.Heads.Arin);
            _dannyHead = GameInstance.Content.Load<Texture2D>(Resources.UI.Heads.Danny);
            _mainIntroSound = Content.Load<SoundEffect>(Resources.Sounds.Intro.Main).CreateInstance();
            _hardDudesSound = Content.Load<SoundEffect>(Resources.Sounds.Intro.HardDudes).CreateInstance();

            _mainIntroSound.IsLooped = false;
            _hardDudesSound.IsLooped = false;

            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
            _fontBatch = new SpriteBatch(GameInstance.GraphicsDevice);
        }

        public override void Draw(GameTime time)
        {
            _batch.Begin(SpriteBatchUsage.Default);
            _fontBatch.Begin(SpriteBatchUsage.Font);

            _backgroundRenderer.Draw(_batch, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);

            switch (_stage)
            {
                case 0:
                    DrawArinHead();
                    break;
                case 1:
                    DrawDannyHead();
                    break;
                case 2:
                    DrawIntermediate();
                    break;
                case 3:
                    DrawTitle();
                    break;
            }

            _batch.End();
            _fontBatch.End();
        }

        private void DrawTitle()
        {
            var logoWidth = (int)(_logoTexture.Width);
            var logoHeight = (int)(_logoTexture.Height);

            float floatingOffset = (float)Math.Sin(_contentFloat);

            _batch.Draw(_logoTexture,
                new Rectangle((int)(GameInstance.ClientRectangle.Width / 2f),
                (int)(300 + floatingOffset * 10f),
                (int)(logoWidth * _mainIntro), (int)(logoHeight * _mainIntro)),
                null, Color.White, 10 * (1f - _mainIntro),
                new Vector2(_logoTexture.Width / 2f, _logoTexture.Height / 2f), SpriteEffects.None, 0f);

            _batch.Draw(_arinHead, new Rectangle(
                (int)(GameInstance.ClientRectangle.Width / 2f
                    + logoWidth / 2f
                    + (GameInstance.ClientRectangle.Width / 2f) * (1f - _mainIntro)),
                (int)(240 + floatingOffset * -5f),
                240, 240), new Rectangle(0, 0, ARIN_HEAD_SIZE, ARIN_HEAD_SIZE),
                Color.White, 60 * ((1f - _mainIntro) / 10f), new Vector2(ARIN_HEAD_SIZE / 2f, ARIN_HEAD_SIZE / 2f), SpriteEffects.None, 0f);

            int dannyHeadIndex = (int)(9 * _dannyLaughAnimation);
            if (_dannyLaughAnimationBack == 0f)
                dannyHeadIndex = 0;

            _batch.Draw(_dannyHead, new Rectangle(
                (int)(GameInstance.ClientRectangle.Width / 2f
                    - logoWidth / 2f
                    - DANNY_HEAD_SIZE / 2f
                    - (GameInstance.ClientRectangle.Width / 2f) * (1f - _mainIntro)),
                (int)(240 + floatingOffset * -5f),
                240, 240), new Rectangle(dannyHeadIndex * DANNY_HEAD_SIZE, 0, DANNY_HEAD_SIZE, DANNY_HEAD_SIZE),
                Color.White, -60 * ((1f - _mainIntro) / 10f), new Vector2(DANNY_HEAD_SIZE / 2f, DANNY_HEAD_SIZE / 2f), SpriteEffects.None, 0f);

            var subtitleSize = _grumpFont.MeasureString(GameController.GAME_TITLE);
            var subtitleState = (20 - _hardDudesDelay) / 20f;

            TextRenderHelper.RenderGrumpText(_fontBatch, _grumpFont, GameController.GAME_TITLE,
                new Vector2(
                GameInstance.ClientRectangle.Width / 2f - subtitleSize.X / 2f,
                GameInstance.ClientRectangle.Height + 10 - 200 * subtitleState), 1f, (int)(255 * subtitleState));
        }

        private void DrawArinHead()
        {
            int headSize = (int)(GameInstance.Window.ClientBounds.Height * 0.9f);

            var headX = 0f;
            var titleX = 0f;
            int headTexture = 0;

            if (_arinHeadIntro <= 0.2f)
            {
                var state = _arinHeadIntro / 0.2f;

                headX = GameInstance.Window.ClientBounds.Width -
                    (GameInstance.Window.ClientBounds.Width / 2f) * state
                    - headSize / 2;
                titleX = (GameInstance.Window.ClientBounds.Width / 2f) * state
                    - 400;
            }
            else if (_arinHeadIntro <= 0.8f)
            {
                var state = _arinHeadIntro / 0.8f;

                headX = GameInstance.Window.ClientBounds.Width / 2f
                    - state * 20f
                    - headSize / 2;
                titleX = GameInstance.Window.ClientBounds.Width / 2f
                    + 10f * state
                    - 400;

                headTexture = (int)(state * 10);
            }
            else
            {
                var state = (_arinHeadIntro - 0.8f) / 0.2f;

                headX = GameInstance.Window.ClientBounds.Width / 2f
                    - 20
                    - state * (GameInstance.Window.ClientBounds.Width)
                    - headSize / 2;
                titleX = GameInstance.Window.ClientBounds.Width / 2f
                    + 10f
                    + state * GameInstance.Window.ClientBounds.Width
                    - 400;
            }
            
            _batch.Draw(_arinHead,
                new Rectangle((int)headX, GameInstance.ClientRectangle.Height / 2 - headSize / 2, headSize, headSize),
                new Rectangle(headTexture * ARIN_HEAD_SIZE, 0, ARIN_HEAD_SIZE, ARIN_HEAD_SIZE), Color.White);

            TextRenderHelper.RenderGrumpText(_fontBatch, _grumpFont, "EGORAPTOR",
                new Vector2(titleX, GameInstance.ClientRectangle.Height - 180), 1.5f);
        }

        private void DrawDannyHead()
        {
            int headSize = (int)(GameInstance.Window.ClientBounds.Height * 0.9f);

            var headX = 0f;
            var titleX = 0f;

            if (_dannyHeadIntro <= 0.2f)
            {
                var state = _dannyHeadIntro / 0.2f;

                headX = state * ((GameInstance.Window.ClientBounds.Width) / 2f + headSize)
                    - headSize * 1.5f;
                titleX = (GameInstance.Window.ClientBounds.Width)
                    - 300
                    - GameInstance.Window.ClientBounds.Width / 2f * state;
            }
            else if (_dannyHeadIntro <= 0.8f)
            {
                var state = _dannyHeadIntro / 0.8f;

                headX = ((GameInstance.Window.ClientBounds.Width + headSize) / 2f)
                    + state * 20f
                    - headSize;
                titleX = GameInstance.Window.ClientBounds.Width / 2f
                    - 300
                    + 10f * state;
            }
            else
            {
                var state = (_dannyHeadIntro - 0.8f) / 0.2f;

                headX = ((GameInstance.Window.ClientBounds.Width + headSize) / 2f)
                    + 20f
                    + GameInstance.Window.ClientBounds.Width * state
                    - headSize;
                titleX = GameInstance.Window.ClientBounds.Width / 2f
                    - 300
                    + 10f
                    - GameInstance.Window.ClientBounds.Width * state;
            }

            _batch.Draw(_dannyHead,
                new Rectangle((int)headX, GameInstance.ClientRectangle.Height / 2 - headSize / 2, headSize, headSize),
                new Rectangle(0, 0, DANNY_HEAD_SIZE, DANNY_HEAD_SIZE), Color.White);

            TextRenderHelper.RenderGrumpText(_fontBatch, _grumpFont, "DANNY",
                new Vector2(titleX, GameInstance.ClientRectangle.Height - 180), 1.5f);
        }

        private void DrawIntermediate()
        {
            if (_intermediateIntro >= 0f)
            {
                var state = _intermediateIntro / 0.3f;
                if (state > 1f)
                    state = 1f;

                var text = "AND";
                var size = _grumpFont.MeasureString(text) * 1.5f;

                TextRenderHelper.RenderGrumpText(_fontBatch, _grumpFont, text,
                    new Vector2(
                        GameInstance.ClientRectangle.Width * 2f - (GameInstance.ClientRectangle.Width * 1.5f + size.X / 2f) * state,
                        GameInstance.ClientRectangle.Height / 2f - size.Y * 1.5f), 1.5f);
            }
            if (_intermediateIntro >= 0.3f)
            {
                var state = (_intermediateIntro - 0.3f) / 0.3f;
                if (state > 1f)
                    state = 1f;

                var text = "WE'RE";
                var size = _grumpFont.MeasureString(text) * 1.5f;

                TextRenderHelper.RenderGrumpText(_fontBatch, _grumpFont, text,
                    new Vector2(
                        -GameInstance.ClientRectangle.Width + (GameInstance.ClientRectangle.Width * 1.5f - size.X / 2f) * state,
                        GameInstance.ClientRectangle.Height / 2f - size.Y * 0.5f), 1.5f);
            }
            if (_intermediateIntro >= 0.6f)
            {
                var state = (_intermediateIntro - 0.6f) / 0.3f;
                if (state > 1f)
                    state = 1f;

                var text = "THE";
                var size = _grumpFont.MeasureString(text) * 1.5f;

                TextRenderHelper.RenderGrumpText(_fontBatch, _grumpFont, text,
                    new Vector2(
                        GameInstance.ClientRectangle.Width * 2f - (GameInstance.ClientRectangle.Width * 1.5f + size.X / 2f) * state,
                        GameInstance.ClientRectangle.Height / 2f + size.Y * 0.5f), 1.5f);
            }
        }

        public override void Update(GameTime time)
        {
            _backgroundRenderer.Update();

            switch (_stage)
            {
                case 0:
                    UpdateArinHead();
                    break;
                case 1:
                    UpdateDannyHead();
                    break;
                case 2:
                    UpdateIntermediate();
                    break;
                case 3:
                    UpdateTitle();
                    break;
            }
        }

        private void UpdateArinHead()
        {
            if (_mainIntroSound.State == SoundState.Stopped)
            {
                _mainIntroSound.Play();
                _mainSoundStarted = DateTime.UtcNow;
            }

            var diff = (DateTime.UtcNow - _mainSoundStarted).TotalMilliseconds;
            _arinHeadIntro = (float)(diff / 1100);

            if (diff >= 1100)
            {
                _stage++;
            }
        }

        private void UpdateDannyHead()
        {
            var diff = (DateTime.UtcNow - _mainSoundStarted).TotalMilliseconds;
            _dannyHeadIntro = (float)((diff - 1100) / 1500);

            if (diff >= 2600)
            {
                _stage++;
            }
        }

        private void UpdateIntermediate()
        {
            var diff = (DateTime.UtcNow - _mainSoundStarted).TotalMilliseconds;
            _intermediateIntro = (float)((diff - 2600) / 1500);

            if (diff >= 4100)
            {
                _stage++;
            }
        }

        private void UpdateTitle()
        {
            if (_mainIntro < 1f)
            {
                _mainIntro += 0.04f;
                if (_mainIntro >= 1f)
                {
                    _mainIntro = 1f;
                }
            }

            if (_mainIntro == 1f)
            {
                _contentFloat += 0.07f;
                
                if (_hardDudesDelay > 0)
                {
                    _hardDudesDelay--;
                    if (_hardDudesDelay == 0)
                    {
                        _hardDudesSound.Play();
                        MediaPlayer.Play(Content.Load<Song>(Resources.Music.GGVersus));
                        Task.Run(() => MusicPlayerHelper.FadeIn(50));
                    }
                }
                else
                {
                    if (_dannyLaughAnimationDelay > 0f)
                    {
                        _dannyLaughAnimationDelay -= 0.02f;
                        if (_dannyLaughAnimationDelay <= 0f)
                        {
                            _dannyLaughAnimationDelay = 0f;
                        }
                    }
                    else if (_dannyLaughAnimation < 1f)
                    {
                        _dannyLaughAnimation += 0.01f;
                        if (_dannyLaughAnimation >= 1f)
                        {
                            _dannyLaughAnimation = 1f;
                        }
                    }
                    else if (_dannyLaughAnimationBack > 0f)
                    {
                        _dannyLaughAnimationBack -= 0.01f;
                        if (_dannyLaughAnimationBack <= 0f)
                        {
                            _dannyLaughAnimationBack = 0f;
                        }
                    }

                    // When a button is pressed, open the next screen:
                    if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.A))
                    {
                        StartGame();
                    }
                }
            }
        }

        private void StartGame()
        {
            GetComponent<GameSessionManager>().Load();
            GetComponent<ScreenManager>().SetScreen(new TransitionScreen(this, new StageScreen()));
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
