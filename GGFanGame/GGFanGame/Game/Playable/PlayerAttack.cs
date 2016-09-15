using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// Defines a single part of an attack chain.
    /// </summary>
    internal struct PlayerAttack
    {
        private readonly Dictionary<int, AttackDefinition> _attacks;
        private Vector2 _movement;

        public PlayerAttack(Animation animation, Vector2 movement)
        {
            this.animation = animation;
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
        public bool hasAttackForFrame(int frame) => _attacks.Keys.Contains(frame);

        /// <summary>
        /// Returns an attack for a specific frame.
        /// </summary>
        public AttackDefinition getAttackForFrame(int frame) => _attacks[frame];

        /// <summary>
        /// The animation for this combo.
        /// </summary>
        public Animation animation { get; private set; }

        /// <summary>
        /// The auto movement in X direction.
        /// </summary>
        public float xMovement => _movement.X;

        /// <summary>
        /// The auto movement in Y direction.
        /// </summary>
        public float yMovement => _movement.Y;
    }
}
