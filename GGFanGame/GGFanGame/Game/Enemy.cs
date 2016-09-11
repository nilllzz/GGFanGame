using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game
{
    /// <summary>
    /// The base enemy class.
    /// </summary>
    abstract class Enemy : InteractableStageObject
    {
        public event Action<StageObject> OnDeath;
        
        public Enemy()
        { }

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
            ObjectState setToState = ObjectState.Idle;

            if (state == ObjectState.Dead)
            {
                setToState = ObjectState.Dead;
                if (animationEnded())
                {
                    canBeRemoved = true;

                    OnDeath?.Invoke(this);
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
    }
}