using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Drawing
{
    /// <summary>
    /// Handles blur drawing.
    /// </summary>
    internal class BlurHandler : IDisposable
    {
        private const int BLUR_RADIUS = 7;
        private const float BLUR_AMOUNT = 2.0f;
        
        private GaussianBlur _blurCore;

        private RenderTarget2D _rt1, _rt2;

        internal bool isDisposed { get; private set; } 

        /// <summary>
        /// Creates a new instance of the <see cref="BlurHandler"/> class.
        /// </summary>
        /// <param name="width">The width of the target texture.</param>
        /// <param name="height">The height of the target texture.</param>
        public BlurHandler(int width, int height)
        {
            _blurCore = new GaussianBlur();
            _blurCore.computeKernel(BLUR_RADIUS, BLUR_AMOUNT);

            var renderTargetWidth = width / 2;
            var renderTargetHeight = height / 2;

            _rt1 = new RenderTarget2D(gameInstance.GraphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                gameInstance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            _rt2 = new RenderTarget2D(gameInstance.GraphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                gameInstance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            _blurCore.computeOffsets(renderTargetWidth, renderTargetHeight);
        }

        /// <summary>
        /// Draws a texture with a blur effect.
        /// </summary>
        public void draw(Texture2D drawTexture)
        {
            var result = _blurCore.performGaussianBlur(drawTexture, _rt1, _rt2);

            gameInstance.GraphicsDevice.Clear(Color.White);
            gameInstance.spriteBatch.Draw(result, new Rectangle(0, 0, drawTexture.Width, drawTexture.Height), Color.White);
        }

        /// <summary>
        /// Applies the blur effect to a texture.
        /// </summary>
        /// <param name="t">The texture to be blurred.</param>
        /// <returns>Returns the blurred texture.</returns>
        internal Texture2D blurTexture(Texture2D t)
        {
            return _blurCore.performGaussianBlur(t, _rt1, _rt2);
        }

        public void Dispose()
        {
            dispose(true);
        }

        ~BlurHandler()
        {
            dispose(false);
        }

        private void dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (_rt1 != null && !_rt1.IsDisposed) _rt1.Dispose();
                    if (_rt2 != null && !_rt2.IsDisposed) _rt2.Dispose();
                    if (_blurCore != null && !_blurCore.isDisposed) _blurCore.Dispose();
                }

                _rt1 = null;
                _rt2 = null;
                _blurCore = null;

                isDisposed = true;
            }
        }
    }
}