using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game
{
    /// <summary>
    /// A class that represents an interactable object, the base for all players, enemies and other complex objects.
    /// </summary>
    internal abstract class InteractableStageObject : StageObject
    {
        private readonly Dictionary<ObjectState, Animation> _animations = new Dictionary<ObjectState, Animation>();
        private int _actionHintTextAlpha;
        private StageObject _supportingObject;

        #region Properties

        /// <summary>
        /// The sprite sheet to render.
        /// </summary>
        protected Texture2D SpriteSheet { get; set; }

        /// <summary>
        /// The current state of this object.
        /// </summary>
        public ObjectState State { get; set; }

        /// <summary>
        /// The frame index the current animation is at.
        /// </summary>
        protected int AnimationFrame { get; set; } = 0;

        /// <summary>
        /// Indicates wether the animation should loop once finished.
        /// </summary>
        protected bool RepeatAnimation { get; set; } = true;

        /// <summary>
        /// The delay between frames of animation.
        /// </summary>
        protected double AnimationDelay { get; private set; } = 0f;

        /// <summary>
        /// If the default draw void should draw a shadow.
        /// </summary>
        protected bool DrawShadow { get; set; } = true;

        /// <summary>
        /// The size of the default shadow, relative to 1.
        /// </summary>
        protected double ShadowSize { get; set; } = 1f;

        /// <summary>
        /// If this object falls when in the air.
        /// </summary>
        protected bool GravityAffected { get; set; } = true;

        /// <summary>
        /// When this is true, an attack makes this object face towards the attack.
        /// </summary>
        protected bool FaceAttack { get; set; } = true;

        protected Vector3 AutoMovement = new Vector3(0);

        public bool HasAction { get; set; } = false;

        protected string ActionHintText { get; set; } = "TEST";

        protected bool RenderActionHint { get; set; } = false;

        protected StageObject LastAttackedBy { get; private set; }

        #endregion

        protected InteractableStageObject()
        {
            SetState(ObjectState.Idle);
            CanInteract = true;
        }

        /// <summary>
        /// Adds an animation for a specific object state.
        /// </summary>
        protected void AddAnimation(ObjectState state, Animation animation)
        {
            _animations.Add(state, animation);
        }

        public override void Draw()
        {
            var frame = GetAnimation().GetFrameRec(AnimationFrame);
            var stageScale = Stage.ActiveStage.Camera.Scale;

            if (DrawShadow)
            {
                var shadowWidth = (int)(frame.Width * ShadowSize);
                var shadowHeight = (int)(frame.Height * ShadowSize * (1d / 4d));

                Drawing.Graphics.DrawEllipse(new Rectangle((int)((X - shadowWidth / 2d) * stageScale),
                           (int)((Z - shadowHeight / 2d - Stage.ActiveStage.GetGround(Position)) * stageScale),
                           (int)(shadowWidth * stageScale),
                           (int)(shadowHeight * stageScale)),
                           Stage.ActiveStage.AmbientColor, stageScale); //TODO: maybe, we have the shadow fade away when the player jumps?
            }

            var effect = SpriteEffects.None;
            if (Facing == ObjectFacing.Left) //Flip the sprite if facing the other way.
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            GameInstance.SpriteBatch.Draw(SpriteSheet, new Rectangle((int)((X - frame.Width / 2d) * stageScale),
                                                                     (int)((Z - Y - frame.Height) * stageScale),
                                                                     (int)(frame.Width * stageScale),
                                                                     (int)(frame.Height * stageScale)),
                                                       frame, Color.White, 0f, Vector2.Zero, effect, 0f);

            DrawActionHint();
        }

        /// <summary>
        /// Draws the action hint of this object.
        /// </summary>
        private void DrawActionHint()
        {
            if ((HasAction && RenderActionHint))
            {
                //Test if player is in range and get smallest range:
                var smallestPlayerDistance = -1f;

                foreach (var obj in Stage.ActiveStage.GetObjects())
                {
                    if (obj.GetType().IsSubclassOf(typeof(Playable.PlayerCharacter)) && obj != this)
                    {
                        var distance = Vector3.Distance(Position, obj.Position);

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
                    GameInstance.SpriteBatch.DrawString(GameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFont"), ActionHintText, new Vector2(X, Z - Y) * (float)Stage.ActiveStage.Camera.Scale, new Color(255, 255, 255, _actionHintTextAlpha));
                }
            }
        }

        public override void Update()
        {
            //Item1 is the actual object and Item2 is the Y position:
            var supporting = Stage.ActiveStage.GetSupporting(GetFeetPosition());
            UpdateSupporting(supporting.Item1);
            UpdateAutoMovement(supporting.Item2);

            if (GetAnimation().Frames.Length > 1)
            {
                AnimationDelay--;
                if (AnimationDelay <= 0d)
                {
                    AnimationFrame++;
                    if (AnimationFrame == GetAnimation().Frames.Length)
                    {
                        if (RepeatAnimation)
                        {
                            AnimationFrame = 0;
                        }
                        else
                        {
                            AnimationFrame--;
                        }
                    }
                    AnimationDelay = GetAnimation().Frames[AnimationFrame].FrameLength;
                }
            }
        }

        private void UpdateSupporting(StageObject supportingObject)
        {
            //We get the supporting object here.
            //This is important, because when the supporting object (aka the object this object stands on) changes its position,
            //this one gets dragged along with it.
            //When the supporting object changed, we unsubscribe from its position changed event and subscribe to the new one.
            if (supportingObject != _supportingObject)
            {
                if (_supportingObject != null)
                    _supportingObject.OnPositionChanged -= SupportingObjectPositionChanged;

                _supportingObject = supportingObject;

                if (_supportingObject != null)
                    _supportingObject.OnPositionChanged += SupportingObjectPositionChanged;
            }
        }

        /// <summary>
        /// Returns if the set animation ended. This happens after the animation is over and repeatAnimation is false.
        /// </summary>
        protected bool AnimationEnded()
        {
            return AnimationFrame == GetAnimation().Frames.Length - 1;
        }

        /// <summary>
        /// Returns the current animation.
        /// </summary>
        protected virtual Animation GetAnimation()
        {
            return _animations[State];
        }

        /// <summary>
        /// Sets the state to a new one and resets animations.
        /// </summary>
        protected void SetState(ObjectState newState)
        {
            if (newState != State)
            {
                State = newState;
                AnimationFrame = 0;
                AnimationDelay = GetAnimation().Frames[0].FrameLength;
            }
        }

        /// <summary>
        /// Updates the auto movement of the object. This also updates falling.
        /// </summary>
        private void UpdateAutoMovement(float groundY)
        {
            if (AutoMovement.X > 0f)
            {
                AutoMovement.X -= 0.5f;
                if (AutoMovement.X < 0f)
                    AutoMovement.X = 0f;
            }
            if (AutoMovement.X < 0f)
            {
                AutoMovement.X += 0.5f;
                if (AutoMovement.X > 0f)
                    AutoMovement.X = 0f;
            }

            if (AutoMovement.Y > 0f)
            {
                AutoMovement.Y -= 0.5f;
                if (AutoMovement.Y < 0f)
                    AutoMovement.Y = 0f;
            }
            else
            {
                if (Y > groundY && GravityAffected)
                {
                    AutoMovement.Y--;
                }
                if (AutoMovement.Y < 0f && !GravityAffected)
                {
                    AutoMovement.Y = 0f;
                }
            }

            if (AutoMovement.Z > 0f)
            {
                AutoMovement.Z -= 0.5f;
                if (AutoMovement.Z < 0f)
                    AutoMovement.Z = 0f;
            }
            if (AutoMovement.Z < 0f)
            {
                AutoMovement.Z += 0.5f;
                if (AutoMovement.Z > 0f)
                    AutoMovement.Z = 0f;
            }

            Y += AutoMovement.Y;

            var desiredPos = new Vector3(X + AutoMovement.X, Y, Z + AutoMovement.Z);

            if (AutoMovement.X != 0f || AutoMovement.Z != 0f)
            {
                if (!Stage.ActiveStage.Intersects(this, desiredPos))
                {
                    Position = desiredPos;
                }
                else
                {
                    var desiredPosX = new Vector3(X + AutoMovement.X, Y, Z);
                    var desiredPosZ = new Vector3(X, Y, Z + AutoMovement.Z);

                    if (!Stage.ActiveStage.Intersects(this, desiredPosX))
                    {
                        X = desiredPosX.X;
                    }
                    if (!Stage.ActiveStage.Intersects(this, desiredPosZ))
                    {
                        Z = desiredPosX.Z;
                    }
                }
            }
            
            if (Y < groundY)
            {
                Y = groundY;
                //Spawn an action word for where the player landed.
                Stage.ActiveStage.AddObject(new ActionWord(ActionWord.GetWordText(ActionWordType.Landing), ObjectColor, 0.3f, Position));

                if (AutoMovement.Y < -17f && State == ObjectState.HurtFalling)
                {
                    AutoMovement.Y = 8f;
                    if (Facing == ObjectFacing.Left)
                        AutoMovement.X = 12f;
                    else
                        AutoMovement.X = -12f;
                }
                else
                {
                    AutoMovement.Y = 0f;
                }
            }
        }

        public override Point GetDrawingSize()
        {
            //Returns the drawing size of the current frame:
            var frame = GetAnimation().GetFrameRec(AnimationFrame);
            var stageScale = Stage.ActiveStage.Camera.Scale;
            return new Point((int)(frame.Width * stageScale), (int)(frame.Height * stageScale));
        }

        public override void GetHit(StageObject origin, Vector3 movement, int health, bool knockback)
        {
            base.GetHit(origin, movement, health, knockback);

            AutoMovement = movement;
            this.Health -= health;

            if (health <= 0 || knockback)
            {
                SetState(ObjectState.HurtFalling);
            }
            else
            {
                SetState(ObjectState.Hurt);
            }

            LastAttackedBy = origin;
        }

        public override void GetHit(Attack attack)
        {
            base.GetHit(attack);

            var knockbackValue = attack.Strength + attack.Origin.Strength - Weight * 0.7f;

            if (Weight * 0.7f >= attack.Strength + attack.Origin.Strength)
            {
                knockbackValue = 0f;
                AutoMovement.Y += 1.5f;
            }

            if (State == ObjectState.Blocking)
            {
                Health -= (int)(attack.Health / 4d);
                if (attack.Facing == ObjectFacing.Right)
                    AutoMovement.X += knockbackValue;
                else
                    AutoMovement.X += -knockbackValue;
            }
            else
            {
                Health -= attack.Health;

                if (Health <= 0)
                {
                    if (attack.Facing == ObjectFacing.Right)
                    {
                        AutoMovement.X += knockbackValue * 1.5f;
                        AutoMovement.Y += knockbackValue;
                    }
                    else
                    {
                        AutoMovement.X += -(knockbackValue * 1.5f);
                        AutoMovement.Y += knockbackValue;
                    }
                }
                else
                {
                    if (attack.Knockback)
                    {
                        if (attack.Facing == ObjectFacing.Right)
                        {
                            AutoMovement.X += knockbackValue * 1.5f;
                            AutoMovement.Y += knockbackValue;
                        }
                        else
                        {
                            AutoMovement.X += -(knockbackValue * 1.5f);
                            AutoMovement.Y += knockbackValue;
                        }
                    }
                    else
                    {
                        if (attack.Facing == ObjectFacing.Right)
                        {
                            AutoMovement.X += knockbackValue;
                        }
                        else
                        {
                            AutoMovement.X += -knockbackValue;
                        }
                    }
                }

                RepeatAnimation = false;

                if (Health <= 0 || attack.Knockback)
                    SetState(ObjectState.HurtFalling);
                else
                    SetState(ObjectState.Hurt);
            }

            if (FaceAttack)
            {
                if (attack.Facing == ObjectFacing.Left)
                    Facing = ObjectFacing.Right;
                else
                    Facing = ObjectFacing.Left;
            }

            LastAttackedBy = attack.Origin;
        }

        /// <summary>
        /// This updates this object's position when its supporting object gets moved around.
        /// </summary>
        private void SupportingObjectPositionChanged(StageObject obj, Vector3 previousPosition)
        {
            //Get the difference in position and apply this difference to the position of this object:
            var difference = obj.Position - previousPosition;
            Position += difference;
        }
    }
}