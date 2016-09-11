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

        private StageScreen _preScreen;

        private Texture2D _barry, _bubble;
        private SpriteFont _font;
        private RenderTarget2D _target;
        private MenuBackgroundRenderer _backgroundRenderer;

        private float _preScreenSize = 0f;
        private bool _closing = false;

        public PauseScreen(StageScreen preScreen)
        {
            initializeContentManager();

            _preScreen = preScreen;
            _backgroundRenderer = new MenuBackgroundRenderer(Color.Black, new Color(164, 108, 46), new Color(236, 130, 47), new Color(242, 153, 90));
            _backgroundRenderer.applyTransparency = true;

            _barry = content.Load<Texture2D>(@"UI\Pause\barry_pause");
            _bubble = content.Load<Texture2D>(@"UI\Pause\paused_bubble");
            _font = content.Load<SpriteFont>(@"Fonts\CartoonFont");
        }

        public override void draw()
        {
            if (isDisposed) return;

            if (_target == null)
                _target = new RenderTarget2D(gameInstance.GraphicsDevice, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);

            // acquire background and draw it
            _backgroundRenderer.draw();

            // create texture of pre screen
            RenderTargetManager.beginRenderScreenToTarget(_target);
            _preScreen.drawStage();
            RenderTargetManager.endRenderScreenToTarget();

            // draw pre screen:
            var preScreenWidth = _target.Width * (1f - _preScreenSize * SCREEN_SIZE_MULTIPLIER);
            var preScreenHeight = _target.Height * (1f - _preScreenSize * SCREEN_SIZE_MULTIPLIER);

            gameInstance.spriteBatch.Draw(_target, new Rectangle((int)(GameController.RENDER_WIDTH * 0.5f - preScreenWidth * 0.5f),
                                                                 (int)(GameController.RENDER_HEIGHT * 0.5f - preScreenHeight * 0.5f),
                                                                 (int)preScreenWidth,
                                                                 (int)preScreenHeight), Color.White);
            // draw the pre screen's HUD in full size
            _preScreen.drawHUD();

            // draw barry
            int barryWidth = _barry.Width / 2;
            int barryHeight = _barry.Height / 2;
            gameInstance.spriteBatch.Draw(texture: _barry,
                                        destinationRectangle: new Rectangle((int)(-barryWidth + barryWidth * _preScreenSize), 200, barryWidth, barryHeight),
                                        color: Color.White,
                                        effects: SpriteEffects.FlipHorizontally);

            // draw speech bubble
            gameInstance.spriteBatch.Draw(_bubble, new Vector2(barryWidth, 200), new Color(255, 255, 255, (int)(255 * _preScreenSize)));
            gameInstance.spriteBatch.DrawString(_font, "PAUSED", new Vector2(barryWidth + 30, 244), new Color(0, 0, 0, (int)(255 * _preScreenSize)), 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);

            // draw level info
            gameInstance.fontBatch.DrawString(_font, "STAGE: 1-1 (ATTITUDE CITY)\nSTORY MODE", new Vector2(200, GameController.RENDER_HEIGHT - 135 * _preScreenSize), Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
        }

        public override void update()
        {
            _backgroundRenderer.update();

            if (GamePadHandler.buttonPressed(PlayerIndex.One, Buttons.B) && !_closing)
            {
                _closing = true;
            }
            else if (_closing)
            {
                _preScreenSize = MathHelper.Lerp(_preScreenSize, 0f, 0.3f);
                if (_preScreenSize <= 0.01f)
                {
                    ScreenManager.getInstance().setScreen(_preScreen);
                }
            }
            else
            {
                _preScreenSize = MathHelper.Lerp(_preScreenSize, 1f, 0.3f);
            }
        }

        public override void close()
        {
            Dispose();
        }
        
        protected override void dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (_target != null && !_target.IsDisposed) _target.Dispose();
                    if (_backgroundRenderer != null && !_backgroundRenderer.isDisposed) _backgroundRenderer.Dispose();
                }

                _target = null;
                _backgroundRenderer = null;
            }
            base.dispose(disposing);
        }
    }
}
