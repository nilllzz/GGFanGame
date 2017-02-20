using System;
using System.Collections.Generic;
using System.IO;
using GGFanGame.Content;
using GGFanGame.Drawing;
using GGFanGame.Input;
using GGFanGame.Screens.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The Load Save screen to either load a save or create a new one.
    /// </summary>
    internal class LoadSaveScreen : Screen
    {
        private readonly List<SaveContainer> _saves = new List<SaveContainer>();

        private float _offset;
        private int _selected;
        private int _alpha;

        private readonly SpriteFont _grumpFont;
        private MenuBackgroundRenderer _backgroundRenderer;
        private SpriteBatch _batch;

        public LoadSaveScreen(MenuBackgroundRenderer backgroundRenderer)
        {
            _backgroundRenderer = backgroundRenderer;
            _batch = new SpriteBatch(GameInstance.GraphicsDevice);

            var saveIndex = 0;
            foreach (var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\Saves\", "*.json", SearchOption.TopDirectoryOnly))
            {
                _saves.Add(new SaveContainer(saveIndex, new GameSession(file)));

                saveIndex++;
            }

            _saves.Add(new SaveContainer(saveIndex, null) { NewGameButton = true });

            _grumpFont = GameInstance.Content.Load<SpriteFont>(Resources.Fonts.CartoonFont);
        }

        public override void Update()
        {
            _backgroundRenderer.Update();

            if (IsCurrentScreen)
            {
                if (GetComponent<ControlsHandler>().DownPressed(PlayerIndex.One))
                {
                    _selected++;
                }
                if (GetComponent<ControlsHandler>().UpPressed(PlayerIndex.One))
                {
                    _selected--;
                }

                if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.A))
                {
                    var save = _saves[_selected];
                    if (save.NewGameButton)
                    {
                        GetComponent<ScreenManager>().SetScreen(new TransitionScreen(this, new NewGameScreen(this, _backgroundRenderer.Clone())));
                    }
                    else
                    {
                        GetComponent<ScreenManager>().SetScreen(new TransitionScreen(this, new StageScreen()));
                    }
                    //ScreenManager.getInstance().setScreen(new TransitionScreen(gameInstance, this, new PlayerSelectScreen()));
                }
            }

            _offset = MathHelper.Lerp(GetTargetOffset(), _offset, 0.9f);

            if (_alpha < 255)
            {
                _alpha += 10;
                if (_alpha > 255)
                {
                    _alpha = 255;
                }
            }
        }

        private float GetTargetOffset()
            => -(_selected * 160);

        public override void Draw()
        {
            _batch.Begin(SpriteBatchUsage.Default);

            _backgroundRenderer.Draw(_batch, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);

            for (var i = 0; i < _saves.Count; i++)
            {
                _saves[i].Draw(_batch, _grumpFont, new Rectangle(GameController.RENDER_WIDTH / 2 - 300, GameController.RENDER_HEIGHT / 2 - 64 + (int)_offset + _saves[i].Index * 160, 600, 128), i == _selected, _alpha / 255f);
            }

            _batch.End();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_backgroundRenderer != null && !_backgroundRenderer.IsDisposed) _backgroundRenderer.Dispose();
                    if (_batch != null && !_batch.IsDisposed) _batch.Dispose();
                }

                _batch = null;
                _backgroundRenderer = null;
            }

            base.Dispose(disposing);
        }
    }
}
