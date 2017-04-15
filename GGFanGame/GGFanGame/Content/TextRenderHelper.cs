using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace GGFanGame.Content
{
    internal static class TextRenderHelper
    {
        private static readonly Color _white = Color.White;
        private static readonly Color _blue = new Color(122, 141, 235);

        internal static void RenderGrumpText(SpriteBatch batch, SpriteFont font, string text, Vector2 position, float scale = 1f, int alpha = 255)
        {
            var chars = text.Select(c => c.ToString());
            var white = new Color(_white.R, _white.G, _white.B, alpha);
            var blue = new Color(_blue.R, _blue.G, _blue.B, alpha);
            float offset = 0f;

            foreach (var letter in chars)
            {
                var size = font.MeasureString(letter);

                batch.DrawString(font, letter, position + new Vector2(offset - 2f, -2f), blue, 0f, Vector2.Zero, scale + 0.1f, SpriteEffects.None, 0f);
                batch.DrawString(font, letter, position + new Vector2(offset + 4, 4), blue, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                batch.DrawString(font, letter, position + new Vector2(offset, 0), white, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                offset += (size.X + font.Spacing) * scale;
            }
        }
    }
}
