using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGFanGame.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Screens.Menu
{
    internal class MenuBackgroundRenderer : IDisposable
    {
        //The offset of the dots:
        private float _offsetX = 0f, 
                      _offsetY = 0f;

        //The size reference of the dots in the background in pixels
        private const int DOT_SIZE = 16;

        private readonly Color _backgroundFromColor,
                               _backgroundToColor,
                               _dotFromColor,
                               _dotToColor;

        private BlurHandler _blurHandler;
        private RenderTarget2D _target;

        internal bool applyTransparency { get; set; }

        internal bool isDisposed { get; private set; }
        
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

            _blurHandler = new BlurHandler(GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);
            _target = new RenderTarget2D(gameInstance.GraphicsDevice, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);
        }

        internal void draw()
        {
            gameInstance.spriteBatch.Draw(createBackgroundTexture(), gameInstance.clientRectangle, Color.White);
        }

        internal Texture2D createBackgroundTexture()
        {
            RenderTargetManager.beginRenderScreenToTarget(_target);
            
            // draws the background gradient:
            Graphics.drawGradient(gameInstance.clientRectangle, _backgroundFromColor, _backgroundToColor, false, 1d);
            
            Color dotColorDiff = new Color(_dotToColor.R - _dotFromColor.R,
                                           _dotToColor.G - _dotFromColor.G,
                                           _dotToColor.B - _dotFromColor.B);

            //Draw the background dots:
            for (int x = -6; x < 32; x++)
            {
                for (int y = 0; y < 21; y++)
                {
                    int posX = (int)(x * DOT_SIZE * 3 + y * DOT_SIZE + _offsetX);
                    int posY = (int)(y * DOT_SIZE * 3 - (x * DOT_SIZE) + _offsetY);

                    //We shift their color from top to bottom, so we take the different between the height of the screen and the dot's position:
                    double colorShift = (double)posY / gameInstance.clientRectangle.Height;
                    double cR = ((double)dotColorDiff.R) * colorShift;
                    double cG = ((double)dotColorDiff.G) * colorShift;
                    double cB = ((double)dotColorDiff.B) * colorShift;

                    double cA = 255; //alpha value
                    if (applyTransparency)
                    {
                        //When they approach the sides of the screen, make them fade out:
                        if (posX > gameInstance.clientRectangle.Width - 90)
                        {
                            cA = 255 - ((posX - (gameInstance.clientRectangle.Width - 90)) * 3);
                            if (cA < 0)
                            {
                                cA = 0;
                            }
                        }
                        else if (posX < 90)
                        {
                            cA = 255 - ((90 - posX) * 3);
                            if (cA < 0)
                            {
                                cA = 0;
                            }
                        }

                        //Also, make them fade out on the top of the screen, cause the orange-black contrast would be a bit jarring.
                        cA *= colorShift;
                    }

                    //When the dot is inside the rendering area, draw it.
                    if (posX + DOT_SIZE * 2 >= 0 && posX < gameInstance.clientRectangle.Width && posY + DOT_SIZE * 2 >= 0 && posY < gameInstance.clientRectangle.Height)
                    {
                        Graphics.drawCircle(new Vector2(posX, posY), DOT_SIZE * 2, new Color((int)(_dotFromColor.R + cR),
                                                                                             (int)(_dotFromColor.G + cG),
                                                                                             (int)(_dotFromColor.B + cB), (int)cA));
                    }
                }
            }
            RenderTargetManager.endRenderScreenToTarget();

            return _blurHandler.blurTexture(_target);
        }

        internal void update()
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
            dispose(true);
        }

        ~MenuBackgroundRenderer()
        {
            dispose(false);
        }

        private void dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (_blurHandler != null && !_blurHandler.isDisposed) _blurHandler.Dispose();
                    if (_target != null && !_target.IsDisposed) _target.Dispose();
                }

                _blurHandler = null;
                _target = null;

                isDisposed = true;
            }
        }
    }
}
