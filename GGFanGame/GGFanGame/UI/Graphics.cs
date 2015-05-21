using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.UI
{
    /// <summary>
    /// This class is used across the game to render basic forms like rectangles.
    /// </summary>
    public static class Graphics
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
        public static void initialize(GraphicsDevice device, SpriteBatch batch)
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
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color"></param>
        public static void drawRectangle(Rectangle rectangle, Color color)
        {
            if (_initialized)
                _spriteBatch.Draw(_canvas, rectangle, color);
        }

        /// <summary>
        /// Draws a line in a given color.
        /// </summary>
        /// <param name="start">The starting point of the line.</param>
        /// <param name="end">The ending point of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="width">The width of the line.</param>
        public static void drawLine(Vector2 start, Vector2 end, Color color, double width)
        {
            double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
            double length = Vector2.Distance(start, end);

            _spriteBatch.Draw(_canvas, start, null, color, (float)angle, Vector2.Zero, new Vector2((float)length, (float)width), SpriteEffects.None, 0);
        }

        #region Gradients

        //We'd also like to draw simple gradients here.

        private static Dictionary<string, GradientConfiguration> _gradientConfigs = new Dictionary<string, GradientConfiguration>();

        /// <summary>
        /// Used to store gradient configurations.
        /// </summary>
        private struct GradientConfiguration
        {
            //These exist mainly for performance.
            //In order to draw a gradient, we need to generate a texture that is pieced together from the shades of the gradient.
            //If we don't want to do this every frame we render the gradient, we store the generated texture in a configuration.

            //This where we store the generated texture:
            private Texture2D _texture = null;

            public GradientConfiguration(int width, int height, Color fromColor, Color toColor, bool horizontal, int steps)
            {
                _texture = generateTexture(width, height, fromColor, toColor, horizontal, steps);
            }

            /// <summary>
            /// Renders a rectangle filled with the gradient.
            /// </summary>
            /// <param name="r">The rectangle.</param>
            public void draw(Rectangle r)
            {
                _spriteBatch.Draw(_texture, r, Color.White);
            }

            private static Texture2D generateTexture(int width, int height, Color fromColor, Color toColor, bool horizontal, int steps)
            {
                int uSize = height;
                if (horizontal)
                    uSize = width;

                double diffR, diffG, diffB, diffA;

                diffR = (int)toColor.R - (int)fromColor.R;
                diffG = (int)toColor.G - (int)fromColor.G;
                diffB = (int)toColor.B - (int)fromColor.B;
                diffA = (int)toColor.A - (int)fromColor.A;

                double stepCount = steps;
                if (stepCount < 0)
                    stepCount = uSize;

                float stepSize = (float)Math.Ceiling((float)(uSize / stepCount));

                Color[] colorArr = new Color[width * height];

                int cR, cG, cB, cA;
                for (int cStep = 1; cStep <= stepCount; cStep++)
                {
                    cR = (int)(((diffR / stepCount) * cStep) + (int)fromColor.R);
                    cG = (int)(((diffG / stepCount) * cStep) + (int)fromColor.G);
                    cB = (int)(((diffB / stepCount) * cStep) + (int)fromColor.B);
                    cA = (int)(((diffA / stepCount) * cStep) + (int)fromColor.A);

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
                        Color c = new Color(cR, cG, cB, cA);

                        int length = (int)Math.Ceiling(stepSize);
                        int start = (int)((cStep - 1) * stepSize);

                        for (int x = start; x < start + length; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                int i = x + y * width;
                                colorArr[i] = c;
                            }
                        }
                    } //if
                    else
                    {
                        Color c = new Color(cR, cG, cB, cA);

                        int length = (int)Math.Ceiling(stepSize);
                        int start = (int)((cStep - 1) * stepSize);

                        for (int y = start; y < start + length; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                int i = x + y * width;
                                colorArr[i] = c;
                            }
                        }
                    } //else
                } //for

                Texture2D texture = new Texture2D(_device, width, height);
                texture.SetData(colorArr);
                return texture;
            }

            //Generates a checksum for a specific configuration.
            public static string generateChecksum(int width, int height, Color fromColor, Color toColor, bool horizontal, int steps)
            {
                return width.ToString() + "|" + height.ToString() + "|" + fromColor.ToString() + "|" + toColor.ToString() + "|" + horizontal.ToString() + "|" + steps.ToString();
            }
        }

        /// <summary>
        /// Draws a smooth gradient.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="fromColor"></param>
        /// <param name="toColor"></param>
        /// <param name="horizontal"></param>
        public static void drawGradient(Rectangle rectangle, Color fromColor, Color toColor, bool horizontal)
        {
            drawGradient(rectangle, fromColor, toColor, horizontal, -1); //negative number for steps means as many steps as possible.
        }

        /// <summary>
        /// Draws a gradient.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="fromColor"></param>
        /// <param name="toColor"></param>
        /// <param name="horizontal"></param>
        /// <param name="steps"></param>
        public static void drawGradient(Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, int steps)
        {
            if (rectangle.Width > 0 && rectangle.Height > 0)
            {
                GradientConfiguration gradient;
                string checksum = GradientConfiguration.generateChecksum(rectangle.Width, rectangle.Height, fromColor, toColor, horizontal, steps);

                if (_gradientConfigs.ContainsKey(checksum))
                {
                    gradient = _gradientConfigs[checksum];
                }
                else
                {
                    gradient = new GradientConfiguration(rectangle.Width, rectangle.Height, fromColor, toColor, horizontal, steps);
                    _gradientConfigs.Add(checksum, gradient);
                }

                //Finally, draw the configuration:
                gradient.draw(rectangle);
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
            private Texture2D _texture = null;

            public EllipseConfiguration(int width, int height)
            {
                _texture = generateTexture(width, height);
            }

            /// <summary>
            /// Renders the ellipse.
            /// </summary>
            /// <param name="position"></param>
            /// <param name="color"></param>
            public void draw(Vector2 position, Color color)
            {
                _spriteBatch.Draw(_texture, position, color);
            }

            private static Texture2D generateTexture(int width, int height)
            {
                //width and height are x and y radius.

                Color[] colorArr = new Color[width * height];

                float diamX = width / 2f;
                float diamXsq = diamX * diamX;
                float diamY = height / 2f;
                float diamYsq = diamY * diamY;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int index = x * height + y;
                        Vector2 pos = new Vector2(x - diamX, y - diamY);

                        if (pos.LengthSquared() <= diamXsq && pos.LengthSquared() <= diamYsq)
                            colorArr[index] = Color.White;
                        else
                            colorArr[index] = Color.Transparent;
                    }
                }

                Texture2D texture = new Texture2D(_device, width, height);
                texture.SetData(colorArr);
                return texture;
            }

            /// <summary>
            /// Generates a checksum for the given variables.
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public static string generateChecksum(int width, int height)
            {
                return width.ToString() + "," + height.ToString();
            }
        }

        /// <summary>
        /// Draws an ellipse with a specified color.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public static void drawEllipse(Rectangle rectangle, Color color)
        {
            EllipseConfiguration ellipse;
            string checksum = EllipseConfiguration.generateChecksum(rectangle.Width, rectangle.Height);

            if (_ellipseConfigs.ContainsKey(checksum))
            {
                ellipse = _ellipseConfigs[checksum];
            }
            else
            {
                ellipse = new EllipseConfiguration(rectangle.Width, rectangle.Height);
                _ellipseConfigs.Add(checksum, ellipse);
            }

            //Finally, draw the configuration:
            ellipse.draw(new Vector2(rectangle.X, rectangle.Y), color);
        }

        //To draw a circle, we draw an ellipse with x and y radius being the same:
        /// <summary>
        /// Draws a circle with specified radius and color.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public static void drawCircle(Vector2 position, int radius, Color color)
        {
            drawEllipse(new Rectangle((int)position.X, (int)position.Y, radius, radius), color);
        }

        #endregion

    }
}