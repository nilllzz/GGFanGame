using GameDevCommon;
using GameDevCommon.Drawing;
using GameDevCommon.Input;
using GGFanGame.Content;
using GGFanGame.Game;
using GGFanGame.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using static Core;

namespace GGFanGame.Screens.Game
{
    /// <summary>
    /// The screen that is active during the Grump Space scenes.
    /// </summary>
    internal class GrumpSpaceScreen : StageScreen
    {
        private Stage _stage;
        private string _stageId, _worldId;
        private SpriteBatch _batch;
        private RenderTarget2D _stageTarget;
        private BlurHandler _blur;
        private Texture2D _dotOverlay;

        // when stepped into a portal, store destination information
        private bool _portalOpen = false;
        private string _portalTo = "";
        private Vector3 _portalPosition;

        // used for drawing the title of the stage
        private RenderTarget2D _titleTarget;
        private SpriteFont _titleFont;
        private SpriteBatch _titleBatch;
        private MenuBackgroundRenderer _titleBackground;
        private float _titleIntro = 0f;
        private float _titleDelay = 0f;
        private bool _titleOut = false;

        internal StageCamera Camera { get; private set; }

        public GrumpSpaceScreen(string worldId, string stageId, Vector3 initialPosition)
        {
            _stageId = stageId;
            _worldId = worldId;
            _portalPosition = initialPosition;
        }

        public override void LoadContent()
        {
            _stage = StageFactory.Create(Content, _worldId, _stageId);
            _stage.LoadContent();
            _stage.OnePlayer.Position = _portalPosition;
            Camera = new StageCamera(_stage.OnePlayer);

            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
            _blur = new BlurHandler(EffectHelper.GetGaussianBlurEffect(Content), _batch, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);
            _stageTarget = RenderTargetManager.CreateScreenTarget();
            _dotOverlay = Content.Load<Texture2D>(Resources.UI.Portal.dot_overlay);

            _titleBatch = new SpriteBatch(GameInstance.GraphicsDevice);
            _titleFont = Content.Load<SpriteFont>(Resources.Fonts.CartoonFontLarge);
            var titleSize = _titleFont.MeasureString(_stage.Name);
            _titleTarget = RenderTargetManager.CreateRenderTarget((int)Math.Ceiling(titleSize.X), (int)Math.Ceiling(titleSize.Y));
            _titleBackground = new MenuBackgroundRenderer(
               new Color(235, 148, 48),
               new Color(242, 167, 76),
               new Color(236, 130, 47),
               new Color(242, 153, 90));
        }

        public override void Draw(GameTime time)
        {
            GameInstance.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.Transparent, 1.0f, 0);
            RenderStage();

            DrawStage();
            DrawHUD();
        }

        internal override void RenderStage()
        {
            if (_portalOpen)
            {
                RenderTargetManager.BeginRenderToTarget(_stageTarget);

                _stage.Render(Camera);

                RenderTargetManager.EndRenderToTarget();
            }
            else
                _stage.Render(Camera);
        }

        internal override void DrawHUD(SpriteBatch batch = null)
        {
            RenderTargetManager.BeginRenderToTarget(_titleTarget);
            GameInstance.GraphicsDevice.Clear(Color.Transparent);

            _titleBatch.Begin(SpriteBatchUsage.Font);

            _titleBatch.DrawString(_titleFont, _stage.Name,
                new Vector2(0, _titleTarget.Height * (1f - _titleIntro)), Color.White);

            _titleBatch.End();

            RenderTargetManager.EndRenderToTarget();

            var texture = _titleBackground.CreateBackgroundTexture(GameInstance.ClientRectangle.Width, GameInstance.ClientRectangle.Height);
            _batch.DrawMask(() =>
            {
                _batch.Draw(_titleTarget,
                    new Rectangle(
                        _titleTarget.Width / 2 + 50,
                        (int)(_titleTarget.Height * 1.5f),
                        _titleTarget.Width,
                        _titleTarget.Height
                    ),
                    _titleTarget.Bounds, Color.White,
                    -0.2f, new Vector2(_titleTarget.Width / 2f, _titleTarget.Height / 2f), SpriteEffects.None, 0f);

                _batch.DrawRectangle(new Rectangle(
                    _titleTarget.Width / 2 + 60,
                    (int)(_titleTarget.Height * 2.5f) + 20,
                    (int)(_titleTarget.Width * _titleIntro), 10), Color.White, -0.2f);
            }, () =>
            {
                _batch.Draw(texture, Vector2.Zero, Color.White);
            });

            _batch.Begin(SpriteBatchUsage.Default);
            _batch.End();
        }

        internal override void DrawStage(SpriteBatch batch = null)
        {
            _batch.Begin(SpriteBatchUsage.Default);

            _stage.Draw(batch ?? _batch);
            if (_portalOpen)
            {
                var blurred = _blur.BlurTexture(_stageTarget);
                _batch.Draw(blurred, GameInstance.ClientRectangle, Color.White);

                for (int x = 0; x < GameInstance.ClientRectangle.Width; x += _dotOverlay.Width)
                {
                    for (int y = 0; y < GameInstance.ClientRectangle.Height; y += _dotOverlay.Height)
                    {
                        _batch.Draw(_dotOverlay, new Vector2(x, y), Color.White);
                    }
                }
            }

            _batch.End();
        }

        public override void Update(GameTime time)
        {
            _titleBackground.Update();

            if (_titleDelay < 1f)
            {
                _titleDelay += 0.01f;
                if (_titleDelay >= 1f)
                    _titleDelay = 1f;
            }
            else
            {
                if (_titleOut)
                {
                    if (_titleIntro > 0f)
                    {
                        _titleIntro = MathHelper.Lerp(_titleIntro, 0f, 0.1f);
                        if (_titleIntro <= 0.01f)
                            _titleIntro = 0f;
                    }
                }
                else
                {
                    if (_titleIntro < 1f)
                    {
                        _titleIntro = MathHelper.Lerp(_titleIntro, 1f, 0.1f);
                        if (_titleIntro >= 0.999f)
                        {
                            _titleIntro = 1f;
                            _titleOut = true;
                            _titleDelay = 0f;
                        }
                    }
                }
            }

            if (!_portalOpen)
                _stage.Update();
            else
            {
                if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.A))
                {
                    var destinationInfo = _portalTo.Split('/');
                    var transitionScreen = new TransitionScreen(this,
                        new GrumpSpaceScreen(destinationInfo[0], destinationInfo[1], _portalPosition));
                    GetComponent<ScreenManager>().SetScreen(transitionScreen);
                }
            }

            Camera.Update();

            if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.Start))
            {
                GetComponent<ScreenManager>().SetScreen(new PauseScreen(this));
            }

            if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.Back))
            {
                // copy stages from content to output:
                // DEBUG
                var sourceDir = @"C:\Users\Nils\Projects\git\GGFanGame\src\GGFanGame.Content\Content\Levels";
                var targetDir = @"C:\Users\Nils\Projects\git\GGFanGame\src\GGFanGame\bin\Windows\Debug\Content\Levels";
                foreach (var sourceFile in Directory.GetFiles(sourceDir, "*.json", SearchOption.AllDirectories))
                {
                    var targetFile = sourceFile.Replace(sourceDir, targetDir);
                    File.Copy(sourceFile, targetFile, true);
                }

                // reload stage
                _stage = StageFactory.Create(Content, _stage.WorldId, _stage.StageId);
                _stage.LoadContent();
                Camera.FollowObject = _stage.OnePlayer;
                _titleOut = false;
                _titleDelay = 0f;
                _titleIntro = 0f;
            }
        }

        public void InitiatePortal(Vector3 position, string to)
        {
            _portalOpen = true;
            _portalPosition = position;
            _portalTo = to;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_batch != null && !_batch.IsDisposed) _batch.Dispose();
                    if (_stage != null && !_stage.IsDisposed) _stage.Dispose();
                    if (_stageTarget != null && !_stageTarget.IsDisposed) _stageTarget.Dispose();
                    if (_blur != null && !_blur.IsDisposed) _blur.Dispose();
                    if (_titleTarget != null && !_titleTarget.IsDisposed) _titleTarget.Dispose();
                    if (_titleBatch != null && !_titleBatch.IsDisposed) _titleBatch.Dispose();
                    if (_titleBackground != null && !_titleBackground.IsDisposed) _titleBackground.Dispose();
                }

                _batch = null;
                _stage = null;
                _stageTarget = null;
                _blur = null;
                _titleTarget = null;
                _titleBatch = null;
                _titleBackground = null;
            }

            base.Dispose(disposing);
        }
    }
}
