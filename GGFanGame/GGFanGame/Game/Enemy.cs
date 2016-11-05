using System;
using GGFanGame.Game.Playable;

namespace GGFanGame.Game
{
    /// <summary>
    /// The base enemy class.
    /// </summary>
    internal abstract class Enemy : InteractableStageObject
    {
        public event Action<StageObject> OnDeath;

        /// <summary>
        /// The score a player gets when killing this enemy.
        /// </summary>
        public abstract int Score { get; }
        
        public override void Update()
        {
            UpdateState();

            base.Update();
        }

        /// <summary>
        /// Updates the states of this enemy.
        /// </summary>
        private void UpdateState()
        {
            var setToState = ObjectState.Idle;

            if (State == ObjectState.Dead)
            {
                setToState = ObjectState.Dead;
                if (AnimationEnded())
                {
                    Die();
                }
            }

            if (State == ObjectState.Hurt)
            {
                if (AnimationEnded())
                {
                    if (Health <= 0)
                        setToState = ObjectState.Dead;
                    else
                        RepeatAnimation = true;
                }
                else
                {
                    setToState = ObjectState.Hurt;
                }
            }
            if (State == ObjectState.HurtFalling)
            {
                if (AnimationEnded())
                {
                    if (Health <= 0)
                        setToState = ObjectState.Dead;
                    else
                        RepeatAnimation = true;
                }
                else
                {
                    setToState = ObjectState.HurtFalling;
                }
            }

            SetState(setToState);
        }

        private void Die()
        {
            if (LastAttackedBy != null && LastAttackedBy is PlayerCharacter)
                (LastAttackedBy as PlayerCharacter).KilledEnemy(this);

            CanBeRemoved = true;

            OnDeath?.Invoke(this);
        }
    }
}