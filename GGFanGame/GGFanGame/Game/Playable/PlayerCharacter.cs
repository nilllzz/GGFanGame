using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// A playable character.
    /// </summary>
    internal abstract class PlayerCharacter : InteractableStageObject
    {
        private readonly Dictionary<string, PlayerAttack> _attacks = new Dictionary<string, PlayerAttack>();
        private string _nextAttackItem = "";
        private string _attackChain = ""; //The current combo chain.
        private double _attackDelay = 0d; //The time period after an attack to chain a combo.
        private readonly PlayerIndex _playerIndex;
        private int _grumpPower = 0;
        private PlayerStatistics _playerStatistics;

        /// <summary>
        /// The speed of this player character.
        /// </summary>
        protected float playerSpeed { get; set; } = 4f;

        /// <summary>
        /// The name of this player character.
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The amount of hits in the active combo.
        /// </summary>
        public int comboChain { get; private set; }

        /// <summary>
        /// The amount of time until the combo resets.
        /// </summary>
        public int comboDelay { get; private set; }

        /// <summary>
        /// The amount of grump power filling the grump meter of this character.
        /// </summary>
        public int grumpPower
        {
            get
            {
                return _grumpPower;
            }
            set
            {
                _grumpPower = value;
                if (_grumpPower < 0)
                    _grumpPower = 0;
                if (_grumpPower > maxGrumpPower)
                    _grumpPower = maxGrumpPower;
            }
        }

        /// <summary>
        /// The maximum amount of grump power.
        /// </summary>
        public abstract int maxGrumpPower { get; }
        
        /// <summary>
        /// Creates a new instance of the player character class.
        /// </summary>
        protected PlayerCharacter(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
            canLandOn = false;

            objectColor = Drawing.Colors.getColor(playerIndex);
        }

        protected void addAttack(string comboChain, PlayerAttack combo)
        {
            _attacks.Add(comboChain, combo);
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

            // TEST
            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.DPadLeft))
            {
                health -= 10;
            }

            if (state == ObjectState.Attacking && getAnimation().frames[animationFrame].frameLength == animationDelay)
            {
                if (_attacks[_attackChain].hasAttackForFrame(animationFrame))
                {
                    var def = _attacks[_attackChain].getAttackForFrame(animationFrame);

                    var attack = def.attack;

                    //If there's an attack defined, use it:
                    if (attack != null)
                    {
                        attack.facing = facing;
                        var hits = Stage.activeStage.applyAttack(attack, position, def.maxHits);

                        if (hits > 0)
                        {
                            comboChain += hits;
                            comboDelay = 100;
                            grumpPower += 1 + (int)(comboChain / 3d);
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
            if (comboChain > 0 && comboDelay > 0)
            {
                comboDelay--;
                if (comboDelay <= 0)
                {
                    comboDelay = 0;
                    comboChain = 0;
                }
            }
        }

        private void updateState()
        {
            var setToState = ObjectState.Idle;
            var groundY = Stage.activeStage.getGround(getFeetPosition());
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
                if (animationEnded() && autoMovement.X == 0f)
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

                if (autoMovement.X != 0f)
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
                        autoMovement.X -= 10f;
                    else
                        autoMovement.X += 10f;

                    if (autoMovement.Y < 5f)
                    {
                        autoMovement.Y = 5f;
                    }
                }
            }

            //Attacking:
            if (setToState == ObjectState.Idle)
            {
                var comboAddition = "";
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

                        var combo = _attacks[_attackChain];

                        if (facing == ObjectFacing.Left)
                            autoMovement.X -= combo.xMovement;
                        else
                            autoMovement.X += combo.xMovement;

                        autoMovement.Y += combo.yMovement;
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
                autoMovement.X = 14;
                facing = ObjectFacing.Right;
            }
            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.LeftTrigger) && setToState == ObjectState.Idle)
            {
                setToState = ObjectState.Dashing;
                repeatAnimation = false;
                autoMovement.X = -14;
                facing = ObjectFacing.Left;
            }

            //Jumping and landing:
            if (Input.GamePadHandler.buttonPressed(_playerIndex, Buttons.B) && setToState == ObjectState.Idle && Y == groundY)
            {
                autoMovement.Y = 10f;
                setToState = ObjectState.Jumping;
            }
            if (state == ObjectState.Jumping && setToState == ObjectState.Idle)
            {
                if (autoMovement.Y > 0f)
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
            if ((setToState == ObjectState.Idle || setToState == ObjectState.Falling || setToState == ObjectState.Jumping) && autoMovement.X == 0f)
            {
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickRight))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    var desiredPosition = new Vector3(X + playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Right), Y, Z);

                    if (!Stage.activeStage.intersects(this, desiredPosition))
                        X = desiredPosition.X;

                    facing = ObjectFacing.Right;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickLeft))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    var desiredPosition = new Vector3(X - playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Left), Y, Z);

                    if (!Stage.activeStage.intersects(this, desiredPosition))
                        X = desiredPosition.X;

                    facing = ObjectFacing.Left;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickUp))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    var desiredPosition = new Vector3(X, Y, Z - playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Up));

                    if (!Stage.activeStage.intersects(this, desiredPosition))
                        Z = desiredPosition.Z;
                }
                if (Input.GamePadHandler.buttonDown(_playerIndex, Buttons.LeftThumbstickDown))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    var desiredPosition = new Vector3(X, Y, Z + playerSpeed * Input.GamePadHandler.thumbStickDirection(_playerIndex, Input.ThumbStick.Left, Input.InputDirection.Down));

                    if (!Stage.activeStage.intersects(this, desiredPosition))
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

        internal void killedEnemy(Enemy enemy)
        {
            _playerStatistics.kills++;
            _playerStatistics.score += enemy.score;
        }
    }
}