﻿using System;
using System.Collections.Generic;
using System.IO;
using GGFanGame.Drawing;
using GGFanGame.Input;
using GGFanGame.Screens.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static GameProvider;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The Load Save screen to either load a save or create a new one.
    /// </summary>
    internal class LoadSaveScreen : Screen
    {
        /// <summary>
        /// Container to draw a save state.
        /// </summary>
        private class SaveContainer
        {
            private readonly GameSession _session;
            private int _gradientState;
            private bool _gradientFading;
            private float _targetPercent;

            private readonly Texture2D _grumpFaceTexture;

            public SaveContainer(int index, GameSession session)
            {
                _session = session;
                this.Index = index;

                if (_session != null)
                {
                    if (_session.LoadedCorrectly)
                    {
                        _grumpFaceTexture = GameInstance.Content.Load<Texture2D>(@"UI\HUD\" + _session.LastGrump);
                    }
                }
            }

            public int Index { get; }

            public bool NewGameButton { get; set; }

            /// <summary>
            /// Draws the save state.
            /// </summary>
            public void Draw(SpriteFont font, Rectangle targetRect, bool selected, float alphaDelta)
            {
                if (selected)
                {
                    GameInstance.SpriteBatch.Draw(GameInstance.Content.Load<Texture2D>(@"UI\saveBack"), targetRect, new Color(0, 0, 0, (int)(255 * alphaDelta)));
                }
                else
                {
                    GameInstance.SpriteBatch.Draw(GameInstance.Content.Load<Texture2D>(@"UI\saveBack"), targetRect, new Color(255, 255, 255, (int)(255 * alphaDelta)));
                }

                if (NewGameButton)
                {
                    GameInstance.SpriteBatch.DrawString(font, "+ Create new game", new Vector2(targetRect.X + 64, targetRect.Y + 50), new Color(255, 255, 255, (int)(255 * alphaDelta)));
                }
                else
                {
                    if (_session.LoadedCorrectly)
                    {
                        GameInstance.SpriteBatch.Draw(_grumpFaceTexture, new Rectangle(targetRect.X + 16, targetRect.Y + 16, 96, 96), new Rectangle(0, 0, 48, 48), new Color(255, 255, 255, (int)(255 * alphaDelta)));
                        GameInstance.SpriteBatch.DrawString(font, _session.Name, new Vector2(targetRect.X + 128, targetRect.Y + 24), new Color(255, 255, 255, (int)(255 * alphaDelta)));
                        GameInstance.SpriteBatch.DrawString(font, Math.Round(_targetPercent, 2) + "%", new Vector2(targetRect.X + 456, targetRect.Y + 70), new Color(255, 255, 255, (int)(255 * alphaDelta)));

                        Graphics.DrawRectangle(new Rectangle(targetRect.X + 128, targetRect.Y + 70, 300, 32), new Color(0, 0, 0, (int)(255 * alphaDelta)));

                        var width = (int)(292 * (_targetPercent / 100));

                        var gradientProgress = (double)_gradientState / 255;
                        var fromR = (int)(240 * gradientProgress);
                        var fromG = (int)(136 * gradientProgress);
                        var fromB = (int)(47 * gradientProgress);
                        var toR = 164 + (int)(79 * gradientProgress);
                        var toG = 108 + (int)(68 * gradientProgress);
                        var toB = 46 + (int)(37 * gradientProgress);

                        Graphics.DrawGradient(new Rectangle(targetRect.X + 132, targetRect.Y + 74, width, 24),
                                                      new Color(fromR, fromG, fromB, (int)(255 * alphaDelta)), new Color(toR, toG, toB, (int)(255 * alphaDelta)), false, 1d);

                        if (_gradientFading)
                        {
                            _gradientState -= 3;
                            if (_gradientState <= 0)
                            {
                                _gradientState = 0;
                                _gradientFading = false;
                            }
                        }
                        else
                        {
                            _gradientState += 3;
                            if (_gradientState >= 255)
                            {
                                _gradientState = 255;
                                _gradientFading = true;
                            }
                        }

                        if (_targetPercent < (float)_session.Progress)
                        {
                            _targetPercent = MathHelper.Lerp((float)_session.Progress, _targetPercent, 0.92f);
                        }
                    }
                }
            }
        }

        private readonly List<SaveContainer> _saves = new List<SaveContainer>();

        private float _offset;
        private int _selected;
        private int _alpha;

        private readonly SpriteFont _grumpFont;
        private readonly MenuBackgroundRenderer _backgroundRenderer;

        public LoadSaveScreen(MenuBackgroundRenderer backgroundRenderer)
        {
            _backgroundRenderer = backgroundRenderer;
            
            var saveIndex = 0;
            foreach (var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\Saves\", "*.json", SearchOption.TopDirectoryOnly))
            {
                _saves.Add(new SaveContainer(saveIndex, new GameSession(file)));

                saveIndex++;
            }

            _saves.Add(new SaveContainer(saveIndex, null) { NewGameButton = true });

            _grumpFont = GameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFont");
        }

        public override void Update()
        {
            _backgroundRenderer.Update();

            if (IsCurrentScreen)
            {
                if (ControlsHandler.DownPressed(PlayerIndex.One))
                {
                    _selected++;
                }
                if (ControlsHandler.UpPressed(PlayerIndex.One))
                {
                    _selected--;
                }

                if (GamePadHandler.ButtonPressed(PlayerIndex.One, Buttons.A))
                {
                    var save = _saves[_selected];
                    if (save.NewGameButton)
                    {
                        ScreenManager.GetInstance().SetScreen(new TransitionScreen(this, new NewGameScreen(this, _backgroundRenderer)));
                    }
                    else
                    {
                        ScreenManager.GetInstance().SetScreen(new TransitionScreen(this, new StageScreen()));
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
            _backgroundRenderer.Draw(GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);

            for (var i = 0; i < _saves.Count; i++)
            {
                _saves[i].Draw(_grumpFont, new Rectangle(GameController.RENDER_WIDTH / 2 - 300, GameController.RENDER_HEIGHT / 2 - 64 + (int)_offset + _saves[i].Index * 160, 600, 128), i == _selected, _alpha / 255f);
            }
        }
    }
}