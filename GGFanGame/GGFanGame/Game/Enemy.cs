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
        public abstract int score { get; }

        public override void update()
        {
            updateState();

            base.update();
        }

        /// <summary>
        /// Updates the states of this enemy.
        /// </summary>
        private void updateState()
        {
            var setToState = ObjectState.Idle;

            if (state == ObjectState.Dead)
            {
                setToState = ObjectState.Dead;
                if (animationEnded())
                {
                    die();
                }
            }

            if (state == ObjectState.Hurt)
            {
                if (animationEnded())
                {
                    if (health <= 0)
                        setToState = ObjectState.Dead;
                    else
                        repeatAnimation = true;
                }
                else
                {
                    setToState = ObjectState.Hurt;
                }
            }
            if (state == ObjectState.HurtFalling)
            {
                if (animationEnded())
                {
                    if (health <= 0)
                        setToState = ObjectState.Dead;
                    else
                        repeatAnimation = true;
                }
                else
                {
                    setToState = ObjectState.HurtFalling;
                }
            }

            setState(setToState);
        }

        private void die()
        {
            if (lastAttackedBy != null && lastAttackedBy is PlayerCharacter)
                (lastAttackedBy as PlayerCharacter).killedEnemy(this);

            canBeRemoved = true;

            OnDeath?.Invoke(this);
        }
    }
}