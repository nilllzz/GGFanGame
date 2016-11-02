using GGFanGame.Content;
using GGFanGame.Game;
using GGFanGame.Input;
using GGFanGame.Screens.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static GameProvider;

namespace GGFanGame.Screens.Menu
{
    internal class PauseScreen : Screen
    {
        private const float SCREEN_SIZE_MULTIPLIER = 0.5f;

        private readonly StageScreen _preScreen;

        private readonly Texture2D _barry, _bubble;
        private readonly SpriteFont _font;
        private RenderTarget2D _target;
        private MenuBackgroundRenderer _backgroundRenderer;

        private float _preScreenSize;
        private bool _closing;

        public PauseScreen(StageScreen preScreen)
        {
            _preScreen = preScreen;
            _backgroundRenderer = new MenuBackgroundRenderer(Color.Black, new Color(164, 108, 46),
                new Color(236, 130, 47), new Color(242, 153, 90)) {ApplyTransparency = true};

            _barry = Content.Load<Texture2D>(Resources.UI.Pause.barry_pause);
            _bubble = Content.Load<Texture2D>(Resources.UI.Pause.paused_bubble);
            _font = Content.Load<SpriteFont>(Resources.Fonts.CartoonFont);

            _target = new RenderTarget2D(GameInstance.GraphicsDevice, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);
        }

        public override void Draw()
        {
            if (IsDisposed) return;
            
            // acquire background and draw it
            _backgroundRenderer.Draw(GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);

            // create texture of pre screen
            RenderTargetManager.BeginRenderScreenToTarget(_target);
            _preScreen.DrawStage();
            RenderTargetManager.EndRenderScreenToTarget();

            // draw pre screen:
            var preScreenWidth = _target.Width * (1f - _preScreenSize * SCREEN_SIZE_MULTIPLIER);
            var preScreenHeight = _target.Height * (1f - _preScreenSize * SCREEN_SIZE_MULTIPLIER);

            GameInstance.SpriteBatch.Draw(_target, new Rectangle((int)(GameController.RENDER_WIDTH * 0.5f - preScreenWidth * 0.5f),
                                                                 (int)(GameController.RENDER_HEIGHT * 0.5f - preScreenHeight * 0.5f),
                                                                 (int)preScreenWidth,
                                                                 (int)preScreenHeight), Color.White);
            // draw the pre screen's HUD in full size
            _preScreen.DrawHUD();

            // draw barry
            var barryWidth = _barry.Width / 2;
            var barryHeight = _barry.Height / 2;
            GameInstance.SpriteBatch.Draw(texture: _barry,
                                        destinationRectangle: new Rectangle((int)(-barryWidth + barryWidth * _preScreenSize), 200, barryWidth, barryHeight),
                                        color: Color.White,
                                        effects: SpriteEffects.FlipHorizontally);

            // draw speech bubble
            GameInstance.SpriteBatch.Draw(_bubble, new Vector2(barryWidth, 200), new Color(255, 255, 255, (int)(255 * _preScreenSize)));
            GameInstance.SpriteBatch.DrawString(_font, "PAUSED", new Vector2(barryWidth + 30, 244), new Color(0, 0, 0, (int)(255 * _preScreenSize)), 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);

            // draw level info
            var stage = Stage.ActiveStage;
            GameInstance.FontBatch.DrawString(_font, $"STAGE: {stage.WorldId}-{stage.StageId} ({stage.Name})\nSTORY MODE", new Vector2(200, GameController.RENDER_HEIGHT - 135 * _preScreenSize), Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
        }

        public override void Update()
        {
            _backgroundRenderer.Update();

            if (GamePadHandler.ButtonPressed(PlayerIndex.One, Buttons.B) && !_closing)
            {
                _closing = true;
            }
            else if (_closing)
            {
                _preScreenSize = MathHelper.Lerp(_preScreenSize, 0f, 0.3f);
                if (_preScreenSize <= 0.01f)
                {
                    ScreenManager.GetInstance().SetScreen(_preScreen);
                }
            }
            else
            {
                _preScreenSize = MathHelper.Lerp(_preScreenSize, 1f, 0.3f);
            }
        }

        public override void Close()
        {
            Dispose();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_target != null && !_target.IsDisposed) _target.Dispose();
                    if (_backgroundRenderer != null && !_backgroundRenderer.IsDisposed) _backgroundRenderer.Dispose();
                }

                _target = null;
                _backgroundRenderer = null;
            }
            base.Dispose(disposing);
        }
    }
}
