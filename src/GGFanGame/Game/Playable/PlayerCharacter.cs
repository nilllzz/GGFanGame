using System.Collections.Generic;
using System.Linq;
using GameDevCommon.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static Core;

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
        protected float PlayerSpeed { get; set; } = 0.0625f;

        /// <summary>
        /// The name of this player character.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The amount of hits in the active combo.
        /// </summary>
        public int ComboChain { get; private set; }

        /// <summary>
        /// The amount of time until the combo resets.
        /// </summary>
        public int ComboDelay { get; private set; }

        /// <summary>
        /// The amount of grump power filling the grump meter of this character.
        /// </summary>
        public int GrumpPower
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
                if (_grumpPower > MaxGrumpPower)
                    _grumpPower = MaxGrumpPower;
            }
        }

        /// <summary>
        /// The maximum amount of grump power.
        /// </summary>
        public abstract int MaxGrumpPower { get; }

        /// <summary>
        /// Creates a new instance of the player character class.
        /// </summary>
        protected PlayerCharacter(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
            CanLandOn = false;
            IsOpaque = false;

            ObjectColor = Drawing.Colors.GetColor(playerIndex);
        }

        protected void AddAttack(string comboChain, PlayerAttack combo)
        {
            _attacks.Add(comboChain, combo);
        }

        public override void Update()
        {
            UpdateState();

            // TEST
            if (GetComponent<GamePadHandler>().ButtonPressed(_playerIndex, Buttons.DPadLeft))
            {
                Health -= 10;
            }

            if (State == ObjectState.Attacking && GetAnimation().Frames[AnimationFrame].FrameLength == AnimationDelay)
            {
                if (_attacks[_attackChain].HasAttackForFrame(AnimationFrame))
                {
                    var def = _attacks[_attackChain].GetAttackForFrame(AnimationFrame);

                    var attack = def.Attack;

                    //If there's an attack defined, use it:
                    if (attack != null)
                    {
                        attack.Facing = Facing;
                        var hits = ParentStage.ApplyAttack(attack, Position, def.MaxHits);

                        if (hits > 0)
                        {
                            ComboChain += hits;
                            ComboDelay = 100;
                            GrumpPower += 1 + (int)(ComboChain / 3d);
                        }
                    }

                    //Activate the attack def's special:
                    def.UseAttack();
                }
            }

            UpdateCombo();

            base.Update();
        }

        private void UpdateCombo()
        {
            if (ComboChain > 0 && ComboDelay > 0)
            {
                ComboDelay--;
                if (ComboDelay <= 0)
                {
                    ComboDelay = 0;
                    ComboChain = 0;
                }
            }
        }

        private void UpdateState()
        {
            var setToState = ObjectState.Idle;
            var groundY = ParentStage.GetSupporting(this).objY;
            GravityAffected = true;

            if (State == ObjectState.Dead)
            {
                setToState = ObjectState.Dead;
            }

            if (State == ObjectState.HurtFalling)
            {
                if (AnimationEnded())
                    if (Health <= 0)
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

            if (State == ObjectState.Hurt)
            {
                if (AnimationEnded() && AutoMovement.X == 0f)
                {
                    RepeatAnimation = true;
                }
                else
                {
                    setToState = ObjectState.Hurt;
                }
            }

            if (State == ObjectState.StandingUp)
            {
                if (AnimationEnded())
                    RepeatAnimation = true;
                else
                    setToState = ObjectState.StandingUp;
            }

            if (State == ObjectState.Dashing)
            {
                GetAnimation().Frames[3].FrameLength = 1;

                if (AutoMovement.X != 0f)
                {
                    if (AnimationFrame == 3)
                        AnimationFrame = 2;

                    setToState = ObjectState.Dashing;
                    GravityAffected = false;
                }
                else
                {
                    if (AnimationEnded())
                    {
                        RepeatAnimation = true;
                    }
                    else
                    {
                        setToState = ObjectState.Dashing;
                    }
                }
            }

            if (State == ObjectState.JumpAttacking)
            {
                if (Y == groundY)
                {
                    RepeatAnimation = true;
                }
                else
                {
                    setToState = ObjectState.JumpAttacking;
                }
            }

            var gamePadHandler = GetComponent<GamePadHandler>();

            //Jump attacking:
            if (setToState == ObjectState.Idle && Y > groundY && (State == ObjectState.Falling || State == ObjectState.Jumping))
            {
                if (gamePadHandler.ButtonPressed(_playerIndex, Buttons.X) || gamePadHandler.ButtonPressed(_playerIndex, Buttons.A))
                {
                    setToState = ObjectState.JumpAttacking;
                    RepeatAnimation = false;
                    if (Facing == ObjectFacing.Left)
                        AutoMovement.X -= 10f;
                    else
                        AutoMovement.X += 10f;

                    if (AutoMovement.Y < 5f)
                    {
                        AutoMovement.Y = 5f;
                    }
                }
            }

            //Attacking:
            if (setToState == ObjectState.Idle)
            {
                var comboAddition = "";
                if (gamePadHandler.ButtonPressed(_playerIndex, Buttons.X) && comboAddition == "")
                    comboAddition = "B";
                if (gamePadHandler.ButtonPressed(_playerIndex, Buttons.A) && comboAddition == "")
                    comboAddition = "A";

                if (State == ObjectState.Attacking && !AnimationEnded())
                {
                    setToState = ObjectState.Attacking;
                    RepeatAnimation = false;
                    if (_nextAttackItem == "")
                        _nextAttackItem = comboAddition;
                }
                else if ((State == ObjectState.Attacking && AnimationEnded() && _attackDelay > 0) || State != ObjectState.Attacking)
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
                        RepeatAnimation = false;
                        AnimationFrame = 0;

                        var combo = _attacks[_attackChain];

                        if (Facing == ObjectFacing.Left)
                            AutoMovement.X -= combo.XMovement;
                        else
                            AutoMovement.X += combo.XMovement;

                        AutoMovement.Y += combo.YMovement;
                    }
                    else
                    {
                        _attackChain = "";
                        _attackDelay = 0;
                        RepeatAnimation = true;
                    }
                }
            }

            //Dashing in both directions:
            if (gamePadHandler.ButtonPressed(_playerIndex, Buttons.RightTrigger) && setToState == ObjectState.Idle)
            {
                setToState = ObjectState.Dashing;
                RepeatAnimation = false;
                AutoMovement.X = 14;
                Facing = ObjectFacing.Right;
            }
            if (gamePadHandler.ButtonPressed(_playerIndex, Buttons.LeftTrigger) && setToState == ObjectState.Idle)
            {
                setToState = ObjectState.Dashing;
                RepeatAnimation = false;
                AutoMovement.X = -14;
                Facing = ObjectFacing.Left;
            }

            //Jumping and landing:
            if (gamePadHandler.ButtonPressed(_playerIndex, Buttons.B) && setToState == ObjectState.Idle && Y == groundY)
            {
                AutoMovement.Y = 10f;
                setToState = ObjectState.Jumping;
            }
            if (State == ObjectState.Jumping && setToState == ObjectState.Idle)
            {
                if (AutoMovement.Y > 0f)
                {
                    setToState = ObjectState.Jumping;
                }
                else
                {
                    setToState = ObjectState.Falling;
                }
            }

            //Blocking:
            if (gamePadHandler.ButtonDown(_playerIndex, Buttons.LeftShoulder) && setToState == ObjectState.Idle && Y == groundY)
            {
                setToState = ObjectState.Blocking;
            }

            //Walking + movement while in the air:
            if ((setToState == ObjectState.Idle || setToState == ObjectState.Falling || setToState == ObjectState.Jumping) && AutoMovement.X == 0f)
            {
                if (gamePadHandler.ButtonDown(_playerIndex, Buttons.LeftThumbstickRight))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    var desiredPosition = new Vector3(X + PlayerSpeed * gamePadHandler.GetThumbStickDirection(_playerIndex, ThumbStick.Left, InputDirection.Right), Y, Z);

                    if (!ParentStage.Intersects(this, desiredPosition))
                        X = desiredPosition.X;

                    Facing = ObjectFacing.Right;
                }
                if (gamePadHandler.ButtonDown(_playerIndex, Buttons.LeftThumbstickLeft))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    var desiredPosition = new Vector3(X - PlayerSpeed * gamePadHandler.GetThumbStickDirection(_playerIndex, ThumbStick.Left, InputDirection.Left), Y, Z);

                    if (!ParentStage.Intersects(this, desiredPosition))
                        X = desiredPosition.X;

                    Facing = ObjectFacing.Left;
                }
                if (gamePadHandler.ButtonDown(_playerIndex, Buttons.LeftThumbstickUp))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    var desiredPosition = new Vector3(X, Y, Z - PlayerSpeed * gamePadHandler.GetThumbStickDirection(_playerIndex, ThumbStick.Left, InputDirection.Up));

                    if (!ParentStage.Intersects(this, desiredPosition))
                        Z = desiredPosition.Z;
                }
                if (gamePadHandler.ButtonDown(_playerIndex, Buttons.LeftThumbstickDown))
                {
                    if (setToState == ObjectState.Idle)
                        setToState = ObjectState.Walking;

                    var desiredPosition = new Vector3(X, Y, Z + PlayerSpeed * gamePadHandler.GetThumbStickDirection(_playerIndex, ThumbStick.Left, InputDirection.Down));

                    if (!ParentStage.Intersects(this, desiredPosition))
                        Z = desiredPosition.Z;
                }
            }

            //Falling:
            if ((setToState == ObjectState.Walking || setToState == ObjectState.Idle) && Y > groundY && GravityAffected)
            {
                setToState = ObjectState.Falling;
            }

            SetState(setToState);
        }

        protected override void CreateWorld()
        {
            SetWorld(Position + new Vector3(0, 0.5f, 0));
        }

        protected override Animation GetAnimation()
        {
            if (State == ObjectState.Attacking)
            {
                return _attacks[_attackChain].Animation;
            }

            return base.GetAnimation();
        }

        internal void KilledEnemy(Enemy enemy)
        {
            _playerStatistics.Kills++;
            _playerStatistics.Score += enemy.Score;
        }
    }
}
