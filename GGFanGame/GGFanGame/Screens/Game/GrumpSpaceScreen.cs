using GGFanGame.Drawing;
using GGFanGame.Game;
using GGFanGame.Input;
using GGFanGame.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace GGFanGame.Screens.Game
{
    /// <summary>
    /// The screen that is active during the Grump Space scenes.
    /// </summary>
    internal class GrumpSpaceScreen : StageScreen
    {
        private Stage _stage;
        private SpriteBatch _batch;

        public GrumpSpaceScreen()
        {
            _stage = StageFactory.Create(Content, "grumpSpace", "main");
            _stage.LoadContent();

            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
        }

        public override void Draw(GameTime time)
        {
            RenderStage();

            _batch.Begin(SpriteBatchUsage.Default);

            DrawStage();
            DrawHUD();

            _batch.End();
        }

        internal override void RenderStage()
        {
            _stage.Render();
        }

        internal override void DrawHUD(SpriteBatch batch = null)
        {

        }

        internal override void DrawStage(SpriteBatch batch = null)
        {
            _stage.Draw(batch ?? _batch);
        }

        public override void Update(GameTime time)
        {
            _stage.Update();

            if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.Start))
            {
                GetComponent<ScreenManager>().SetScreen(new PauseScreen(this));
            }
        }
    }
}
