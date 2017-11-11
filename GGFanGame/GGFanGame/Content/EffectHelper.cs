using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Content
{
    internal static class EffectHelper
    {
        internal static Effect GetGaussianBlurEffect(ContentManager content, float multiplier = 1f)
        {
            var effect = content.Load<Effect>(Resources.Shaders.GaussianBlur);
            effect.Parameters["multiplier"].SetValue(multiplier);
            return effect;
        }
    }
}
