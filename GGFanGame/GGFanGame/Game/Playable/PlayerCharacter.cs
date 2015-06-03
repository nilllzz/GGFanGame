using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Game.Level.Playable
{
    /// <summary>
    /// A playable character in the grump space.
    /// </summary>
    abstract class PlayerCharacter : InteractableStageObject
    {
        #region Combo

        private Dictionary<string, Animation> _comboAnimations = new Dictionary<string, Animation>();

        private string _comboChain = ""; //The current combo chain.
        private double _comboDelay = 0d; //The time period after an attack to chain a combo.

        /// <summary>
        /// Adds an animation bound to an attack combo.
        /// </summary>
        protected void addComboAnimation(string comboChain, Animation animation)
        {
            _comboAnimations.Add(comboChain, animation);
        }

        private Dictionary<string, Attack> _attacks = new Dictionary<string, Attack>();

        protected void addAttack(string combo, int frame, Attack attack)
        {
            _attacks.Add(combo + frame.ToString(), attack);
        }

        #endregion

        private PlayerIndex _playerIndex;
        private float _playerSpeed = 5f;
        private string _name = "";

        protected float playerSpeed
        {
            get { return _playerSpeed; }
            set { _playerSpeed = value; }
        }

        public string name
        {
            get { return _name; }
        }

        /// <summary>
        /// Creates a new instance of the player character class.
        /// </summary>
        protected PlayerCharacter(GGGame game, PlayerIndex playerIndex, string name) : base(game)
        {
            _playerIndex = playerIndex;
            _name = name;

            objectColor = Drawing.Colors.getColor(playerIndex);
        }

        /// <summary>
        /// Draws the player character.
        /// </summary>
        public override void draw()
        {
            base.draw();
        }

        public override void update()
        {
            updateState();

            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.DPadLeft))
            {
                health -= 10;
            }

            if (state == ObjectState.Attacking && getAnimation().frames[animationFrame].frameLength == animationDelay)
            {
                if (_attacks.Keys.Contains(_comboChain + animationFrame.ToString()))
                {
                    Attack attack = _attacks[_comboChain + animationFrame.ToString()];
                    attack.facing = facing;

                    Stage.activeStage().singleHitAll(attack, position);
                }
            }

            base.update();
        }

        private void updateState()
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

            //Attacking:
            if (!animationEnded() && setToState == ObjectState.Idle && state == ObjectState.Attacking)
            {
                setToState = ObjectState.Attacking;
            }
            else
            {
                if (_comboDelay > 0d)
                {
                    if (setToState != ObjectState.Idle)
                    {
                        _comboDelay = 0d;
                        _comboChain = "";
                    }
                    else
                    {
                        _comboDelay--;
                        if (_comboDelay <= 0d)
                        {
                            _comboDelay = 0d;
                            _comboChain = "";
                            setToState = ObjectState.Idle;
                            repeatAnimation = true;
                        }
                        else
                        {
                            setToState = ObjectState.Attacking;
                        }
                    }
                }
            }

            if (_comboChain == "" || animationEnded())
            {
                if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.X) && (setToState == ObjectState.Idle || setToState == ObjectState.Attacking))
                {
                    _comboChain += "B";
                    if (!_comboAnimations.Keys.Contains(_comboChain))
                    {
                        _comboChain = "";
                        _comboDelay = 0d;
                        setToState = ObjectState.Idle;
                        repeatAnimation = true;
                    }
                    else
                    {
                        animationFrame = 0;
                        setToState = ObjectState.Attacking;
                        repeatAnimation = false;
                        _comboDelay = 12d;
                        if (facing == ObjectFacing.Left)
                            _autoMovement.X -= 12;
                        else
                            _autoMovement.X += 12;
                    }
                }
            }

            //Jumping and landing:
            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.B) && setToState == ObjectState.Idle && Y == 0f)
            {
                _autoMovement.Y = 18f;
                setToState = ObjectState.Jumping;
            }
            if (state == ObjectState.Jumping && setToState == ObjectState.Idle)
            {
                if (_autoMovement.Y > 0f)
                {
                    setToState = ObjectState.Jumping;
                }
                else
                {
                    setToState = ObjectState.Falling;
                }
            }

            //Blocking:
            if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftShoulder) && setToState == ObjectState.Idle && Y == 0f)
            {
                setToState = ObjectState.Blocking;
            }

            //Walking + movement while in the air:
            if ((setToState == ObjectState.Idle || setToState == ObjectState.Falling || setToState == ObjectState.Jumping) && _autoMovement.X == 0f)
            {
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickRight))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;
                    X += _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Right);
                    facing = ObjectFacing.Right;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickLeft))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;
                    X -= _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Left);
                    facing = ObjectFacing.Left;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickUp))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;
                    Z -= _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Up);
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickDown))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;
                    Z += _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Down);
                }
            }

            if ((setToState == ObjectState.Walking || setToState == ObjectState.Idle) && Y > 0f)
            {
                setToState = ObjectState.Falling;
            }

            setState(setToState);
        }

        protected override Animation getAnimation()
        {
            if (state == ObjectState.Attacking)
            {
                return _comboAnimations[_comboChain];
            }

            return base.getAnimation();
        }
    }
}