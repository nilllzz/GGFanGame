using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Game
{
    internal class SpriteSheet
    {
        private struct SpriteSetting
        {
            public Rectangle Rectangle;
            public bool Flipped;
        }

        private readonly Texture2D _texture;
        private readonly Dictionary<SpriteSetting, Texture2D> _parts = new Dictionary<SpriteSetting, Texture2D>();

        internal SpriteSheet(Texture2D texture)
        {
            _texture = texture;
        }

        internal Texture2D GetPart(Rectangle rectangle, bool flipped = false)
        {
            var settings = new SpriteSetting
            {
                Flipped = flipped,
                Rectangle = rectangle
            };

            if (rectangle == _texture.Bounds && !flipped)
            {
                return _texture;
            }
            else
            {
                if (!_parts.TryGetValue(settings, out Texture2D part))
                {
                    var data = new Color[rectangle.Width * rectangle.Height];
                    _texture.GetData(0, rectangle, data, 0, data.Length);

                    if (flipped)
                        data = FlipTextureData(rectangle.Width, rectangle.Height, data);

                    part = new Texture2D(GameInstance.GraphicsDevice, rectangle.Width, rectangle.Height);
                    part.SetData(data);
                    _parts.Add(settings, part);
                }

                return part;
            }
        }

        private Color[] FlipTextureData(int width, int height, Color[] data)
        {
            Color[] flippedData = new Color[data.Length];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int idx = width - 1 - x + y * width;
                    flippedData[x + y * width] = data[idx];
                }
            }

            return flippedData;
        }
    }
}
