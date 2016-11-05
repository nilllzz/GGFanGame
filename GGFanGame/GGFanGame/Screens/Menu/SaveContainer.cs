using System;
using GGFanGame.Content;
using GGFanGame.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// Container to draw a save state.
    /// </summary>
    internal sealed class SaveContainer
    {
        private readonly GameSession _session;
        private int _gradientState;
        private bool _gradientFading;
        private float _targetPercent;

        private readonly Texture2D _grumpFaceTexture;

        public SaveContainer(int index, GameSession session)
        {
            _session = session;
            Index = index;

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
        public void Draw(SpriteBatch batch, SpriteFont font, Rectangle targetRect, bool selected, float alphaDelta)
        {
            if (selected)
            {
                batch.Draw(GameInstance.Content.Load<Texture2D>(Resources.UI.SaveBack), targetRect, new Color(0, 0, 0, (int)(255 * alphaDelta)));
            }
            else
            {
                batch.Draw(GameInstance.Content.Load<Texture2D>(Resources.UI.SaveBack), targetRect, new Color(255, 255, 255, (int)(255 * alphaDelta)));
            }

            if (NewGameButton)
            {
                batch.DrawString(font, "+ Create new game", new Vector2(targetRect.X + 64, targetRect.Y + 50), new Color(255, 255, 255, (int)(255 * alphaDelta)));
            }
            else
            {
                if (_session.LoadedCorrectly)
                {
                    batch.Draw(_grumpFaceTexture, new Rectangle(targetRect.X + 16, targetRect.Y + 16, 96, 96), new Rectangle(0, 0, 48, 48), new Color(255, 255, 255, (int)(255 * alphaDelta)));
                    batch.DrawString(font, _session.Name, new Vector2(targetRect.X + 128, targetRect.Y + 24), new Color(255, 255, 255, (int)(255 * alphaDelta)));
                    batch.DrawString(font, Math.Round(_targetPercent, 2) + "%", new Vector2(targetRect.X + 456, targetRect.Y + 70), new Color(255, 255, 255, (int)(255 * alphaDelta)));

                    batch.DrawRectangle(new Rectangle(targetRect.X + 128, targetRect.Y + 70, 300, 32), new Color(0, 0, 0, (int)(255 * alphaDelta)));

                    var width = (int)(292 * (_targetPercent / 100));

                    var gradientProgress = (double)_gradientState / 255;
                    var fromR = (int)(240 * gradientProgress);
                    var fromG = (int)(136 * gradientProgress);
                    var fromB = (int)(47 * gradientProgress);
                    var toR = 164 + (int)(79 * gradientProgress);
                    var toG = 108 + (int)(68 * gradientProgress);
                    var toB = 46 + (int)(37 * gradientProgress);

                    batch.DrawGradient(new Rectangle(targetRect.X + 132, targetRect.Y + 74, width, 24),
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

}
