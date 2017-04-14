using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Rendering
{
    internal class GraphicsDeviceHelper
    {
        internal static void ResetGraphicsDevice()
        {
            GameInstance.GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            GameInstance.GraphicsDevice.SamplerStates[0] = new SamplerState
            {
                Filter = TextureFilter.Point
            };
            GameInstance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
    }
}
