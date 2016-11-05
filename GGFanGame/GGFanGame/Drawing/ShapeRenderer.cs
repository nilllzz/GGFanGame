using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Drawing
{
    internal static partial class SpriteBatchExtensions
    {
        private static readonly ShapeRenderer _renderer = new ShapeRenderer();

        // TODO: Do something about this method, doesn't work here at all.
        internal static Texture2D CreateJoinedEllipse(int outerWidth, int outerHeight, Rectangle[] ellipses, Color[] colors)
            => ShapeRenderer.CreateJoinedEllipse(outerWidth, outerHeight, ellipses, colors);

        /// <summary>
        /// Used to render shapes like rectangles and ellipses.
        /// </summary>
        private sealed class ShapeRenderer
        {
            private readonly Texture2D _pixel;

            internal ShapeRenderer()
            {
                // We create a 1,1 pixel large texture here, with a single white pixel.
                // That texture will get stretched and colored when using it to draw a rectangle.
                _pixel = new Texture2D(GameInstance.GraphicsDevice, 1, 1);
                _pixel.SetData(new Color[] { Color.White });
            }

            /// <summary>
            /// Draws a rectangle in a given color.
            /// </summary>
            /// <param name="rectangle">The rectangle to draw.</param>
            /// <param name="color">The color of the rectangle.</param>
            internal void DrawRectangle(SpriteBatch batch, Rectangle rectangle, Color color)
            {
                batch.Draw(_pixel, rectangle, color);
            }

            /// <summary>
            /// Draws a line in a given color.
            /// </summary>
            /// <param name="start">The starting point of the line.</param>
            /// <param name="end">The ending point of the line.</param>
            /// <param name="color">The color of the line.</param>
            /// <param name="width">The width of the line.</param>
            internal void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color, double width)
            {
                var angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
                double length = Vector2.Distance(start, end);

                batch.Draw(_pixel, start, null, color, (float)angle, Vector2.Zero, new Vector2((float)length, (float)width), SpriteEffects.None, 0);
            }
            
            private readonly Dictionary<string, GradientConfiguration> _gradientConfigs = new Dictionary<string, GradientConfiguration>();
            
            /// <summary>
            /// Draws a gradient.
            /// </summary>
            internal void DrawGradient(SpriteBatch batch, Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, double scale = 1D, int steps = -1)
            {
                if (rectangle.Width > 0 && rectangle.Height > 0)
                {
                    GradientConfiguration gradient;
                    var checksum = GradientConfiguration.GenerateChecksum(rectangle.Width, rectangle.Height, fromColor, toColor, horizontal, steps);

                    if (_gradientConfigs.ContainsKey(checksum))
                    {
                        gradient = _gradientConfigs[checksum];
                    }
                    else
                    {
                        gradient = new GradientConfiguration((int)(rectangle.Width / scale), (int)(rectangle.Height / scale), fromColor, toColor, horizontal, steps);
                        _gradientConfigs.Add(checksum, gradient);
                    }

                    // Finally, draw the configuration:
                    gradient.Draw(batch, rectangle);
                }
            }


            // To draw circles, we generate an ellipse texture given a radius for x and y and store those.
            // Once we want to draw an ellipse with that specific radius, we grab the corresponding texture and render it.

            private Dictionary<string, EllipseConfiguration> _ellipseConfigs = new Dictionary<string, EllipseConfiguration>();

            /// <summary>
            /// Draws an ellipse with a specified color.
            /// </summary>
            internal void DrawEllipse(SpriteBatch batch, Rectangle rectangle, Color color, double scale = 1D)
            {
                EllipseConfiguration ellipse;
                var checksum = EllipseConfiguration.GenerateChecksum(rectangle.Width, rectangle.Height);

                if (_ellipseConfigs.ContainsKey(checksum))
                {
                    ellipse = _ellipseConfigs[checksum];
                }
                else
                {
                    ellipse = new EllipseConfiguration((int)Math.Ceiling(rectangle.Width / scale), (int)Math.Ceiling(rectangle.Height / scale));
                    _ellipseConfigs.Add(checksum, ellipse);
                }

                // Finally, draw the configuration:
                ellipse.Draw(batch, rectangle, color);
            }
            
            // To draw a circle, we draw an ellipse with x and y radius being the same:
            /// <summary>
            /// Draws a circle with specified radius and color.
            /// </summary>
            internal void DrawCircle(SpriteBatch batch, Vector2 position, int radius, Color color, double scale = 1D)
            {
                DrawEllipse(batch, new Rectangle((int)position.X, (int)position.Y, radius, radius), color, scale);
            }

            /// <summary>
            /// Creates a joined texture from multiple ellipses.
            /// </summary>
            /// <param name="outerWidth">The full width of the joined texture.</param>
            /// <param name="outerHeight">The full height of the joined texture.</param>
            /// <param name="ellipses">Rectangles enclosing the ellipses.</param>
            /// <param name="colors">The colors of the ellipses.</param>
            internal static Texture2D CreateJoinedEllipse(int outerWidth, int outerHeight, Rectangle[] ellipses, Color[] colors)
            {
                //The objects at the same index in the ellipses and colors arrays are corresponding.

                var colorArr = new Color[outerWidth * outerHeight];
                var returnTexture = new Texture2D(GameInstance.GraphicsDevice, outerWidth, outerHeight);

                //By default, the return texture is entirely transparent:
                for (var i = 0; i < colorArr.Length; i++)
                {
                    colorArr[i] = Color.Transparent;
                }

                //Loop through the ellipses and fill the color array with the ellipse colors:
                for (var i = 0; i < ellipses.Length; i++)
                {
                    var ellipse = ellipses[i];
                    var color = colors[i];

                    var ellipseTextureData = EllipseConfiguration.GenerateTextureData(ellipse.Width, ellipse.Height);

                    for (var x = 0; x < ellipse.Width; x++)
                    {
                        for (var y = 0; y < ellipse.Height; y++)
                        {
                            var index = y * ellipse.Width + x;
                            var colIndex = (y + ellipse.Y) * outerWidth + (x + ellipse.X);

                            //Only fill in when the ellipse's color is not transparent:
                            if (ellipseTextureData[index] != Color.Transparent)
                            {
                                colorArr[colIndex] = color;
                            }
                        }
                    }
                }

                returnTexture.SetData(colorArr);

                return returnTexture;
            }
        }
    }
}