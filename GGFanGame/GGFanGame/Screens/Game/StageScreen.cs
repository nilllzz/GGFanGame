using GGFanGame.Drawing;
using GGFanGame.Game;
using GGFanGame.Game.HUD;
using GGFanGame.Input;
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
    internal class StageScreen : Screen
    {
        private Stage _stage;
        private PlayerStatus _oneStatus, _twoStatus, _threeStatus, _fourStatus;
        private SpriteBatch _batch;

        public StageScreen()
        {
            _stage = StageFactory.Create(Content, "1", "1");
            _stage.Load();
            
            _oneStatus = new PlayerStatus(_stage.OnePlayer, PlayerIndex.One, Content);
            _twoStatus = new PlayerStatus(_stage.TwoPlayer, PlayerIndex.Two, Content);
            _threeStatus = new PlayerStatus(_stage.ThreePlayer, PlayerIndex.Three, Content);
            _fourStatus = new PlayerStatus(_stage.FourPlayer, PlayerIndex.Four, Content);

            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
        }

        public override void Draw()
        {
            _batch.Begin(SpriteBatchUsage.Default);

            // seperate these so they can be called seperately from the pause screen.
            DrawStage();
            DrawHUD();

            _batch.End();
        }

        internal void DrawStage(SpriteBatch batch = null)
        {
            _stage.Draw(batch ?? _batch);
        }

        internal void DrawHUD(SpriteBatch batch = null)
        {
            _oneStatus.Draw(batch ?? _batch);
            _twoStatus.Draw(batch ?? _batch);
            _threeStatus.Draw(batch ?? _batch);
            _fourStatus.Draw(batch ?? _batch);
        }

        public override void Update()
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
            // Zoom out: Y
            if (GetComponent<KeyboardHandler>().KeyDown(Keys.Y) && _stage.Camera.Scale > 0.2)
            {
                _stage.Camera.Scale -= 0.01;
            }
            // Zoom in: X
            if (GetComponent<KeyboardHandler>().KeyDown(Keys.X))
            {
                _stage.Camera.Scale += 0.01;
            }
            // Zoom default: C
            if (GetComponent<KeyboardHandler>().KeyPressed(Keys.OemPipe))
            {
                _stage.Camera.Scale = 2;
            }
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
