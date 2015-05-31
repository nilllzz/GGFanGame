using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Screens.Game.Level.GrumpSpace
{
    /// <summary>
    /// A playable character in the grump space.
    /// </summary>
    abstract class PlayerCharacter : InteractableObject
    {
        private PlayerIndex _playerIndex;
        private float _playerSpeed = 5f;

        private Vector3 _autoMovement = new Vector3(0);

        protected float playerSpeed
        {
            get { return _playerSpeed; }
            set { _playerSpeed = value; }
        }

        /// <summary>
        /// Creates a new instance of the player character class.
        /// </summary>
        protected PlayerCharacter(GGGame game, PlayerIndex playerIndex) : base(game)
        {
            _playerIndex = playerIndex;
        }

        /// <summary>
        /// Draws the player character and related things like the shadow.
        /// </summary>
        public override void draw()
        {
            base.draw();

            SpriteEffects effect = SpriteEffects.None;
            if (facing == ObjectFacing.Left)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle frame = getAnimation().getFrameRec(animationFrame);

            gameInstance.spriteBatch.Draw(spriteSheet, new Rectangle((int)X, (int)(Z - Y), frame.Width * 2, frame.Height * 2), frame, Color.White, 0f, Vector2.Zero, effect, 0f);
        }

        public override void update()
        {
            ObjectState setToState = ObjectState.Idle;

            if (state == ObjectState.Dead)
            {
                setToState = ObjectState.Dead;
            }

            if (state == ObjectState.HurtFalling)
            {
                if (animationEnded())
                    if (health <= 0)
                    {
                        setToState = ObjectState.Dead;
                    }
                    else
                    {
                        setToState = ObjectState.StandingUp;
                    }
                else
                {
                    setToState = ObjectState.HurtFalling;
                }
            }

            if (state == ObjectState.Hurt)
            {
                if (animationEnded() && _autoMovement.X == 0f)
                {
                    repeatAnimation = true;
                }
                else
                {
                    setToState = ObjectState.Hurt;
                }
            }

            if (state == ObjectState.StandingUp)
            {
                if (animationEnded())
                    repeatAnimation = true;
                else
                    setToState = ObjectState.StandingUp;
            }

            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.A) && setToState == ObjectState.Idle)
            {
                getHit(false, 10f, 10);
                if (health <= 0)
                {
                    setToState = ObjectState.HurtFalling;
                }
                else
                {
                    setToState = ObjectState.Hurt;
                }
            }
            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.B) && setToState == ObjectState.Idle)
            {
                getHit(true, 15f, 20);
                setToState = ObjectState.HurtFalling;
            }

            if (setToState == ObjectState.Idle)
            {
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickRight))
                {
                    setToState = ObjectState.Walking;
                    X += _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Right);
                    facing = ObjectFacing.Right;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickLeft))
                {
                    setToState = ObjectState.Walking;
                    X -= _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Left);
                    facing = ObjectFacing.Left;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickUp))
                {
                    setToState = ObjectState.Walking;
                    Z -= _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Up);
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickDown))
                {
                    setToState = ObjectState.Walking;
                    Z += _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Down);
                }
            }

            setState(setToState);

            updateAutoMovement();

            base.update();
        }

        public override void getHit(bool knockback, float strength, int health)
        {
            base.getHit(knockback, strength, health);

            this.health -= health;

            if (this.health <= 0)
            {
                if (facing == ObjectFacing.Left)
                {
                    _autoMovement.X = strength * 1.5f;
                    _autoMovement.Y = strength;
                }
                else
                {
                    _autoMovement.X = -(strength * 1.5f);
                    _autoMovement.Y = strength;
                }
            }
            else
            {
                if (knockback)
                {
                    if (facing == ObjectFacing.Left)
                    {
                        _autoMovement.X = strength * 1.5f;
                        _autoMovement.Y = strength;
                    }
                    else
                    {
                        _autoMovement.X = -(strength * 1.5f);
                        _autoMovement.Y = strength;
                    }
                }
                else
                {
                    if (facing == ObjectFacing.Left)
                    {
                        _autoMovement.X = strength;
                    }
                    else
                    {
                        _autoMovement.X = -strength;
                    }
                }
            }


            repeatAnimation = false;
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
                _autoMovement.Y = 0f;
            }
        }
    }
}