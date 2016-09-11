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
        internal static RenderTarget2D defaultTarget { get; private set; }

        internal static void initialize()
        {
            defaultTarget = createScreenTarget();
        }

        internal static RenderTarget2D createScreenTarget()
        {
            return new RenderTarget2D(gameInstance.GraphicsDevice, GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT, false, default(SurfaceFormat), DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        /// <summary>
        /// Resets the render target to the default.
        /// </summary>
        internal static void resetRenderTarget()
        {
            gameInstance.GraphicsDevice.SetRenderTarget(defaultTarget);
        }

        /// <summary>
        /// Begins to render the screen to a render target.
        /// </summary>
        internal static RenderTarget2D beginRenderScreenToTarget()
        {
            var target = createScreenTarget();

            //End the sprite batch, render to current target.
            //Then, set to new render target and begin the batch.
            gameInstance.spriteBatch.End();
            gameInstance.GraphicsDevice.SetRenderTarget(target);
            gameInstance.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            return target;
        }

        internal static void beginRenderScreenToTarget(RenderTarget2D target)
        {
            //End the sprite batch, render to current target.
            //Then, set to new render target and begin the batch.
            gameInstance.spriteBatch.End();
            gameInstance.GraphicsDevice.SetRenderTarget(target);
            gameInstance.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
        }

        /// <summary>
        /// Ends rendering the screen to a render target.
        /// </summary>
        internal static void endRenderScreenToTarget()
        {
            //Ends the sprite batch for the current target, resets the target, and starts the sprite batch for the default target.
            gameInstance.spriteBatch.End();
            resetRenderTarget();
            gameInstance.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
        }

        /// <summary>
        /// Returns the currently active render target.
        /// </summary>
        internal static RenderTarget2D currentRenderTarget => (RenderTarget2D)gameInstance.GraphicsDevice.GetRenderTargets()[0].RenderTarget;
    }
}
