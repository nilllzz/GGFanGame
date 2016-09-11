﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// A playable character.
    /// </summary>
    abstract class PlayerCharacter : InteractableStageObject
    {
        #region Attacking

        /// <summary>
        /// Defines a single part of an attack chain.
        /// </summary>
        protected struct PlayerAttack
        {
            Animation _animation;
            Dictionary<int, AttackDefinition> _attacks;
            Vector2 _movement;

            public PlayerAttack(Animation animation, Vector2 movement)
            {
                _animation = animation;
                _movement = movement;
                _attacks = new Dictionary<int, AttackDefinition>();
            }

            /// <summary>
            /// Adds an attack to a frame of the animation.
            /// </summary>
            public void addAttack(int frame, AttackDefinition attack)
            {
                _attacks.Add(frame, attack);
            }

            /// <summary>
            /// If this combo has an attack defined for a specific frame.
            /// </summary>
            public bool hasAttackForFrame(int frame)
            {
                return _attacks.Keys.Contains(frame);
            }

            /// <summary>
            /// Returns an attack for a specific frame.
            /// </summary>
            public AttackDefinition getAttackForFrame(int frame)
            {
                return _attacks[frame];
            }

            /// <summary>
            /// The animation for this combo.
            /// </summary>
            public Animation animation
            {
                get { return _animation; }
            }

            /// <summary>
            /// The auto movement in X direction.
            /// </summary>
            public float xMovement
            {
                get { return _movement.X; }
            }

            /// <summary>
            /// The auto movement in Y direction.
            /// </summary>
            public float yMovement
            {
                get { return _movement.Y; }
            }
        }

        /// <summary>
        /// An attack definition for a frame in an attack combo.
        /// </summary>
        protected struct AttackDefinition
        {
            private Attack _attack;
            private int _maxHits;

            public delegate void DAttackAction(AttackDefinition attack);
            private DAttackAction _attackAction;

            public AttackDefinition(Attack attack, int maxHits) : this(attack, maxHits, null) { }

            public AttackDefinition(Attack attack, int maxHits, DAttackAction attackAction)
            {
                _attack = attack;
                _maxHits = maxHits;
                _attackAction = attackAction;
            }

            /// <summary>
            /// The attack in this definition.
            /// </summary>
            public Attack attack
            {
                get { return _attack; }
            }

            /// <summary>
            /// The max amount of objects to be hit with this attack.
            /// </summary>
            public int maxHits
            {
                get { return _maxHits; }
            }

            /// <summary>
            /// Performs the attack's special action.
            /// </summary>
            public void useAttack()
            {
                if (_attackAction != null)
                    _attackAction(this);
            }
        }

        private Dictionary<string, PlayerAttack> _attacks = new Dictionary<string, PlayerAttack>();

        protected void addAttack(string comboChain, PlayerAttack combo)
        {
            _attacks.Add(comboChain, combo);
        }

        private string _nextAttackItem = "";
        private string _attackChain = ""; //The current combo chain.
        private double _attackDelay = 0d; //The time period after an attack to chain a combo.

        private int _comboChain = 0; //The amount of hits in this combo.
        private int _comboDelay = 0; //The amount of time until the combo resets.

        #endregion

        private PlayerIndex _playerIndex;
        private float _playerSpeed = 4f;
        private string _name = "";

        /// <summary>
        /// The speed of this player character.
        /// </summary>
        protected float playerSpeed
        {
            get { return _playerSpeed; }
            set { _playerSpeed = value; }
        }

        /// <summary>
        /// The name of this player character.
        /// </summary>
        public string name
        {
            get { return _name; }
        }

        /// <summary>
        /// The amount of hits in this combo.
        /// </summary>
        public int comboChain
        {
            get { return _comboChain; }
        }

        /// <summary>
        /// The amount of time until the combo resets.
        /// </summary>
        public int comboDelay
        {
            get { return _comboDelay; }
        }

        /// <summary>
        /// Creates a new instance of the player character class.
        /// </summary>
        protected PlayerCharacter(PlayerIndex playerIndex, string name)
        {
            _playerIndex = playerIndex;
            _name = name;
            canLandOn = false;

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
                if (_attacks[_attackChain].hasAttackForFrame(animationFrame))
                {
                    AttackDefinition def = _attacks[_attackChain].getAttackForFrame(animationFrame);

                    Attack attack = def.attack;

                    //If there's an attack defined, use it:
                    if (attack != null)
                    {
                        attack.facing = facing;
                        int hits = Stage.activeStage().applyAttack(attack, position, def.maxHits);

                        if (hits > 0)
                        {
                            _comboChain += hits;
                            _comboDelay = 100;
                        }
                    }

                    //Activate the attack def's special:
                    def.useAttack();
                }
            }

            updateCombo();

            base.update();
        }

        private void updateCombo()
        {
            if (_comboChain > 0 && _comboDelay > 0)
            {
                _comboDelay--;
                if (_comboDelay <= 0)
                {
                    _comboDelay = 0;
                    _comboChain = 0;
                }
            }
        }

        private void updateState()
        {
            ObjectState setToState = ObjectState.Idle;
            float groundY = Stage.activeStage().getGround(getFeetPosition());
            gravityAffected = true;

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

            if (state == ObjectState.Dashing)
            {
                getAnimation().frames[3].frameLength = 1;

                if (_autoMovement.X != 0f)
                {
                    if (animationFrame == 3)
                        animationFrame = 2;

                    setToState = ObjectState.Dashing;
                    gravityAffected = false;
                }
                else
                {
                    if (animationEnded())
                    {
                        repeatAnimation = true;
                    }
                    else
                    {
                        setToState = ObjectState.Dashing;
                    }
                }
            }

            if (state == ObjectState.JumpAttacking)
            {
                if (Y == groundY)
                {
                    repeatAnimation = true;
                }
                else
                {
                    setToState = ObjectState.JumpAttacking;
                }
            }

            //Jump attacking:
            if (setToState == ObjectState.Idle && Y > groundY && (state == ObjectState.Falling || state == ObjectState.Jumping))
            {
                if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.X) || Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.A))
                {
                    setToState = ObjectState.JumpAttacking;
                    repeatAnimation = false;
                    if (facing == ObjectFacing.Left)
                        _autoMovement.X -= 10f;
                    else
                        _autoMovement.X += 10f;

                    if (_autoMovement.Y < 5f)
                    {
                        _autoMovement.Y = 5f;
                    }
                }
            }

            //Attacking:
            if (setToState == ObjectState.Idle)
            {
                string comboAddition = "";
                if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.X) && comboAddition == "")
                    comboAddition = "B";
                if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.A) && comboAddition == "")
                    comboAddition = "A";

                if (state == ObjectState.Attacking && !animationEnded())
                {
                    setToState = ObjectState.Attacking;
                    repeatAnimation = false;
                    if (_nextAttackItem == "")
                        _nextAttackItem = comboAddition;
                }
                else if ((state == ObjectState.Attacking && animationEnded() && _attackDelay > 0) || state != ObjectState.Attacking)
                {
                    _attackDelay--;

                    if (_nextAttackItem != "" && comboAddition == "")
                    {
                        comboAddition = _nextAttackItem;
                        _nextAttackItem = "";
                    }

                    if (comboAddition != "" && _attacks.Keys.Contains(_attackChain + comboAddition))
                    {
                        _attackChain += comboAddition;
                        _attackDelay = 12d;

                        setToState = ObjectState.Attacking;
                        repeatAnimation = false;
                        animationFrame = 0;

                        PlayerAttack combo = _attacks[_attackChain];

                        if (facing == ObjectFacing.Left)
                            _autoMovement.X -= combo.xMovement;
                        else
                            _autoMovement.X += combo.xMovement;

                        _autoMovement.Y += combo.yMovement;
                    }
                    else
                    {
                        _attackChain = "";
                        _attackDelay = 0;
                        repeatAnimation = true;
                    }
                }
            }

            //Dashing in both directions:
            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.RightTrigger) && setToState == ObjectState.Idle)
            {
                setToState = ObjectState.Dashing;
                repeatAnimation = false;
                _autoMovement.X = 14;
                facing = ObjectFacing.Right;
            }
            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.LeftTrigger) && setToState == ObjectState.Idle)
            {
                setToState = ObjectState.Dashing;
                repeatAnimation = false;
                _autoMovement.X = -14;
                facing = ObjectFacing.Left;
            }

            //Jumping and landing:
            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.B) && setToState == ObjectState.Idle && Y == groundY)
            {
                _autoMovement.Y = 10f;
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
            if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftShoulder) && setToState == ObjectState.Idle && Y == groundY)
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

                    Vector3 desiredPosition = new Vector3(X + _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Right), Y, Z);

                    if (!Stage.activeStage().intersects(this, desiredPosition))
                        X = desiredPosition.X;

                    facing = ObjectFacing.Right;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickLeft))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    Vector3 desiredPosition = new Vector3(X - _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Left), Y, Z);

                    if (!Stage.activeStage().intersects(this, desiredPosition))
                        X = desiredPosition.X;

                    facing = ObjectFacing.Left;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickUp))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    Vector3 desiredPosition = new Vector3(X, Y, Z - _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Up));

                    if (!Stage.activeStage().intersects(this, desiredPosition))
                        Z = desiredPosition.Z;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickDown))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    Vector3 desiredPosition = new Vector3(X, Y, Z + _playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Down));

                    if (!Stage.activeStage().intersects(this, desiredPosition))
                        Z = desiredPosition.Z;
                }
            }

            //Falling:
            if ((setToState == ObjectState.Walking || setToState == ObjectState.Idle) && Y > groundY && gravityAffected)
            {
                setToState = ObjectState.Falling;
            }

            setState(setToState);
        }

        protected override Animation getAnimation()
        {
            if (state == ObjectState.Attacking)
            {
                return _attacks[_attackChain].animation;
            }

            return base.getAnimation();
        }
    }
}