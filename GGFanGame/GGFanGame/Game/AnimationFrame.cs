using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    /// <summary>
    /// A single frame of an animation.
    /// </summary>
    internal struct AnimationFrame
    {
        public Point frameSize { get; set; }

        /// <summary>
        /// The coordinates of this frame in the sprite sheet.
        /// </summary>
        public Point startPosition { get; set; }

        /// <summary>
        /// The length this frame appears on screen in frames.
        /// </summary>
        public double frameLength { get; set; }

        /// <summary>
        /// Returns the rectangle this frame represents in the sprite sheet.
        /// </summary>
        public Rectangle getRect()
        {
            return new Rectangle(startPosition.X, startPosition.Y, frameSize.X, frameSize.Y);
        }
    }
}
