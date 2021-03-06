﻿using System;
using GGFanGame.Content;
using GGFanGame.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;
using GameDevCommon.Drawing;
using System.Threading.Tasks;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The screen to display a Game Grumps transition for switching between screens.
    /// </summary>
    internal class TransitionScreen : Screen
    {
        internal override bool ReplacePrevious => false;

        private SpriteBatch _batch;
        private readonly Texture2D _ggOverlay, _loadingAnimation;
        private float _overlaySize = 80f;
        private float _rotation;

        private bool _loading = false;
        private int _loadingFrame = 0;
        private float _loadingFrameDelay = 1f;

        // If the screen outro is playing, or the intro.
        private bool _outro = true;

        // out screen is the current one, inscreen the new one.
        private readonly Screen _outScreen, _inScreen;

        public TransitionScreen(Screen outScreen, Screen inScreen)
        {
            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
            _ggOverlay = GameInstance.Content.Load<Texture2D>(Resources.UI.Logos.GameGrumpsTransition);
            _loadingAnimation = GameInstance.Content.Load<Texture2D>(Resources.UI.Loading);
            _outScreen = outScreen;
            _inScreen = inScreen;
        }

        public override void Draw(GameTime time)
        {
            _batch.Begin(SpriteBatchUsage.Default);

            if (!_loading)
            {
                if (_outro)
                    _outScreen.Draw(time);
                else
                    _inScreen.Draw(time);
            }

            if (_overlaySize > 0)
            {
                // Render the rotating logo:
                _batch.Draw(_ggOverlay, new Rectangle(GameController.RENDER_WIDTH / 2,
                                                        GameController.RENDER_HEIGHT / 2,
                                                        (int)(_ggOverlay.Width * _overlaySize),
                                                        (int)(_ggOverlay.Height * _overlaySize)),
                    null, Color.White, _rotation, new Vector2(_ggOverlay.Width / 2, _ggOverlay.Height / 2), SpriteEffects.None, 0f);

                // Get the space between the edges of the screen and the logo.
                var diffX = GameController.RENDER_WIDTH - (_ggOverlay.Width * _overlaySize);
                var diffY = GameController.RENDER_HEIGHT - (_ggOverlay.Height * _overlaySize);

                var addSide = (int)(160 * _overlaySize);

                // When needed, draw black rectangles at the side:
                if (diffX + 50 > 0)
                {
                    _batch.DrawRectangle(new Rectangle(0, 0, (int)(diffX * 0.5f) + addSide, GameController.RENDER_HEIGHT), Color.Black);
                    _batch.DrawRectangle(new Rectangle(GameController.RENDER_WIDTH - (int)Math.Floor(diffX / 2) - 2 - addSide, 0, (int)Math.Ceiling(diffX / 2) + 2 + addSide, GameController.RENDER_HEIGHT), Color.Black);
                    _batch.DrawRectangle(new Rectangle((int)(diffX / 2), 0, (int)(GameController.RENDER_WIDTH - diffX) + 1, (int)(diffY / 2) + 1 + addSide), Color.Black);
                    _batch.DrawRectangle(new Rectangle((int)(diffX / 2), GameController.RENDER_HEIGHT - (int)Math.Floor(diffY / 2) - 2 - addSide, (int)(GameController.RENDER_WIDTH - diffX), (int)(diffY / 2) + 2 + addSide), Color.Black);
                }

                // Draw slightly fading rectangle.
                _batch.DrawRectangle(GameInstance.ClientRectangle, new Color(0, 0, 0, (int)(255 * (1f - _overlaySize / 2f))));
            }
            else
            {
                _batch.DrawRectangle(GameInstance.ClientRectangle, Color.Black);
            }

            if (_loading)
            {
                _batch.Draw(_loadingAnimation, new Rectangle(GameController.RENDER_WIDTH / 2 - 64, GameController.RENDER_HEIGHT / 2 - 64, 128, 128),
                    new Rectangle(_loadingFrame * 64, 0, 64, 64), Color.White);
            }

            _batch.End();
        }

        public override void Update(GameTime time)
        {
            if (_loading)
            {
                _loadingFrameDelay -= 0.15f;
                if (_loadingFrameDelay <= 0f)
                {
                    _loadingFrameDelay = 1f;
                    _loadingFrame++;
                    if (_loadingFrame * 64 == _loadingAnimation.Width)
                        _loadingFrame = 0;
                }
                return;
            }

            if (_outro)
            {
                _overlaySize = MathHelper.Lerp(0f, _overlaySize, 0.9f);
                _rotation -= 0.08f;
                _outScreen.Update(time);

                if (_overlaySize - 0.01f <= 0f)
                {
                    _overlaySize = 0.01f;
                    _outro = false;
                    _loading = true;
                    Task.Run(() => LoadScreenContent());
                }
            }
            else
            {
                _overlaySize += MathHelper.Lerp(0f, 80f, 0.92f * (_overlaySize / 600f)); // It works, dont question why.
                _rotation += 0.08f;
                _inScreen.Update(time);

                // Once the intro animation is done, switch to the new screen.
                if (_overlaySize + 0.01f >= 80f)
                {
                    GetComponent<ScreenManager>().SetScreen(_inScreen);
                }
            }
        }

        private void LoadScreenContent()
        {
            _outScreen.Close(); // the screen closes once the transition animation is over.
            _inScreen.LoadContent();
            _loading = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_batch != null && !_batch.IsDisposed) _batch.Dispose();
                }

                _batch = null;
            }

            base.Dispose(disposing);
        }
    }
}
