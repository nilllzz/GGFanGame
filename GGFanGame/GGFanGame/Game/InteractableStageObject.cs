using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game
{
    /// <summary>
    /// A class that represents an interactable object, the base for all players, enemies and other complex objects.
    /// </summary>
    abstract class InteractableStageObject : StageObject
    {
        private Dictionary<ObjectState, Animation> _animations = new Dictionary<ObjectState, Animation>();
        private int _actionHintTextAlpha = 0;
        private StageObject _supportingObject = null;
        protected Vector3 _autoMovement = new Vector3(0);

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

        public bool hasAction { get; set; } = false;

        protected string actionHintText { get; set; } = "TEST";

        protected bool renderActionHint { get; set; } = false;

        #endregion

        public InteractableStageObject()
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

            if (drawShadow)
            {
                int shadowWidth = (int)(frame.Width * shadowSize);
                int shadowHeight = (int)(frame.Height * shadowSize * (1d / 4d));

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
            if ((hasAction && renderActionHint))
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
                    gameInstance.spriteBatch.DrawString(gameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFont"), actionHintText, new Vector2(X, Z - Y) * (float)Stage.activeStage().camera.scale, new Color(255, 255, 255, _actionHintTextAlpha));
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
                if (Y > groundY && gravityAffected)
                {
                    _autoMovement.Y--;
                }
                if (_autoMovement.Y < 0f && !gravityAffected)
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
                Stage.activeStage().addObject(new ActionWord(ActionWord.getWordText(ActionWordType.Landing), objectColor, 0.3f, position));

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

            if (faceAttack)
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
        private void supportingObjectPositionChanged(StageObject obj, Vector3 previousPosition)
        {
            //Get the difference in position and apply this difference to the position of this object:
            Vector3 difference = obj.position - previousPosition;
            position += difference;
        }
    }
}