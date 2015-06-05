using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Level
{
    /// <summary>
    /// A class that represents an interactable object, the base for all players, enemies and other complex objects.
    /// </summary>
    abstract class InteractableStageObject : StageObject
    {
        /// <summary>
        /// An animation for the object, which is a collection of frames.
        /// </summary>
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

            /// <summary>
            /// Returns a rectangle depicting one of the animation's frames in the sprite sheet.
            /// </summary>
            /// <param name="animationFrame">The frame index</param>
            /// <returns></returns>
            public Rectangle getFrameRec(int animationFrame)
            {
                return _frames[animationFrame].getRect();
            }
        }

        /// <summary>
        /// A single frame of an animation.
        /// </summary>
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

            /// <summary>
            /// The coordinates of this frame in the sprite sheet.
            /// </summary>
            /// <returns></returns>
            public Point startPosition
            {
                get { return _startPosition; }
                set { _startPosition = value; }
            }

            /// <summary>
            /// The length this frame appears on screen in frames.
            /// </summary>
            /// <returns></returns>
            public double frameLength
            {
                get { return _frameLength; }
                set { _frameLength = value; }
            }

            /// <summary>
            /// Returns the rectangle this frame represents in the sprite sheet.
            /// </summary>
            /// <returns></returns>
            public Rectangle getRect()
            {
                return new Rectangle(_startPosition.X, _startPosition.Y, _frameSize.X, _frameSize.Y);
            }
        }

        private Texture2D _spriteSheet;
        private ObjectState _state;
        private Dictionary<ObjectState, Animation> _animations = new Dictionary<ObjectState, Animation>();
        private int _animationFrame = 0;
        private bool _repeatAnimation = true;
        private double _animationDelay = 0f;
        private double _shadowSize = 1;
        private bool _drawShadow = true;

        protected Vector3 _autoMovement = new Vector3(0);

        #region Properties

        protected Texture2D spriteSheet
        {
            get { return _spriteSheet; }
            set { _spriteSheet = value; }
        }

        public ObjectState state
        {
            get { return _state; }
            set { _state = value; }
        }

        protected int animationFrame
        {
            get { return _animationFrame; }
            set { _animationFrame = value; }
        }

        protected bool repeatAnimation
        {
            get { return _repeatAnimation; }
            set { _repeatAnimation = value; }
        }

        protected double animationDelay
        {
            get { return _animationDelay; }
        }

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

        public InteractableStageObject(GGGame game) : base(game)
        {
            setState(ObjectState.Idle);
            canInteract = true;
        }

        #endregion

        /// <summary>
        /// Adds an animation for a specific object state.
        /// </summary>
        protected void addAnimation(ObjectState state, Animation animation)
        {
            _animations.Add(state, animation);
        }

        public override void draw()
        {
            Rectangle frame = getAnimation().getFrameRec(animationFrame);
            double stageScale = Stage.activeStage().scale;

            if (_drawShadow)
            {
                int shadowWidth = (int)(frame.Width * _shadowSize * stageScale);
                int shadowHeight = (int)(frame.Height * _shadowSize * stageScale * (1d / (stageScale * 2)));


                Drawing.Graphics.drawEllipse(new Rectangle((int)(X - (shadowWidth / 2d)),
                                (int)(Z - shadowHeight / 2d - Stage.activeStage().getGround(position)),
                                shadowWidth,
                                shadowHeight),
                  new Color(0, 0, 0, 100), stageScale); //TODO: maybe, we have the shadow fade away when the player jumps?
            }

            SpriteEffects effect = SpriteEffects.None;
            if (facing == ObjectFacing.Left) //Flip the sprite if facing the other way.
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            gameInstance.spriteBatch.Draw(spriteSheet, new Rectangle((int)(X - frame.Width / 2d * stageScale),
                                                                     (int)(Z - Y - frame.Height * stageScale),
                                                                     (int)(frame.Width * stageScale),
                                                                     (int)(frame.Height * stageScale)),
                                                       frame, Color.White, 0f, Vector2.Zero, effect, 0f);
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

        /// <summary>
        /// Returns if the set animation ended. This happens after the animation is over and repeatAnimation is false.
        /// </summary>
        /// <returns></returns>
        protected bool animationEnded()
        {
            return _animationFrame == getAnimation().frames.Length - 1;
        }

        /// <summary>
        /// Returns the current animation.
        /// </summary>
        protected virtual Animation getAnimation()
        {
            return _animations[_state];
        }

        /// <summary>
        /// Sets the state to a new one and resets animations.
        /// </summary>
        protected void setState(ObjectState newState)
        {
            if (newState != _state)
            {
                _state = newState;
                _animationFrame = 0;
                _animationDelay = getAnimation().frames[0].frameLength;
            }
        }

        /// <summary>
        /// Updates the auto movement of the object. This also updates falling.
        /// </summary>
        private void updateAutoMovement()
        {
            float groundY = Stage.activeStage().getGround(getFeetPosition());

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
                if (Y > groundY)
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

            if (Y < groundY)
            {
                Y = groundY;
                //Spawn an action word for where the player landed.
                Stage.activeStage().addActionWord(new ActionWord(gameInstance, ActionWord.getWordText(ActionWord.WordType.Landing), objectColor, 0.3f, position));

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

        public override Point getDrawingSize()
        {
            //Returns the drawing size of the current frame:
            Rectangle frame = getAnimation().getFrameRec(animationFrame);
            double stageScale = Stage.activeStage().scale;
            return new Point((int)(frame.Width * stageScale), (int)(frame.Height * stageScale));
        }

        public override void getHit(Attack attack)
        {
            base.getHit(attack);

            float knockbackValue = attack.strength + attack.origin.strength - weigth * 0.7f;

            if (state == ObjectState.Blocking)
            {
                health -= (int)(attack.health / 4d);
                if (attack.facing == ObjectFacing.Right)
                    _autoMovement.X = knockbackValue;
                else
                    _autoMovement.X = -knockbackValue;
            }
            else
            {
                health -= attack.health;

                if (health <= 0)
                {
                    if (attack.facing == ObjectFacing.Right)
                    {
                        _autoMovement.X = knockbackValue * 1.5f;
                        _autoMovement.Y = knockbackValue;
                    }
                    else
                    {
                        _autoMovement.X = -(knockbackValue * 1.5f);
                        _autoMovement.Y = knockbackValue;
                    }
                }
                else
                {
                    if (attack.knockback)
                    {
                        if (attack.facing == ObjectFacing.Right)
                        {
                            _autoMovement.X = knockbackValue * 1.5f;
                            _autoMovement.Y = knockbackValue;
                        }
                        else
                        {
                            _autoMovement.X = -(knockbackValue * 1.5f);
                            _autoMovement.Y = knockbackValue;
                        }
                    }
                    else
                    {
                        if (attack.facing == ObjectFacing.Right)
                        {
                            _autoMovement.X = knockbackValue;
                        }
                        else
                        {
                            _autoMovement.X = -knockbackValue;
                        }
                    }
                }

                repeatAnimation = false;

                if (health <= 0 || attack.knockback)
                    setState(ObjectState.HurtFalling);
                else
                    setState(ObjectState.Hurt);
            }

            if (attack.facing == ObjectFacing.Left)
                facing = ObjectFacing.Right;
            else
                facing = ObjectFacing.Left;
        }
    }
}