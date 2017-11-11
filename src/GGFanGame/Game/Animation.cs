using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    /// <summary>
    /// An animation for the object, which is a collection of frames.
    /// </summary>
    internal struct Animation
    {
        public AnimationFrame[] Frames { get; }

        public Animation(int frameCount, Point startPosition, Point frameSize, double frameLength) : this(frameCount, startPosition, frameSize, frameLength, 0)
        { }

        public Animation(int frameCount, Point startPosition, Point frameSize, double frameLength, int repeatLastFrameCount)
        {
            Frames = new AnimationFrame[frameCount + repeatLastFrameCount];
            for (var i = 0; i < frameCount; i++)
            {
                Frames[i] = new AnimationFrame { FrameLength = frameLength, StartPosition = startPosition + new Point(i * frameSize.X, 0), FrameSize = frameSize };
            }
            if (repeatLastFrameCount > 0)
            {
                for (var i = 0; i < repeatLastFrameCount; i++)
                {
                    Frames[frameCount + i] = Frames[frameCount - 1];
                }
            }
        }

        /// <summary>
        /// Returns a rectangle depicting one of the animation's frames in the sprite sheet.
        /// </summary>
        /// <param name="animationFrame">The frame index</param>
        public Rectangle GetFrameRec(int animationFrame)
        {
            return Frames[animationFrame].GetRect();
        }
    }
}
