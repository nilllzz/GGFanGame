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
        protected Texture2D spriteSheet { get; set; }

        /// <summary>
        /// The current state of this object.
        /// </summary>
        public ObjectState state { get; set; }

        /// <summary>
        /// The frame index the current animation is at.
        /// </summary>
        protected int animationFrame { get; set; } = 0;

        /// <summary>
        /// Indicates wether the animation should loop once finished.
        /// </summary>
        protected bool repeatAnimation { get; set; } = true;

        /// <summary>
        /// The delay between frames of animation.
        /// </summary>
        protected double animationDelay { get; private set; } = 0f;

        /// <summary>
        /// If the default draw void should draw a shadow.
        /// </summary>
        protected bool drawShadow { get; set; } = true;

        /// <summary>
        /// The size of the default shadow, relative to 1.
        /// </summary>
        protected double shadowSize { get; set; } = 1f;

        /// <summary>
        /// If this object falls when in the air.
        /// </summary>
        protected bool gravityAffected { get; set; } = true;

        /// <summary>
        /// When this is true, an attack makes this object face towards the attack.
        /// </summary>
        protected bool faceAttack { get; set; } = true;

        protected Vector3 autoMovement = new Vector3(0);

        public bool hasAction { get; set; } = false;

        protected string actionHintText { get; set; } = "TEST";

        protected bool renderActionHint { get; set; } = false;

        protected StageObject lastAttackedBy { get; private set; }

        #endregion

        protected InteractableStageObject()
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
            var frame = getAnimation().getFrameRec(animationFrame);
            var stageScale = Stage.activeStage.camera.scale;

            if (drawShadow)
            {
                var shadowWidth = (int)(frame.Width * shadowSize);
                var shadowHeight = (int)(frame.Height * shadowSize * (1d / 4d));

                Drawing.Graphics.drawEllipse(new Rectangle((int)((X - shadowWidth / 2d) * stageScale),
                           (int)((Z - shadowHeight / 2d - Stage.activeStage.getGround(position)) * stageScale),
                           (int)(shadowWidth * stageScale),
                           (int)(shadowHeight * stageScale)),
                           Stage.activeStage.ambientColor, stageScale); //TODO: maybe, we have the shadow fade away when the player jumps?
            }

            var effect = SpriteEffects.None;
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
            if ((hasAction && renderActionHint))
            {
                //Test if player is in range and get smallest range:
                var smallestPlayerDistance = -1f;

                foreach (var obj in Stage.activeStage.getObjects())
                {
                    if (obj.GetType().IsSubclassOf(typeof(Playable.PlayerCharacter)) && obj != this)
                    {
                        var distance = Vector3.Distance(position, obj.position);

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
                    gameInstance.spriteBatch.DrawString(gameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFont"), actionHintText, new Vector2(X, Z - Y) * (float)Stage.activeStage.camera.scale, new Color(255, 255, 255, _actionHintTextAlpha));
                }
            }
        }

        public override void update()
        {
            //Item1 is the actual object and Item2 is the Y position:
            var supporting = Stage.activeStage.getSupporting(getFeetPosition());
            updateSupporting(supporting.Item1);
            updateAutoMovement(supporting.Item2);

            if (getAnimation().frames.Length > 1)
            {
                animationDelay--;
                if (animationDelay <= 0d)
                {
                    animationFrame++;
                    if (animationFrame == getAnimation().frames.Length)
                    {
                        if (repeatAnimation)
                        {
                            animationFrame = 0;
                        }
                        else
                        {
                            animationFrame--;
                        }
                    }
                    animationDelay = getAnimation().frames[animationFrame].frameLength;
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
        protected bool animationEnded()
        {
            return animationFrame == getAnimation().frames.Length - 1;
        }

        /// <summary>
        /// Returns the current animation.
        /// </summary>
        protected virtual Animation getAnimation()
        {
            return _animations[state];
        }

        /// <summary>
        /// Sets the state to a new one and resets animations.
        /// </summary>
        protected void setState(ObjectState newState)
        {
            if (newState != state)
            {
                state = newState;
                animationFrame = 0;
                animationDelay = getAnimation().frames[0].frameLength;
            }
        }

        /// <summary>
        /// Updates the auto movement of the object. This also updates falling.
        /// </summary>
        private void updateAutoMovement(float groundY)
        {
            if (autoMovement.X > 0f)
            {
                autoMovement.X -= 0.5f;
                if (autoMovement.X < 0f)
                    autoMovement.X = 0f;
            }
            if (autoMovement.X < 0f)
            {
                autoMovement.X += 0.5f;
                if (autoMovement.X > 0f)
                    autoMovement.X = 0f;
            }

            if (autoMovement.Y > 0f)
            {
                autoMovement.Y -= 0.5f;
                if (autoMovement.Y < 0f)
                    autoMovement.Y = 0f;
            }
            else
            {
                if (Y > groundY && gravityAffected)
                {
                    autoMovement.Y--;
                }
                if (autoMovement.Y < 0f && !gravityAffected)
                {
                    autoMovement.Y = 0f;
                }
            }

            if (autoMovement.Z > 0f)
            {
                autoMovement.Z -= 0.5f;
                if (autoMovement.Z < 0f)
                    autoMovement.Z = 0f;
            }
            if (autoMovement.Z < 0f)
            {
                autoMovement.Z += 0.5f;
                if (autoMovement.Z > 0f)
                    autoMovement.Z = 0f;
            }

            Y += autoMovement.Y;

            var desiredPos = new Vector3(X + autoMovement.X, Y, Z + autoMovement.Z);

            if (autoMovement.X != 0f || autoMovement.Z != 0f)
            {
                if (!Stage.activeStage.intersects(this, desiredPos))
                {
                    position = desiredPos;
                }
                else
                {
                    var desiredPosX = new Vector3(X + autoMovement.X, Y, Z);
                    var desiredPosZ = new Vector3(X, Y, Z + autoMovement.Z);

                    if (!Stage.activeStage.intersects(this, desiredPosX))
                    {
                        X = desiredPosX.X;
                    }
                    if (!Stage.activeStage.intersects(this, desiredPosZ))
                    {
                        Z = desiredPosX.Z;
                    }
                }
            }
            
            if (Y < groundY)
            {
                Y = groundY;
                //Spawn an action word for where the player landed.
                Stage.activeStage.addObject(new ActionWord(ActionWord.getWordText(ActionWordType.Landing), objectColor, 0.3f, position));

                if (autoMovement.Y < -17f && state == ObjectState.HurtFalling)
                {
                    autoMovement.Y = 8f;
                    if (facing == ObjectFacing.Left)
                        autoMovement.X = 12f;
                    else
                        autoMovement.X = -12f;
                }
                else
                {
                    autoMovement.Y = 0f;
                }
            }
        }

        public override Point getDrawingSize()
        {
            //Returns the drawing size of the current frame:
            var frame = getAnimation().getFrameRec(animationFrame);
            var stageScale = Stage.activeStage.camera.scale;
            return new Point((int)(frame.Width * stageScale), (int)(frame.Height * stageScale));
        }

        public override void getHit(StageObject origin, Vector3 movement, int health, bool knockback)
        {
            base.getHit(origin, movement, health, knockback);

            autoMovement = movement;
            this.health -= health;

            if (health <= 0 || knockback)
            {
                setState(ObjectState.HurtFalling);
            }
            else
            {
                setState(ObjectState.Hurt);
            }

            lastAttackedBy = origin;
        }

        public override void getHit(Attack attack)
        {
            base.getHit(attack);

            var knockbackValue = attack.strength + attack.origin.strength - weight * 0.7f;

            if (weight * 0.7f >= attack.strength + attack.origin.strength)
            {
                knockbackValue = 0f;
                autoMovement.Y += 1.5f;
            }

            if (state == ObjectState.Blocking)
            {
                health -= (int)(attack.health / 4d);
                if (attack.facing == ObjectFacing.Right)
                    autoMovement.X += knockbackValue;
                else
                    autoMovement.X += -knockbackValue;
            }
            else
            {
                health -= attack.health;

                if (health <= 0)
                {
                    if (attack.facing == ObjectFacing.Right)
                    {
                        autoMovement.X += knockbackValue * 1.5f;
                        autoMovement.Y += knockbackValue;
                    }
                    else
                    {
                        autoMovement.X += -(knockbackValue * 1.5f);
                        autoMovement.Y += knockbackValue;
                    }
                }
                else
                {
                    if (attack.knockback)
                    {
                        if (attack.facing == ObjectFacing.Right)
                        {
                            autoMovement.X += knockbackValue * 1.5f;
                            autoMovement.Y += knockbackValue;
                        }
                        else
                        {
                            autoMovement.X += -(knockbackValue * 1.5f);
                            autoMovement.Y += knockbackValue;
                        }
                    }
                    else
                    {
                        if (attack.facing == ObjectFacing.Right)
                        {
                            autoMovement.X += knockbackValue;
                        }
                        else
                        {
                            autoMovement.X += -knockbackValue;
                        }
                    }
                }

                repeatAnimation = false;

                if (health <= 0 || attack.knockback)
                    setState(ObjectState.HurtFalling);
                else
                    setState(ObjectState.Hurt);
            }

            if (faceAttack)
            {
                if (attack.facing == ObjectFacing.Left)
                    facing = ObjectFacing.Right;
                else
                    facing = ObjectFacing.Left;
            }

            lastAttackedBy = attack.origin;
        }

        /// <summary>
        /// This updates this object's position when its supporting object gets moved around.
        /// </summary>
        private void supportingObjectPositionChanged(StageObject obj, Vector3 previousPosition)
        {
            //Get the difference in position and apply this difference to the position of this object:
            var difference = obj.position - previousPosition;
            position += difference;
        }
    }
}