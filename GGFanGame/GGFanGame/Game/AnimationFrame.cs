using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    /// <summary>
    /// A single frame of an animation.
    /// </summary>
    internal struct AnimationFrame
    {
        private Point _frameSize;
        private Point _startPosition;
        private double _frameLength;

        public Point frameSize
        {
            get { return _frameSize; }
            set { _frameSize = value; }
        }

        /// <summary>
        /// The coordinates of this frame in the sprite sheet.
        /// </summary>
        public Point startPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }

        /// <summary>
        /// The length this frame appears on screen in frames.
        /// </summary>
        public double frameLength
        {
            get { return _frameLength; }
            set { _frameLength = value; }
        }

        /// <summary>
        /// Returns the rectangle this frame represents in the sprite sheet.
        /// </summary>
        public Rectangle getRect()
        {
            return new Rectangle(_startPosition.X, _startPosition.Y, _frameSize.X, _frameSize.Y);
        }
    }
}
