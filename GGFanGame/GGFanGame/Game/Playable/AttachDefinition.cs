using System;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// An attack definition for a frame in an attack combo.
    /// </summary>
    internal struct AttackDefinition
    {
        private readonly Action<AttackDefinition> _attackAction;

        public AttackDefinition(Attack attack, int maxHits, Action<AttackDefinition> attackAction = null)
        {
            Attack = attack;
            MaxHits = maxHits;
            _attackAction = attackAction;
        }

        /// <summary>
        /// The attack in this definition.
        /// </summary>
        public Attack Attack { get; }

        /// <summary>
        /// The max amount of objects to be hit with this attack.
        /// </summary>
        public int MaxHits { get; }

        /// <summary>
        /// Performs the attack's special action.
        /// </summary>
        public void UseAttack()
        {
            _attackAction?.Invoke(this);
        }
    }
}
