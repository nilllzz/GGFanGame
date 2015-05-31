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

            public Rectangle getFrameRec(int animationFrame)
            {
                return _frames[animationFrame].getRect();
            }
        }

        protected struct AnimationFrame
        {
            private Point _frameSize;
            private Point _startPosition;
            private double _frameLength;

            public Point frameSize
            {
                get { return _frameSize; }
                set { _frameSize = value; }
            }
            public Point startPosition
            {
                get { return _startPosition; }
                set { _startPosition = value; }
            }
            public double frameLength
            {
                get { return _frameLength; }
                set { _frameLength = value; }
            }

            public Rectangle getRect()
            {
                return new Rectangle(_startPosition.X, _startPosition.Y, _frameSize.X, _frameSize.Y);
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

        protected double shadowSize
        {
            get { return _shadowSize; }
            set { _shadowSize = value; }
        }

        protected Vector3 _autoMovement = new Vector3(0);

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

                int shadowWidth = (int)(drawFrame.Width * _shadowSize * 2d);
                int shadowHeight = (int)(drawFrame.Height * _shadowSize * 2d * (1d / 4d));

                gameInstance.spriteBatch.Draw(_shadowTexture, 
                    new Rectangle((int)(X + (drawFrame.Width - (shadowWidth / 2d))), 
                                  (int)(Z + drawFrame.Height * 2d - shadowHeight / 2d), 
                                  shadowWidth, 
                                  shadowHeight), 
                    new Color(0, 0, 0, 100));
            }
        }

        public override void update()
        {
            updateAutoMovement();

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

        private void updateAutoMovement()
        {
            if (_autoMovement.X > 0f)
            {
                _autoMovement.X--;
                if (_autoMovement.X < 0f)
                    _autoMovement.X = 0f;
            }
            if (_autoMovement.X < 0f)
            {
                _autoMovement.X++;
                if (_autoMovement.X > 0f)
                    _autoMovement.X = 0f;
            }

            if (_autoMovement.Y > 0f)
            {
                _autoMovement.Y--;
                if (_autoMovement.Y < 0f)
                    _autoMovement.Y = 0f;
            }
            else
            {
                if (Y > 0f)
                {
                    _autoMovement.Y--;
                }
            }

            if (_autoMovement.Z > 0f)
            {
                _autoMovement.Z--;
                if (_autoMovement.Z < 0f)
                    _autoMovement.Z = 0f;
            }
            if (_autoMovement.Z < 0f)
            {
                _autoMovement.Z++;
                if (_autoMovement.Z > 0f)
                    _autoMovement.Z = 0f;
            }

            X += _autoMovement.X;
            Y += _autoMovement.Y;
            Z += _autoMovement.Z;

            if (Y < 0f)
            {
                Y = 0f;

                if (_autoMovement.Y < -17f && state == ObjectState.HurtFalling)
                {
                    _autoMovement.Y = 8f;
                    if (facing == ObjectFacing.Left)
                        _autoMovement.X = 12f;
                    else
                        _autoMovement.X = -12f;
                }
                else
                {
                    _autoMovement.Y = 0f;
                }
            }
        }
    }
}