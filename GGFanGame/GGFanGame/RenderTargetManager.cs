using GGFanGame.Drawing;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame
{
    /// <summary>
    /// Class to manage the render targets assigned to the game's <see cref="GraphicsDevice"/>.
    /// </summary>
    internal static class RenderTargetManager
    {
        // the default render target for the game.
        internal static RenderTarget2D DefaultTarget { get; private set; }

        internal static void initialize()
        {
            DefaultTarget = CreateScreenTarget();
        }

        internal static RenderTarget2D CreateScreenTarget()
        {
            return new RenderTarget2D(GameInstance.GraphicsDevice, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT, false, default(SurfaceFormat), DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        /// <summary>
        /// Resets the render target to the default.
        /// </summary>
        internal static void ResetRenderTarget()
        {
            GameInstance.GraphicsDevice.SetRenderTarget(DefaultTarget);
        }

        internal static void BeginRenderScreenToTarget(RenderTarget2D target)
        {
            //set to new render target 
            GameInstance.GraphicsDevice.SetRenderTarget(target);
        }

        /// <summary>
        /// Ends rendering the screen to a render target.
        /// </summary>
        internal static void EndRenderScreenToTarget()
        {
            BeginRenderScreenToTarget(DefaultTarget);
        }

        /// <summary>
        /// Returns the currently active render target.
        /// </summary>
        internal static RenderTarget2D CurrentRenderTarget => (RenderTarget2D)GameInstance.GraphicsDevice.GetRenderTargets()[0].RenderTarget;
    }
}
