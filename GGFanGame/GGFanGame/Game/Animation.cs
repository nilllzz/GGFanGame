using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    /// <summary>
    /// An animation for the object, which is a collection of frames.
    /// </summary>
    internal struct Animation
    {
        private AnimationFrame[] _frames;

        public AnimationFrame[] frames
        {
            get { return _frames; }
        }

        public Animation(int frameCount, Point startPosition, Point frameSize, double frameLength) : this(frameCount, startPosition, frameSize, frameLength, 0)
        { }

        public Animation(int frameCount, Point startPosition, Point frameSize, double frameLength, int repeatLastFrameCount)
        {
            _frames = new AnimationFrame[frameCount + repeatLastFrameCount];
            for (int i = 0; i < frameCount; i++)
            {
                _frames[i] = new AnimationFrame() { frameLength = frameLength, startPosition = startPosition + new Point(i * frameSize.X, 0), frameSize = frameSize };
            }
            if (repeatLastFrameCount > 0)
            {
                for (int i = 0; i < repeatLastFrameCount; i++)
                {
                    _frames[frameCount + i] = _frames[frameCount - 1];
                }
            }
        }

        /// <summary>
        /// Returns a rectangle depicting one of the animation's frames in the sprite sheet.
        /// </summary>
        /// <param name="animationFrame">The frame index</param>
        public Rectangle getFrameRec(int animationFrame)
        {
            return _frames[animationFrame].getRect();
        }
    }
}
