using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GGFanGame.GameProvider;

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

        private Texture2D _spriteSheet;
        private ObjectState _state;
        private Dictionary<ObjectState, Animation> _animations = new Dictionary<ObjectState, Animation>();
        private int _animationFrame = 0;
        private bool _repeatAnimation = true;
        private double _animationDelay = 0f;
        private double _shadowSize = 1;
        private bool _drawShadow = true;
        private bool _faceAttack = true;
        private bool _gravityAffected = true;

        private bool _hasAction = false;
        private string _actionHintText = "TEST";
        private bool _renderActionHint = false;
        private int _actionHintTextAlpha = 0;

        private StageObject _supportingObject = null;

        protected Vector3 _autoMovement = new Vector3(0);

        #region Properties

        /// <summary>
        /// The sprite sheet to render.
        /// </summary>
        /// <returns></returns>
        protected Texture2D spriteSheet
        {
            get { return _spriteSheet; }
            set { _spriteSheet = value; }
        }

        /// <summary>
        /// The current state of this object.
        /// </summary>
        /// <returns></returns>
        public ObjectState state
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// The frame index the current animation is at.
        /// </summary>
        /// <returns></returns>
        protected int animationFrame
        {
            get { return _animationFrame; }
            set { _animationFrame = value; }
        }

        /// <summary>
        /// Indicates wether the animation should loop once finished.
        /// </summary>
        /// <returns></returns>
        protected bool repeatAnimation
        {
            get { return _repeatAnimation; }
            set { _repeatAnimation = value; }
        }

        /// <summary>
        /// The delay between frames of animation.
        /// </summary>
        /// <returns></returns>
        protected double animationDelay
        {
            get { return _animationDelay; }
        }

        /// <summary>
        /// If the default draw void should draw a shadow.
        /// </summary>
        /// <returns></returns>
        protected bool drawShadow
        {
            get { return _drawShadow; }
            set { _drawShadow = value; }
        }

        /// <summary>
        /// The size of the default shadow, relative to 1.
        /// </summary>
        /// <returns></returns>
        protected double shadowSize
        {
            get { return _shadowSize; }
            set { _shadowSize = value; }
        }

        /// <summary>
        /// If this object falls when in the air.
        /// </summary>
        /// <returns></returns>
        protected bool gravityAffected
        {
            get { return _gravityAffected; }
            set { _gravityAffected = value; }
        }

        /// <summary>
        /// When this is true, an attack makes this object face towards the attack.
        /// </summary>
        /// <returns></returns>
        protected bool faceAttack
        {
            get { return _faceAttack; }
            set { _faceAttack = value; }
        }

        public bool hasAction
        {
            get { return _hasAction; }
            set { _hasAction = value; }
        }

        protected string actionHintText
        {
            get { return _actionHintText; }
            set { _actionHintText = value; }
        }

        protected bool renderActionHint
        {
            get { return _renderActionHint; }
            set { _renderActionHint = value; }
        }

        #endregion

        public InteractableStageObject() : base()
        {
            setState(ObjectState.Idle);
            canInteract = true;
        }

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
            double stageScale = Stage.activeStage().camera.scale;

            if (_drawShadow)
            {
                int shadowWidth = (int)(frame.Width * _shadowSize);
                int shadowHeight = (int)(frame.Height * _shadowSize * (1d / 4d));

                Drawing.Graphics.drawEllipse(new Rectangle((int)((X - shadowWidth / 2d) * stageScale),
                           (int)((Z - shadowHeight / 2d - Stage.activeStage().getGround(position)) * stageScale),
                           (int)(shadowWidth * stageScale),
                           (int)(shadowHeight * stageScale)),
                           Stage.activeStage().ambientColor, stageScale); //TODO: maybe, we have the shadow fade away when the player jumps?
            }

            SpriteEffects effect = SpriteEffects.None;
            if (facing == ObjectFacing.Left) //Flip the sprite if facing the other way.
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            gameInstance.spriteBatch.Draw(spriteSheet, new Rectangle((int)((X - frame.Width / 2d) * stageScale),
                                                                     (int)((Z - Y - frame.Height) * stageScale),
                                                                     (int)(frame.Width * stageScale),
                                                                     (int)(frame.Height * stageScale)),
                                                       frame, Color.White, 0f, Vector2.Zero, effect, 0f);

            drawActionHint();
        }

        /// <summary>
        /// Draws the action hint of this object.
        /// </summary>
        private void drawActionHint()
        {
            if ((_hasAction && _renderActionHint))
            {
                //Test if player is in range and get smallest range:
                float smallestPlayerDistance = -1f;

                foreach (StageObject obj in Stage.activeStage().getObjects())
                {
                    if (obj.GetType().IsSubclassOf(typeof(Playable.PlayerCharacter)) && obj != this)
                    {
                        float distance = Vector3.Distance(position, obj.position);

                        if (distance < smallestPlayerDistance || smallestPlayerDistance < 0f)
                        {
                            smallestPlayerDistance = distance;
                        }
                    }
                }

                if (smallestPlayerDistance >= 0f && smallestPlayerDistance <= 12f)
                {
                    if (_actionHintTextAlpha < 255)
                    {
                        _actionHintTextAlpha += 11;
                        if (_actionHintTextAlpha >= 255)
                        {
                            _actionHintTextAlpha = 255;
                        }
                    }
                }
                else
                {
                    if (_actionHintTextAlpha > 0)
                    {
                        _actionHintTextAlpha -= 11;
                        if (_actionHintTextAlpha <= 0)
                        {
                            _actionHintTextAlpha = 0;
                        }
                    }
                }

                if (_actionHintTextAlpha > 0)
                {
                    //TODO: Render properly.
                    gameInstance.spriteBatch.DrawString(gameInstance.fontManager.load(@"CartoonFont"), _actionHintText, new Vector2(X, Z - Y) * (float)Stage.activeStage().camera.scale, new Color(255, 255, 255, _actionHintTextAlpha));
                }
            }
        }

        public override void update()
        {
            //Item1 is the actual object and Item2 is the Y position:
            var supporting = Stage.activeStage().getSupporting(getFeetPosition());
            updateSupporting(supporting.Item1);
            updateAutoMovement(supporting.Item2);

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

        private void updateSupporting(StageObject supportingObject)
        {
            //We get the supporting object here.
            //This is important, because when the supporting object (aka the object this object stands on) changes its position,
            //this one gets dragged along with it.
            //When the supporting object changed, we unsubscribe from its position changed event and subscribe to the new one.
            if (supportingObject != _supportingObject)
            {
                if (_supportingObject != null)
                    _supportingObject.OnPositionChanged -= supportingObjectPositionChanged;

                _supportingObject = supportingObject;

                if (_supportingObject != null)
                    _supportingObject.OnPositionChanged += supportingObjectPositionChanged;
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
        private void updateAutoMovement(float groundY)
        {
            if (_autoMovement.X > 0f)
            {
                _autoMovement.X -= 0.5f;
                if (_autoMovement.X < 0f)
                    _autoMovement.X = 0f;
            }
            if (_autoMovement.X < 0f)
            {
                _autoMovement.X += 0.5f;
                if (_autoMovement.X > 0f)
                    _autoMovement.X = 0f;
            }

            if (_autoMovement.Y > 0f)
            {
                _autoMovement.Y -= 0.5f;
                if (_autoMovement.Y < 0f)
                    _autoMovement.Y = 0f;
            }
            else
            {
                if (Y > groundY && _gravityAffected)
                {
                    _autoMovement.Y--;
                }
                if (_autoMovement.Y < 0f && !_gravityAffected)
                {
                    _autoMovement.Y = 0f;
                }
            }

            if (_autoMovement.Z > 0f)
            {
                _autoMovement.Z -= 0.5f;
                if (_autoMovement.Z < 0f)
                    _autoMovement.Z = 0f;
            }
            if (_autoMovement.Z < 0f)
            {
                _autoMovement.Z += 0.5f;
                if (_autoMovement.Z > 0f)
                    _autoMovement.Z = 0f;
            }

            Y += _autoMovement.Y;

            Vector3 desiredPos = new Vector3(X + _autoMovement.X, Y, Z + _autoMovement.Z);

            if (_autoMovement.X != 0f || _autoMovement.Z != 0f)
            {
                if (!Stage.activeStage().intersects(this, desiredPos))
                {
                    position = desiredPos;
                }
                else
                {
                    Vector3 desiredPosX = new Vector3(X + _autoMovement.X, Y, Z);
                    Vector3 desiredPosZ = new Vector3(X, Y, Z + _autoMovement.Z);

                    if (!Stage.activeStage().intersects(this, desiredPosX))
                    {
                        X = desiredPosX.X;
                    }
                    if (!Stage.activeStage().intersects(this, desiredPosZ))
                    {
                        Z = desiredPosX.Z;
                    }
                }
            }

            if (Y < groundY)
            {
                Y = groundY;
                //Spawn an action word for where the player landed.
                Stage.activeStage().addObject(new ActionWord(ActionWord.getWordText(ActionWord.WordType.Landing), objectColor, 0.3f, position));

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
            double stageScale = Stage.activeStage().camera.scale;
            return new Point((int)(frame.Width * stageScale), (int)(frame.Height * stageScale));
        }

        public override void getHit(Vector3 movement, int health, bool knockback)
        {
            base.getHit(movement, health, knockback);

            _autoMovement = movement;
            this.health -= health;

            if (health <= 0 || knockback)
            {
                setState(ObjectState.HurtFalling);
            }
            else
            {
                setState(ObjectState.Hurt);
            }
        }

        public override void getHit(Attack attack)
        {
            base.getHit(attack);

            float knockbackValue = attack.strength + attack.origin.strength - weight * 0.7f;

            if (weight * 0.7f >= attack.strength + attack.origin.strength)
            {
                knockbackValue = 0f;
                _autoMovement.Y += 1.5f;
            }

            if (state == ObjectState.Blocking)
            {
                health -= (int)(attack.health / 4d);
                if (attack.facing == ObjectFacing.Right)
                    _autoMovement.X += knockbackValue;
                else
                    _autoMovement.X += -knockbackValue;
            }
            else
            {
                health -= attack.health;

                if (health <= 0)
                {
                    if (attack.facing == ObjectFacing.Right)
                    {
                        _autoMovement.X += knockbackValue * 1.5f;
                        _autoMovement.Y += knockbackValue;
                    }
                    else
                    {
                        _autoMovement.X += -(knockbackValue * 1.5f);
                        _autoMovement.Y += knockbackValue;
                    }
                }
                else
                {
                    if (attack.knockback)
                    {
                        if (attack.facing == ObjectFacing.Right)
                        {
                            _autoMovement.X += knockbackValue * 1.5f;
                            _autoMovement.Y += knockbackValue;
                        }
                        else
                        {
                            _autoMovement.X += -(knockbackValue * 1.5f);
                            _autoMovement.Y += knockbackValue;
                        }
                    }
                    else
                    {
                        if (attack.facing == ObjectFacing.Right)
                        {
                            _autoMovement.X += knockbackValue;
                        }
                        else
                        {
                            _autoMovement.X += -knockbackValue;
                        }
                    }
                }

                repeatAnimation = false;

                if (health <= 0 || attack.knockback)
                    setState(ObjectState.HurtFalling);
                else
                    setState(ObjectState.Hurt);
            }

            if (_faceAttack)
            {
                if (attack.facing == ObjectFacing.Left)
                    facing = ObjectFacing.Right;
                else
                    facing = ObjectFacing.Left;
            }
        }

        /// <summary>
        /// This updates this object's position when its supporting object gets moved around.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="previousPosition"></param>
        private void supportingObjectPositionChanged(StageObject obj, Vector3 previousPosition)
        {
            //Get the difference in position and apply this difference to the position of this object:
            Vector3 difference = obj.position - previousPosition;
            position += difference;
        }
    }
}