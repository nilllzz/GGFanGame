using GameDevCommon.Drawing;
using GGFanGame.Game;
using GGFanGame.Game.HUD;
using GameDevCommon.Input;
using GGFanGame.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace GGFanGame.Screens.Game
{
    /// <summary>
    /// Main screen to play stages in.
    /// </summary>
    internal class InGameScreen : StageScreen
    {
        private Stage _stage;
        private PlayerStatus _oneStatus, _twoStatus, _threeStatus, _fourStatus;
        private SpriteBatch _batch;

        public InGameScreen()
        {
            _stage = StageFactory.Create(Content, "grumpSpace", "dojo");
            _stage.LoadContent();

            _oneStatus = new PlayerStatus(_stage.OnePlayer, PlayerIndex.One, Content);
            _twoStatus = new PlayerStatus(_stage.TwoPlayer, PlayerIndex.Two, Content);
            _threeStatus = new PlayerStatus(_stage.ThreePlayer, PlayerIndex.Three, Content);
            _fourStatus = new PlayerStatus(_stage.FourPlayer, PlayerIndex.Four, Content);

            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
        }

        public override void Draw(GameTime time)
        {
            // seperate these so they can be called seperately from the pause screen.

            RenderStage();

            _batch.Begin(SpriteBatchUsage.Default);

            DrawStage();
            DrawHUD();

            _batch.End();
        }

        internal override void RenderStage()
        {
            //_stage.Render();
        }

        internal override void DrawStage(SpriteBatch batch = null)
        {
            _stage.Draw(batch ?? _batch);
        }

        internal override void DrawHUD(SpriteBatch batch = null)
        {
            _oneStatus.Draw(batch ?? _batch);
            _twoStatus.Draw(batch ?? _batch);
            _threeStatus.Draw(batch ?? _batch);
            _fourStatus.Draw(batch ?? _batch);
        }

        public override void Update(GameTime time)
        {
            _stage.Update();

            if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.Start))
            {
                GetComponent<ScreenManager>().SetScreen(new PauseScreen(this));
            }

            //TEST CODE: When pressed P, rendering switches to 3D bounding box test stage:
            if (GetComponent<KeyboardHandler>().KeyPressed(Keys.P))
            {
                GetComponent<ScreenManager>().SetScreen(new Debug.BoundingBoxTestScreen());
            }

            UpdateHUD();
        }

        private void UpdateHUD()
        {
            _oneStatus.Update(_stage.TimeDelta);
            _twoStatus.Update(_stage.TimeDelta);
            _threeStatus.Update(_stage.TimeDelta);
            _fourStatus.Update(_stage.TimeDelta);
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_oneStatus != null && !_oneStatus.IsDisposed) _oneStatus.Dispose();
                    if (_twoStatus != null && !_twoStatus.IsDisposed) _twoStatus.Dispose();
                    if (_threeStatus != null && !_threeStatus.IsDisposed) _threeStatus.Dispose();
                    if (_fourStatus != null && !_fourStatus.IsDisposed) _fourStatus.Dispose();
                }

                _oneStatus = null;
                _twoStatus = null;
                _threeStatus = null;
                _fourStatus = null;
            }

            base.Dispose(disposing);
        }
    }
}
