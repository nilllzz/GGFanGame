using System;
using GGFanGame.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Screens.Menu
{
    internal class MenuBackgroundRenderer : IDisposable
    {
        //The offset of the dots:
        private float _offsetX,
                      _offsetY;

        //The size reference of the dots in the background in pixels
        private const int DOT_SIZE = 16;

        private readonly Color _backgroundFromColor,
                               _backgroundToColor,
                               _dotFromColor,
                               _dotToColor,
                               _dotColorDiff;

        private BlurHandler _blurHandler;
        private RenderTarget2D _target;
        private SpriteBatch _batch; // TODO: dispose

        internal bool ApplyTransparency { get; set; }

        internal bool IsDisposed { get; private set; }
        
        internal MenuBackgroundRenderer()
         : this(Vector2.Zero) { }

        internal MenuBackgroundRenderer(Vector2 initialDotOffset)
         : this (initialDotOffset, 
               new Color(235, 148, 48), 
               new Color(242, 167, 76), 
               new Color(236, 130, 47), 
               new Color(242, 153, 90)) { }

        internal MenuBackgroundRenderer(Color backgroundFromColor,
                                        Color backgroundToColor,
                                        Color dotFromColor,
                                        Color dotToColor)
         : this(Vector2.Zero, 
               backgroundFromColor, 
               backgroundToColor, 
               dotFromColor, 
               dotToColor) { }

        internal MenuBackgroundRenderer(Vector2 initialDotOffset,
                                        Color backgroundFromColor, 
                                        Color backgroundToColor,
                                        Color dotFromColor,
                                        Color dotToColor)
        {
            _offsetX = initialDotOffset.X;
            _offsetY = initialDotOffset.Y;

            _backgroundFromColor = backgroundFromColor;
            _backgroundToColor = backgroundToColor;
            _dotFromColor = dotFromColor;
            _dotToColor = dotToColor;

            _dotColorDiff = new Color(Math.Abs(_dotToColor.R - _dotFromColor.R),
                                    Math.Abs(_dotToColor.G - _dotFromColor.G),
                                    Math.Abs(_dotToColor.B - _dotFromColor.B),
                                    Math.Abs(_dotToColor.A - _dotFromColor.A));

            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
        }

        internal void Draw(SpriteBatch batch)
        {
            Draw(batch, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);
        }

        internal void Draw(SpriteBatch batch, int width, int height)
        {
            batch.Draw(CreateBackgroundTexture(width, height), GameInstance.ClientRectangle, Color.White);
        }

        internal Texture2D CreateBackgroundTexture(int width, int height)
        {
            CreateRenderDevices(width, height);

            RenderTargetManager.BeginRenderScreenToTarget(_target);
            _batch.Begin(SpriteBatchUsage.Default);

            // draws the background gradient:
            _batch.DrawGradient(new Rectangle(0, 0, width, height), _backgroundFromColor, _backgroundToColor, false, 1d);
            
            //Draw the background dots:
            for (var x = -6; x < 32; x++)
            {
                for (var y = 0; y < 21; y++)
                {
                    var posX = (int)(x * DOT_SIZE * 3 + y * DOT_SIZE + _offsetX);
                    var posY = (int)(y * DOT_SIZE * 3 - (x * DOT_SIZE) + _offsetY);

                    //We shift their color from top to bottom, so we take the different between the height of the screen and the dot's position:
                    var colorShift = (double)posY / height;
                    var cR = _dotColorDiff.R * colorShift;
                    var cG = _dotColorDiff.G * colorShift;
                    var cB = _dotColorDiff.B * colorShift;

                    double cA = 255;
                    
                    if (ApplyTransparency)
                    {
                        //When they approach the sides of the screen, make them fade out:
                        if (posX > width - 90)
                        {
                            cA -= ((posX - (width - 90)) * 3);
                            if (cA < 0)
                            {
                                cA = 0;
                            }
                        }
                        else if (posX < 90)
                        {
                            cA -= ((90 - posX) * 3);
                            if (cA < 0)
                            {
                                cA = 0;
                            }
                        }

                        //Also, make them fade out on the top of the screen, cause the orange-black contrast would be a bit jarring.
                        cA *= colorShift;
                    }

                    //When the dot is inside the rendering area, draw it.
                    if (posX + DOT_SIZE * 2 >= 0 && posX < width && posY + DOT_SIZE * 2 >= 0 && posY < height)
                    {
                        _batch.DrawCircle(new Vector2(posX, posY), DOT_SIZE * 2, new Color((int)(_dotFromColor.R + cR),
                                                                                            (int)(_dotFromColor.G + cG),
                                                                                            (int)(_dotFromColor.B + cB), 
                                                                                            (int)(cA)));
                    }
                }
            }
            _batch.End();

            RenderTargetManager.EndRenderScreenToTarget();

            return _blurHandler.BlurTexture(_target);
        }

        /// <summary>
        /// Creates the render target and blur handler in case they have not been created at all or for the desired width/height.
        /// </summary>
        private void CreateRenderDevices(int width, int height)
        {
            if (_target == null || _blurHandler == null || width != _target.Width || height != _target.Height)
            {
                _target = new RenderTarget2D(GameInstance.GraphicsDevice, width, height);
                _blurHandler = new BlurHandler(_batch, width, height);
            }
        }

        internal void Update()
        {
            //Update the dot animation:
            _offsetX -= 0.9f;
            _offsetY += 0.3f;

            //Reset it, once it went through a complete cycle:
            if (_offsetX <= (float)-DOT_SIZE * 3)
            {
                _offsetX = 0;
                _offsetY = 0;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~MenuBackgroundRenderer()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_blurHandler != null && !_blurHandler.IsDisposed) _blurHandler.Dispose();
                    if (_target != null && !_target.IsDisposed) _target.Dispose();
                    if (_batch != null && !_batch.IsDisposed) _batch.Dispose();
                }

                _blurHandler = null;
                _target = null;

                IsDisposed = true;
            }
        }
    }
}
