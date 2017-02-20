using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Drawing
{
    internal static partial class SpriteBatchExtensions
    {
        /// <summary>
        /// Begins the SpriteBatch with parameters matching the specified usage.
        /// </summary>
        internal static void Begin(this SpriteBatch batch, SpriteBatchUsage usage)
        {
            switch (usage)
            {
                case SpriteBatchUsage.Default:
                    batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    break;
                case SpriteBatchUsage.Font:
                    batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    break;
                case SpriteBatchUsage.RealTransparency:
                    batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    break;
            }
        }

        internal static void DrawRectangle(this SpriteBatch batch, Rectangle rectangle, Color color)
            => _renderer.DrawRectangle(batch, rectangle, color);
        
        internal static void DrawCircle(this SpriteBatch batch, Vector2 position, int radius, Color color, double scale = 1D)
            => _renderer.DrawCircle(batch, position, radius, color, scale);

        internal static void DrawEllipse(this SpriteBatch batch, Rectangle rectangle, Color color, double scale = 1D)
            => _renderer.DrawEllipse(batch, rectangle, color, scale);
        
        internal static void DrawGradient(this SpriteBatch batch, Rectangle rectangle, Color fromColor, Color toColor, bool horizontal, double scale = 1D, int steps = -1)
            => _renderer.DrawGradient(batch, rectangle, fromColor, toColor, horizontal, scale, steps);

        internal static void DrawLine(this SpriteBatch batch, Vector2 start, Vector2 end, Color color, double width)
            => _renderer.DrawLine(batch, start, end, color, width);
    }
}
