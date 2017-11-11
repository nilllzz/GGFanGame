using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    /// <summary>
    /// A single frame of an animation.
    /// </summary>
    internal struct AnimationFrame
    {
        public Point FrameSize { get; set; }

        /// <summary>
        /// The coordinates of this frame in the sprite sheet.
        /// </summary>
        public Point StartPosition { get; set; }

        /// <summary>
        /// The length this frame appears on screen in frames.
        /// </summary>
        public double FrameLength { get; set; }

        /// <summary>
        /// Returns the rectangle this frame represents in the sprite sheet.
        /// </summary>
        public Rectangle GetRect()
        {
            return new Rectangle(StartPosition.X, StartPosition.Y, FrameSize.X, FrameSize.Y);
        }
    }
}
