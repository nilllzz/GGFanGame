using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Drawing
{
    internal static partial class SpriteBatchExtensions
    {

        /// <summary>
        /// Used to store ellipse configurations.
        /// </summary>
        private struct EllipseConfiguration
        {
            private readonly Texture2D _texture;

            public EllipseConfiguration(int width, int height)
            {
                var texture = new Texture2D(GameInstance.GraphicsDevice, width, height);
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
                // width and height are x and y diameter.

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
    }
}
