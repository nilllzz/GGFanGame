﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The Load Save screen to either load a save or create a new one.
    /// </summary>
    class LoadSaveScreen : Screen
    {
        /// <summary>
        /// Container to draw a save state.
        /// </summary>
        private class SaveContainer
        {
            int _index;
            GameSession _save;
            int _gradientState = 0;
            bool _gradientFading = false;
            bool _newGameButton = false;
            float _targetPercent = 0f;

            Texture2D _grumpFaceTexture;

            public SaveContainer(int index, GameSession save)
            {
                _save = save;
                _index = index;

                if (_save != null)
                {
                    if (_save.loadedCorrectly)
                    {
                        _grumpFaceTexture = gameInstance.textureManager.load(@"UI\HUD\" + _save.lastGrump);
                    }
                }
            }

            public int index => _index;

            public bool newGameButton
            {
                set { _newGameButton = value; }
            }

            /// <summary>
            /// Draws the save state.
            /// </summary>
            public void draw(SpriteFont font, Rectangle targetRect, bool selected, float alphaDelta)
            {
                if (selected)
                {
                    gameInstance.spriteBatch.Draw(gameInstance.textureManager.load(@"UI\saveBack"), targetRect, new Color(0, 0, 0, (int)(255 * alphaDelta)));
                }
                else
                {
                    gameInstance.spriteBatch.Draw(gameInstance.textureManager.load(@"UI\saveBack"), targetRect, new Color(255, 255, 255, (int)(255 * alphaDelta)));
                }

                if (_newGameButton)
                {
                    gameInstance.spriteBatch.DrawString(font, "+ Create new game", new Vector2(targetRect.X + 64, targetRect.Y + 50), new Color(255, 255, 255, (int)(255 * alphaDelta)));
                }
                else
                {
                    if (_save.loadedCorrectly)
                    {
                        gameInstance.spriteBatch.Draw(_grumpFaceTexture, new Rectangle(targetRect.X + 16, targetRect.Y + 16, 96, 96), new Rectangle(0, 0, 48, 48), new Color(255, 255, 255, (int)(255 * alphaDelta)));
                        gameInstance.spriteBatch.DrawString(font, _save.name, new Vector2(targetRect.X + 128, targetRect.Y + 24), new Color(255, 255, 255, (int)(255 * alphaDelta)));
                        gameInstance.spriteBatch.DrawString(font, Math.Round(_targetPercent, 2) + "%", new Vector2(targetRect.X + 456, targetRect.Y + 70), new Color(255, 255, 255, (int)(255 * alphaDelta)));

                        Drawing.Graphics.drawRectangle(new Rectangle(targetRect.X + 128, targetRect.Y + 70, 300, 32), new Color(0, 0, 0, (int)(255 * alphaDelta)));

                        int width = (int)(292 * (_targetPercent / 100));

                        double gradientProgress = (double)_gradientState / 255;
                        int fromR = (int)(240 * gradientProgress);
                        int fromG = (int)(136 * gradientProgress);
                        int fromB = (int)(47 * gradientProgress);
                        int toR = 164 + (int)(79 * gradientProgress);
                        int toG = 108 + (int)(68 * gradientProgress);
                        int toB = 46 + (int)(37 * gradientProgress);

                        Drawing.Graphics.drawGradient(new Rectangle(targetRect.X + 132, targetRect.Y + 74, width, 24),
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

                        if (_targetPercent < (float)_save.progress)
                        {
                            _targetPercent = MathHelper.Lerp((float)_save.progress, _targetPercent, 0.92f);
                        }
                    }
                }
            }
        }

        private List<SaveContainer> _saves = new List<SaveContainer>();

        private float _offset = 0f;
        private int _selected = 0;
        private int _alpha = 0;

        private SpriteFont _grumpFont = null;
        private MenuBackgroundRenderer _backgroundRenderer;

        public LoadSaveScreen(MenuBackgroundRenderer backgroundRenderer)
        {
            _backgroundRenderer = backgroundRenderer;

            int saveIndex = 0;
            foreach (string file in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\Saves\", "*.json", System.IO.SearchOption.TopDirectoryOnly))
            {
                _saves.Add(new SaveContainer(saveIndex, new GameSession(file)));

                saveIndex++;
            }

            _saves.Add(new SaveContainer(saveIndex, null) { newGameButton = true });

            _grumpFont = gameInstance.fontManager.load(@"CartoonFont");
        }

        public override void update()
        {
            _backgroundRenderer.update();

            if (isCurrentScreen)
            {
                if (Input.ControlsHandler.downPressed(PlayerIndex.One))
                {
                    _selected++;
                }
                if (Input.ControlsHandler.upPressed(PlayerIndex.One))
                {
                    _selected--;
                }

                if (Input.GamePadHandler.buttonPressed(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.A))
                {
                    ScreenManager.getInstance().setScreen(new TransitionScreen(this, new Game.StageScreen()));
                    //ScreenManager.getInstance().setScreen(new TransitionScreen(gameInstance, this, new PlayerSelectScreen()));
                }
            }

            _offset = MathHelper.Lerp(getTargetOffset(), _offset, 0.9f);

            if (_alpha < 255)
            {
                _alpha += 10;
                if (_alpha > 255)
                {
                    _alpha = 255;
                }
            }
        }

        private float getTargetOffset()
        {
            return -(_selected * 160);
        }

        public override void draw()
        {
            _backgroundRenderer.draw();

            for (int i = 0; i < _saves.Count; i++)
            {
                _saves[i].draw(_grumpFont, new Rectangle(GameController.RENDER_WIDTH / 2 - 300, GameController.RENDER_HEIGHT / 2 - 64 + (int)_offset + _saves[i].index * 160, 600, 128), i == _selected, _alpha / 255f);
            }
        }
    }
}