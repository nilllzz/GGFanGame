using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Screens.Game.Level.GrumpSpace
{
    public enum ObjectState
    {
        Idle,
        Walking,
        Jumping,
        Falling,

        Hurt,
        HurtFalling,
        OnBack,
        StandingUp,
        Dead
    }

    abstract class InteractableObject : LevelObject
    {
        private static Texture2D _shadowTexture = null;

        protected struct Animation
        {
            public AnimationFrame[] frames;

            public Animation(int frameCount, Point startPosition, Point frameSize, double frameLength) : this(frameCount, startPosition, frameSize, frameLength, 0)
            { }

            public Animation(int frameCount, Point startPosition, Point frameSize, double frameLength, int repeatLastFrameCount)
            {
                frames = new AnimationFrame[frameCount + repeatLastFrameCount];
                for (int i = 0; i < frameCount; i++)
                {
                    frames[i] = new AnimationFrame() { frameLength = frameLength, startPosition = startPosition + new Point(i * frameSize.X, 0), frameSize = frameSize };
                }
                if (repeatLastFrameCount > 0)
                {
                    for (int i = 0; i < repeatLastFrameCount; i++)
                    {
                        frames[frameCount + i] = frames[frameCount - 1];
                    }
                }
            }

            public Rectangle getFrameRec(int animationFrame)
            {
                return frames[animationFrame].getRect();
            }
        }

        protected struct AnimationFrame
        {
            public Point frameSize;
            public Point startPosition;
            public double frameLength;

            public Rectangle getRect()
            {
                return new Rectangle(startPosition.X, startPosition.Y, frameSize.X, frameSize.Y);
            }
        }

        private Texture2D _spriteSheet;

        protected Texture2D spriteSheet
        {
            get { return _spriteSheet; }
            set { _spriteSheet = value; }
        }

        private ObjectState _state;

        public ObjectState state
        {
            get { return _state; }
            set { _state = value; }
        }

        private Dictionary<ObjectState, Animation> _animations = new Dictionary<ObjectState, Animation>();
        private int _animationFrame = 0;
        protected int animationFrame
        {
            get { return _animationFrame; }
            set { _animationFrame = value; }
        }

        private bool _repeatAnimation = true;
        protected bool repeatAnimation
        {
            get { return _repeatAnimation; }
            set { _repeatAnimation = value; }
        }

        private double _animationDelay = 0f;

        private double _shadowSize = 1;
        private bool _drawShadow = true;

        protected bool drawShadow
        {
            get { return _drawShadow; }
            set { _drawShadow = value; }
        }

        public InteractableObject(GGGame game) : base(game)
        {
            if (_shadowTexture == null)
                _shadowTexture = gameInstance.textureManager.getResource(@"Misc\Shadow");

            setState(ObjectState.Idle);
        }

        protected void addAnimation(ObjectState state, Animation animation)
        {
            _animations.Add(state, animation);
        }

        public override void draw()
        {
            if (_drawShadow)
            {
                Rectangle drawFrame = getAnimation().getFrameRec(_animationFrame);

                gameInstance.spriteBatch.Draw(_shadowTexture, new Rectangle((int)X, (int)Z + (int)(drawFrame.Height * (3d / 4d)), drawFrame.Width, drawFrame.Height / 4), new Color(0, 0, 0, 100));
            }
        }

        public override void update()
        {
            if (getAnimation().frames.Length > 1)
            {
                _animationDelay--;
                if (_animationDelay <= 0d)
                {
                    _animationFrame++;
                    if (_animationFrame == getAnimation().frames.Length)
                    {
                        if (_repeatAnimation)
                        {
                            _animationFrame = 0;
                        }
                        else
                        {
                            _animationFrame--;
                        }
                    }
                    _animationDelay = getAnimation().frames[_animationFrame].frameLength;
                }
            }
        }

        protected bool animationEnded()
        {
            return _animationFrame == getAnimation().frames.Length - 1;
        }

        protected Animation getAnimation()
        {
            return _animations[_state];
        }

        protected void setState(ObjectState state)
        {
            if (state != _state)
            {
                _state = state;
                _animationFrame = 0;
                _animationDelay = getAnimation().frames[0].frameLength;
            }
        }

        public virtual void getHit(bool knockback, float strength, int health)
        {
            //By default, nothing happens...
        }
    }
}