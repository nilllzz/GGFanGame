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

        private Vector2 _autoMovement = new Vector2(0);

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

            if (state == ObjectState.StandingUp)
            {
                if (animationEnded())
                    repeatAnimation = true;
                else
                    setToState = ObjectState.StandingUp;
            }

            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.A) && setToState == ObjectState.Idle)
            {
                setToState = ObjectState.HurtFalling;
                if (facing == ObjectFacing.Left)
                    _autoMovement.X = 18;
                else
                    _autoMovement.X = -18;

                repeatAnimation = false;
                health -= 24;
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
            if (_autoMovement.Y < 0f)
            {
                _autoMovement.Y++;
                if (_autoMovement.Y > 0f)
                    _autoMovement.Y = 0f;
            }

            X += _autoMovement.X;
            Z += _autoMovement.Y;
        }
    }
}