using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Drawing
{
    /// <summary>
    /// This class is used across the game to render basic forms like rectangles.
    /// </summary>
    static class Graphics
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
        public static void drawRectangle(Rectangle rectangle, Color color)
        {
            drawRectangle(_spriteBatch, rectangle, color);
        }

        /// <summary>
        /// Draws a rectangle in a given color.
        /// </summary>
        public static void drawRectangle(SpriteBatch batch, Rectangle rectangle, Color color)
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
        public static void drawLine(Vector2 start, Vector2 end, Color color, double width)
        {
            drawLine(_spriteBatch, start, end, color, width);
        }

        /// <summary>
        /// Draws a line in a given color.
        /// </summary>
        /// <param name="start">The starting point of the line.</param>
        /// <param name="end">The ending point of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="width">The width of the line.</param>
        public static void drawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color, double width)
        {
            if (_initialized)
            {
                double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
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
        public static void drawBorder(int thickness, Rectangle rectangle, Color color)
        {
            drawRectangle(new Rectangle(rectangle.X + thickness, rectangle.Y, rectangle.Width - thickness, thickness), color);
            drawRectangle(new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y + thickness, thickness, rectangle.Height - thickness), color);
            drawRectangle(new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width - thickness, thickness), color);
            drawRectangle(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height - thickness), color);
        }


        #region Shadows

        private static Texture2D generateTexture(bool[] spriteTransparency, Point spriteOffset, Rectangle boundingRect, Rectangle projShadowRect, 
                                            Rectangle topShadowRect, Rectangle groundShadowRect)
        {
            Color[] colorArr = new Color[boundingRect.Width * boundingRect.Height];

            // Fill color array
            ellipseTex(spriteTransparency, spriteOffset, colorArr, topShadowRect, topShadowRect.Y - boundingRect.Y); // TODO: Can optimize - don't have to draw bottom half of ellipse
            ellipseTex(spriteTransparency, spriteOffset, colorArr, groundShadowRect, groundShadowRect.Y - boundingRect.Y);  // TODO: Can optimize - don't have to draw top half of ellipse
            rectTex(spriteTransparency, spriteOffset, colorArr, projShadowRect, projShadowRect.Y - boundingRect.Y);

            // Create texture
            Texture2D texture = new Texture2D(_device, boundingRect.Width, boundingRect.Height);
            texture.SetData(colorArr);
            return texture;
        }

        /// <summary>
        /// Fills the points in the color array with an ellipse inside the given rectangle
        /// </summary>
        private static Color[] ellipseTex(bool[] spriteTransparency, Point spriteOffset, Color[] colorArr, Rectangle rectangle, int yLoc)
        {
            Point center = new Point(rectangle.Width / 2, rectangle.Height / 2);
            double xRadius = rectangle.Width / 2d;
            double yRadius = rectangle.Height / 2d;

            for (int x = 0; x < rectangle.Width; x++)
            {
                for (int y = yLoc; y < rectangle.Height + yLoc; y++)
                {
                    int index = y * rectangle.Width + x;

                    // TODO: Need to implement spriteTransparency if statement

                    //if (x < spriteOffset.X)

                    //if (inBounds(x, y, spriteTransparency.Length, spriteOffset))
                    //{
                        Point normalized = new Point(x - center.X, y - yLoc - center.Y);

                        if (((normalized.X * normalized.X) / (xRadius * xRadius)) + ((normalized.Y * normalized.Y) / (yRadius * yRadius)) <= 1.0)
                            colorArr[index] = Color.White;
                        else
                            colorArr[index] = Color.Transparent;
                    /*}
                    else
                    {
                        colorArr[index] = Color.Transparent;
                    }*/
                }
            }

            return colorArr;
        }

        /// <summary>
        /// Fills the points in the color array with an rectangle inside the given rectangle
        /// </summary>
        private static Color[] rectTex(bool[] spriteTransparency, Point spriteOffset, Color[] colorArr, Rectangle rectangle, int yLoc)
        {
            for (int x = 0; x < rectangle.Width; x++)
            {
                for (int y = yLoc; y < rectangle.Height + yLoc; y++)
                {
                    int index = y * rectangle.Width + x;

                    // TODO: Need to implement spriteTransparency if statement

                    colorArr[index] = Color.White;
                }
            }
            return colorArr;
        }

        /// <summary>
        /// Draws a shadow on a supporting object, fitting it's sprite shape.
        /// </summary>
        public static void drawSupportingShadow(Game.Level.StageObject supportingObject, Texture2D spriteSheet, Rectangle frame,
                                            float X, float Z, double shadowWidth, double shadowHeight, float groundPosition, double scale)
        {
            // Create top and bottom elliptical shadow bounds
            Rectangle topShadowRect = createTopShadowRectangle(X, Z, shadowWidth, shadowHeight, groundPosition, 1d);
            Rectangle groundShadowRect = createGroundShadowRectangle(X, Z, shadowWidth, shadowHeight, 1d);    
            
            // Create projection rectangle bounds
            Rectangle projShadowRect = new Rectangle((int)((X - shadowWidth / 2d)),
                                           topShadowRect.Y + (int)(shadowHeight / 2d),
                                           (int)(shadowWidth),
                                           groundShadowRect.Y - topShadowRect.Y);

            // Rectangle bounds of the entire texture
            Rectangle boundingRect = new Rectangle(projShadowRect.X, projShadowRect.Y - (int)(topShadowRect.Height / 2d), projShadowRect.Width, projShadowRect.Height + topShadowRect.Height);


            // Get spriteSheet transparency color data for just that frame. This will be used to check transparency when creating the final texture.
            bool[] spriteTransparency;

            if (spriteSheet != null)           
                spriteTransparency = getSpriteColorData(spriteSheet, frame);
            else
                spriteTransparency = new bool[frame.Width * frame.Height]; // TODO: These need to be all true or false depending on the _objectColor

            // Create sprite frame offset with shadow
            Point spriteOffset = new Point((int)(supportingObject.X - boundingRect.X), (int)(Z - supportingObject.Z)); // TODO: This is wrong

            Console.WriteLine("spriteOffset: (" + spriteOffset.X + ", " + spriteOffset.Y + ")");
            //Console.WriteLine("boundingRect.X = " + boundingRect.X + ", supportingObject.X = " + supportingObject.X +
            //    ", boundingRect.Y = " + boundingRect.Y + ", supportingObject.Y = " + supportingObject.Y +
            //    ", supportingObject.Z = " + supportingObject.Z + ", Z = " + Z);


            // Create texture
            Texture2D texture = generateTexture(spriteTransparency, spriteOffset, boundingRect, projShadowRect, topShadowRect, groundShadowRect);


            // Create final bounding rectangle with scale
            Rectangle rect = new Rectangle((int)((X - shadowWidth / 2d) * scale), (int)(boundingRect.Y * scale),
                                           (int)(boundingRect.Width * scale), (int)(boundingRect.Height * scale));

            // Finally, draw:
            if (_initialized)
                _spriteBatch.Draw(texture, rect, Game.Level.Stage.activeStage().ambientColor);


            // TODO: Draw ground shadow inverted alpha masked with the spriteSheet


        }


        // TODO: Create a dictionary and transparency configuration so you don't have to generate this everytime

        /// <summary>
        /// Get spriteSheet frame's transparency data.
        /// </summary>
        private static bool[] getSpriteColorData(Texture2D spriteSheet, Rectangle frame)
        {
            // Get entire spriteSheet color data (can optimize for just frame?):      
            int width = spriteSheet.Width;
            int height = spriteSheet.Height;
            Color[] spriteData = new Color[width * height];
            spriteSheet.GetData<Color>(spriteData, 0, spriteData.Length);

            // Create frame sprite transprency array:
            bool[] spriteTransparency = new bool[frame.Width * frame.Height];
            
            //Console.WriteLine("TRANSPARENCY DATA");
            for (int x = 0; x < frame.Width; x++)
            {
                for (int y = 0; y < frame.Height; y++)
                {
                    int dataIndex = (y + frame.Y) * spriteSheet.Width + x + frame.X;
                    int transparencyIndex = y * frame.Width + x;

                    if (spriteData[dataIndex].Equals(Color.Transparent))
                        spriteTransparency[transparencyIndex] = false;
                    else
                        spriteTransparency[transparencyIndex] = true;

                    
                    // Debugging (THIS WILL BE SIDEWAYS)
                    //if (spriteTransparency[transparencyIndex]) Console.Write("X");
                    //else Console.Write(" ");
                }
                //Console.WriteLine();
            }


            return spriteTransparency;
        }

        //private static bool inBounds(int index, int arrayLength, Point offset)
        //{
        //    return true;
        //}

        /// <summary>
        /// Draws a simple ground shadow.
        /// </summary>
        public static void drawGroundShadow(float X, float Z, double shadowWidth, double shadowHeight, float groundPosition, double scale)
        {
            drawEllipse(createGroundShadowRectangle(X, Z, shadowWidth, shadowHeight, scale),
                            Game.Level.Stage.activeStage().ambientColor, scale); //TODO: maybe, we have the shadow fade away when the player jumps?
        }

        /// <summary>
        /// Creates the rectangle bounds for a shadow on top of an object.
        /// </summary>
        private static Rectangle createTopShadowRectangle(float X, float Z, double shadowWidth, double shadowHeight, float groundPosition, double scale)
        {
            return new Rectangle((int)((X - shadowWidth / 2d) * scale),
                                 (int)((Z - shadowHeight / 2d - groundPosition) * scale),
                                 (int)(shadowWidth * scale),
                                 (int)(shadowHeight * scale));
        }

        /// <summary>
        /// Creates the rectangle bounds for a shadow on the ground.
        /// </summary>
        private static Rectangle createGroundShadowRectangle(float X, float Z, double shadowWidth, double shadowHeight, double scale)
        {
            return new Rectangle((int)((X - shadowWidth / 2d) * scale),
                                 (int)((Z - shadowHeight / 2d) * scale),
                                 (int)(shadowWidth * scale),
                                 (int)(shadowHeight * scale));
        }

        #endregion


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
            private Texture2D _texture;

            public GradientConfiguration(int width, int height, Color fromColor, Color toColor, bool horizontal, int steps)
            {
                _texture = generateTexture(width, height, fromColor, toColor, horizontal, steps);
            }

            /// <summary>
            /// Renders a rectangle filled with the gradient.
            /// </summary>
            /// <param name="r">The rectangle.</param>
            public void draw(SpriteBatch batch, Rectangle r)
            {
                batch.Draw(_texture, r, Color.White);
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
        public static void drawGradient(Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, double scale)
        {
            drawGradient(_spriteBatch, rectangle, fromColor, toColor, horizontal, -1, scale); //negative number for steps means as many steps as possible.
        }

        /// <summary>
        /// Draws a smooth gradient.
        /// </summary>
        public static void drawGradient(SpriteBatch batch, Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, double scale)
        {
            drawGradient(batch, rectangle, fromColor, toColor, horizontal, -1, scale); //negative number for steps means as many steps as possible.
        }

        /// <summary>
        /// Draws a gradient.
        /// </summary>
        public static void drawGradient(Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, int steps, double scale)
        {
            drawGradient(_spriteBatch, rectangle, fromColor, toColor, horizontal, steps, scale);
        }

        /// <summary>
        /// Draws a gradient.
        /// </summary>
        public static void drawGradient(SpriteBatch batch, Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, int steps, double scale)
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
                    gradient = new GradientConfiguration((int)(rectangle.Width / scale), (int)(rectangle.Height / scale), fromColor, toColor, horizontal, steps);
                    _gradientConfigs.Add(checksum, gradient);
                }

                //Finally, draw the configuration:
                gradient.draw(batch, rectangle);
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
            private Texture2D _texture;

            public EllipseConfiguration(int width, int height)
            {
                _texture = generateTexture(width, height);
            }

            /// <summary>
            /// Renders the ellipse.
            /// </summary>
            /// <param name="position"></param>
            /// <param name="color"></param>
            public void draw(SpriteBatch batch, Rectangle rectangle, Color color)
            {
                batch.Draw(_texture, rectangle, color);
            }

            private static Texture2D generateTexture(int width, int height)
            {
                //width and height are x and y diameter.

                Color[] colorArr = new Color[width * height];

                Point center = new Point(width / 2, height / 2);

                double xRadius = width / 2d;
                double yRadius = height / 2d;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int index = y * width + x;

                        Point normalized = new Point(x - center.X, y - center.Y);

                        if (((normalized.X * normalized.X) / (xRadius * xRadius)) + ((normalized.Y * normalized.Y) / (yRadius * yRadius)) <= 1.0)
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
        public static void drawEllipse(SpriteBatch batch, Rectangle rectangle, Color color, double scale)
        {
            EllipseConfiguration ellipse;
            string checksum = EllipseConfiguration.generateChecksum(rectangle.Width, rectangle.Height);

            if (_ellipseConfigs.ContainsKey(checksum))
            {
                ellipse = _ellipseConfigs[checksum];
            }
            else
            {
                ellipse = new EllipseConfiguration((int)(rectangle.Width / scale), (int)(rectangle.Height / scale));
                _ellipseConfigs.Add(checksum, ellipse);
            }

            //Finally, draw the configuration:
            ellipse.draw(batch, rectangle, color);
        }

        /// <summary>
        /// Draws an ellipse with a specified color - Original call.
        /// </summary>
        public static void drawEllipse(Rectangle rectangle, Color color)
        {
            drawEllipse(_spriteBatch, rectangle, color, 1d);
        }
        
        /// <summary>
        /// Draws an ellipse with a specified color - Original call overloaded with scale.
        /// </summary>
        public static void drawEllipse(Rectangle rectangle, Color color, double scale)
        {
            drawEllipse(_spriteBatch, rectangle, color, scale);
        }

        //To draw a circle, we draw an ellipse with x and y radius being the same:
        /// <summary>
        /// Draws a circle with specified radius and color.
        /// </summary>
        public static void drawCircle(SpriteBatch batch, Vector2 position, int radius, Color color, double scale)
        {
            drawEllipse(batch, new Rectangle((int)position.X, (int)position.Y, radius, radius), color, scale);
        }

        /// <summary>
        /// Draws a circle with specified radius and color - Original call.
        /// </summary>
        public static void drawCircle(Vector2 position, int radius, Color color)
        {
            drawCircle(_spriteBatch, position, radius, color, 1d);
        }
        
        /// <summary>
        /// Draws a circle with specified radius and color - Original call overloaded with scale.
        /// </summary>
        public static void drawCircle(Vector2 position, int radius, Color color, double scale)
        {
            drawCircle(_spriteBatch, position, radius, color, scale);
        }

        #endregion

    }
}