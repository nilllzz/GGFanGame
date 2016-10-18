using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

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

        /// <summary>
        /// Begins to render the screen to a render target.
        /// </summary>
        internal static RenderTarget2D BeginRenderScreenToTarget()
        {
            var target = CreateScreenTarget();

            //End the sprite batch, render to current target.
            //Then, set to new render target and begin the batch.
            GameInstance.SpriteBatch.End();
            GameInstance.GraphicsDevice.SetRenderTarget(target);
            GameInstance.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            return target;
        }

        internal static void BeginRenderScreenToTarget(RenderTarget2D target)
        {
            //End the sprite batch, render to current target.
            //Then, set to new render target and begin the batch.
            GameInstance.SpriteBatch.End();
            GameInstance.GraphicsDevice.SetRenderTarget(target);
            GameInstance.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
        }

        /// <summary>
        /// Ends rendering the screen to a render target.
        /// </summary>
        internal static void EndRenderScreenToTarget()
        {
            //Ends the sprite batch for the current target, resets the target, and starts the sprite batch for the default target.
            GameInstance.SpriteBatch.End();
            ResetRenderTarget();
            GameInstance.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
        }

        /// <summary>
        /// Returns the currently active render target.
        /// </summary>
        internal static RenderTarget2D CurrentRenderTarget => (RenderTarget2D)GameInstance.GraphicsDevice.GetRenderTargets()[0].RenderTarget;
    }
}
