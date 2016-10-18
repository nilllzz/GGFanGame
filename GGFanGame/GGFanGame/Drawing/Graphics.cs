using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Drawing
{
    /// <summary>
    /// This class is used across the game to render basic forms like rectangles.
    /// </summary>
    internal static class Graphics
    {
        //Stores a single pixel to draw forms with:
        private static Texture2D _canvas = null;

        //Keep pointers to these here.
        private static SpriteBatch _spriteBatch = null;
        private static GraphicsDevice _device = null;

        private static bool _initialized = false;

        /// <summary>
        /// Initializes the Graphics class so it can be used to draw.
        /// </summary>
        /// <param name="device">The graphics device of the game.</param>
        public static void Initialize(GraphicsDevice device, SpriteBatch batch)
        {
            if (!_initialized)
            {
                //We create a 1,1 pixel large texture here, with a single white pixel.
                //That texture will get stretched and colored when using it to draw a rectangle.
                _canvas = new Texture2D(device, 1, 1);
                _canvas.SetData(new Color[] { Color.White });

                _spriteBatch = batch;
                _device = device;

                _initialized = true;
            }
        }

        /// <summary>
        /// Draws a rectangle in a given color.
        /// </summary>
        public static void DrawRectangle(Rectangle rectangle, Color color)
        {
            DrawRectangle(_spriteBatch, rectangle, color);
        }

        /// <summary>
        /// Draws a rectangle in a given color.
        /// </summary>
        public static void DrawRectangle(SpriteBatch batch, Rectangle rectangle, Color color)
        {
            if (_initialized)
                batch.Draw(_canvas, rectangle, color);
        }

        /// <summary>
        /// Draws a line in a given color.
        /// </summary>
        /// <param name="start">The starting point of the line.</param>
        /// <param name="end">The ending point of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="width">The width of the line.</param>
        public static void DrawLine(Vector2 start, Vector2 end, Color color, double width)
        {
            DrawLine(_spriteBatch, start, end, color, width);
        }

        /// <summary>
        /// Draws a line in a given color.
        /// </summary>
        /// <param name="start">The starting point of the line.</param>
        /// <param name="end">The ending point of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="width">The width of the line.</param>
        public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color, double width)
        {
            if (_initialized)
            {
                var angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
                double length = Vector2.Distance(start, end);

                batch.Draw(_canvas, start, null, color, (float)angle, Vector2.Zero, new Vector2((float)length, (float)width), SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Draws a border around a rectangle.
        /// </summary>
        /// <param name="thickness">The border thickness.</param>
        /// <param name="rectangle">The rectangle to draw around.</param>
        /// <param name="color">The color of the border.</param>
        public static void DrawBorder(int thickness, Rectangle rectangle, Color color)
        {
            DrawRectangle(new Rectangle(rectangle.X + thickness, rectangle.Y, rectangle.Width - thickness, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y + thickness, thickness, rectangle.Height - thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width - thickness, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height - thickness), color);
        }

        #region Gradients

        //We'd also like to draw simple gradients here.

        private static readonly Dictionary<string, GradientConfiguration> _gradientConfigs = new Dictionary<string, GradientConfiguration>();

        /// <summary>
        /// Used to store gradient configurations.
        /// </summary>
        private struct GradientConfiguration
        {
            //These exist mainly for performance.
            //In order to draw a gradient, we need to generate a texture that is pieced together from the shades of the gradient.
            //If we don't want to do this every frame we render the gradient, we store the generated texture in a configuration.

            //This where we store the generated texture:
            private readonly Texture2D _texture;

            public GradientConfiguration(int width, int height, Color fromColor, Color toColor, bool horizontal, int steps)
            {
                _texture = GenerateTexture(width, height, fromColor, toColor, horizontal, steps);
            }

            /// <summary>
            /// Renders a rectangle filled with the gradient.
            /// </summary>
            /// <param name="r">The rectangle.</param>
            public void Draw(SpriteBatch batch, Rectangle r)
            {
                batch.Draw(_texture, r, Color.White);
            }

            private static Texture2D GenerateTexture(int width, int height, Color fromColor, Color toColor, bool horizontal, int steps)
            {
                var uSize = height;
                if (horizontal)
                    uSize = width;

                double diffR = (int)toColor.R - (int)fromColor.R;
                double diffG = (int)toColor.G - (int)fromColor.G;
                double diffB = (int)toColor.B - (int)fromColor.B;
                double diffA = (int)toColor.A - (int)fromColor.A;

                double stepCount = steps;
                if (stepCount < 0)
                    stepCount = uSize;

                var stepSize = (float)Math.Ceiling((float)(uSize / stepCount));

                var colorArr = new Color[width * height];

                for (var cStep = 1; cStep <= stepCount; cStep++)
                {
                    var cR = (int)(((diffR / stepCount) * cStep) + (int)fromColor.R);
                    var cG = (int)(((diffG / stepCount) * cStep) + (int)fromColor.G);
                    var cB = (int)(((diffB / stepCount) * cStep) + (int)fromColor.B);
                    var cA = (int)(((diffA / stepCount) * cStep) + (int)fromColor.A);

                    if (cR < 0)
                        cR += 255;
                    if (cG < 0)
                        cG += 255;
                    if (cB < 0)
                        cB += 255;
                    if (cA < 0)
                        cA += 255;

                    if (horizontal)
                    {
                        var c = new Color(cR, cG, cB, cA);

                        var length = (int)Math.Ceiling(stepSize);
                        var start = (int)((cStep - 1) * stepSize);

                        for (var x = start; x < start + length; x++)
                        {
                            for (var y = 0; y < height; y++)
                            {
                                var i = x + y * width;
                                colorArr[i] = c;
                            }
                        }
                    } //if
                    else
                    {
                        var c = new Color(cR, cG, cB, cA);

                        var length = (int)Math.Ceiling(stepSize);
                        var start = (int)((cStep - 1) * stepSize);

                        for (var y = start; y < start + length; y++)
                        {
                            for (var x = 0; x < width; x++)
                            {
                                var i = x + y * width;
                                colorArr[i] = c;
                            }
                        }
                    } //else
                } //for

                var texture = new Texture2D(_device, width, height);
                texture.SetData(colorArr);
                return texture;
            }

            //Generates a checksum for a specific configuration.
            public static string GenerateChecksum(int width, int height, Color fromColor, Color toColor, bool horizontal, int steps)
            {
                return width.ToString() + "|" + height.ToString() + "|" + fromColor.ToString() + "|" + toColor.ToString() + "|" + horizontal.ToString() + "|" + steps.ToString();
            }
        }

        /// <summary>
        /// Draws a smooth gradient.
        /// </summary>
        public static void DrawGradient(Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, double scale)
        {
            DrawGradient(_spriteBatch, rectangle, fromColor, toColor, horizontal, -1, scale); //negative number for steps means as many steps as possible.
        }

        /// <summary>
        /// Draws a smooth gradient.
        /// </summary>
        public static void DrawGradient(SpriteBatch batch, Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, double scale)
        {
            DrawGradient(batch, rectangle, fromColor, toColor, horizontal, -1, scale); //negative number for steps means as many steps as possible.
        }

        /// <summary>
        /// Draws a gradient.
        /// </summary>
        public static void DrawGradient(Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, int steps, double scale)
        {
            DrawGradient(_spriteBatch, rectangle, fromColor, toColor, horizontal, steps, scale);
        }

        /// <summary>
        /// Draws a gradient.
        /// </summary>
        public static void DrawGradient(SpriteBatch batch, Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, int steps, double scale)
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

                //Finally, draw the configuration:
                gradient.Draw(batch, rectangle);
            }
        }

        #endregion

        #region Ellipses

        //To draw circles, we generate an ellipse texture given a radius for x and y and store those.
        //Once we want to draw an ellipse with that specific radius, we grab the corresponding texture and render it.

        private static Dictionary<string, EllipseConfiguration> _ellipseConfigs = new Dictionary<string, EllipseConfiguration>();

        /// <summary>
        /// Used to store ellipse configurations.
        /// </summary>
        private struct EllipseConfiguration
        {
            private readonly Texture2D _texture;

            public EllipseConfiguration(int width, int height)
            {
                var texture = new Texture2D(_device, width, height);
                texture.SetData(GenerateTextureData(width, height));
                _texture = texture;
            }

            /// <summary>
            /// Renders the ellipse.
            /// </summary>
            public void Draw(SpriteBatch batch, Rectangle rectangle, Color color)
            {
                batch.Draw(_texture, rectangle, color);
            }

            public static Color[] GenerateTextureData(int width, int height)
            {
                //width and height are x and y diameter.

                var colorArr = new Color[width * height];

                var center = new Point(width / 2, height / 2);

                var xRadius = width / 2d;
                var yRadius = height / 2d;

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var index = y * width + x;

                        var normalized = new Point(x - center.X, y - center.Y);

                        if (((normalized.X * normalized.X) / (xRadius * xRadius)) + ((normalized.Y * normalized.Y) / (yRadius * yRadius)) <= 1.0)
                            colorArr[index] = Color.White;
                        else
                            colorArr[index] = Color.Transparent;
                    }
                }

                return colorArr;
            }

            /// <summary>
            /// Generates a checksum for the given variables.
            /// </summary>
            public static string GenerateChecksum(int width, int height)
            {
                return width.ToString() + "," + height.ToString();
            }
        }

        /// <summary>
        /// Draws an ellipse with a specified color.
        /// </summary>
        public static void DrawEllipse(SpriteBatch batch, Rectangle rectangle, Color color, double scale)
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

            //Finally, draw the configuration:
            ellipse.Draw(batch, rectangle, color);
        }

        /// <summary>
        /// Draws an ellipse with a specified color - Original call.
        /// </summary>
        public static void DrawEllipse(Rectangle rectangle, Color color)
        {
            DrawEllipse(_spriteBatch, rectangle, color, 1d);
        }

        /// <summary>
        /// Draws an ellipse with a specified color - Original call overloaded with scale.
        /// </summary>
        public static void DrawEllipse(Rectangle rectangle, Color color, double scale)
        {
            DrawEllipse(_spriteBatch, rectangle, color, scale);
        }

        //To draw a circle, we draw an ellipse with x and y radius being the same:
        /// <summary>
        /// Draws a circle with specified radius and color.
        /// </summary>
        public static void DrawCircle(SpriteBatch batch, Vector2 position, int radius, Color color, double scale)
        {
            DrawEllipse(batch, new Rectangle((int)position.X, (int)position.Y, radius, radius), color, scale);
        }

        /// <summary>
        /// Draws a circle with specified radius and color - Original call.
        /// </summary>
        public static void DrawCircle(Vector2 position, int radius, Color color)
        {
            DrawCircle(_spriteBatch, position, radius, color, 1d);
        }

        /// <summary>
        /// Draws a circle with specified radius and color - Original call overloaded with scale.
        /// </summary>
        public static void DrawCircle(Vector2 position, int radius, Color color, double scale)
        {
            DrawCircle(_spriteBatch, position, radius, color, scale);
        }

        #endregion

        #region JoinedShapes

        /// <summary>
        /// Creates a joined texture from multiple ellipses.
        /// </summary>
        /// <param name="outerWidth">The full width of the joined texture.</param>
        /// <param name="outerHeight">The full height of the joined texture.</param>
        /// <param name="ellipses">Rectangles enclosing the ellipses.</param>
        /// <param name="colors">The colors of the ellipses.</param>
        public static Texture2D CreateJoinedEllipse(int outerWidth, int outerHeight, Rectangle[] ellipses, Color[] colors)
        {
            //The objects at the same index in the ellipses and colors arrays are corresponding.

            var colorArr = new Color[outerWidth * outerHeight];
            var returnTexture = new Texture2D(_device, outerWidth, outerHeight);

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

        #endregion

    }
}