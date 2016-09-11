using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    /// <summary>
    /// Represents an attack used by an enemy or player.
    /// </summary>
    internal class Attack
    {
        private Vector3 _size, _offset;

        public Attack(StageObject origin, bool knockback, int health, float strength, Vector3 size, Vector3 offset, ObjectFacing facing = ObjectFacing.Left)
        {
            this.origin = origin;
            this.knockback = knockback;
            this.health = health;
            this.facing = facing;
            this.strength = strength;

            _size = size;
            _offset = offset;
        }

        /// <summary>
        /// The origin object of this attack.
        /// </summary>
        public StageObject origin { get; private set; }

        /// <summary>
        /// If this attack has a strong knockback that hits to the ground.
        /// </summary>
        public bool knockback { get; private set; }

        /// <summary>
        /// The health this attack depletes.
        /// </summary>
        public int health { get; private set; }

        /// <summary>
        /// The strength of this attack.
        /// </summary>
        public float strength { get; private set; }

        /// <summary>
        /// When an object gets hit, this is the facing to set to.
        /// </summary>
        public ObjectFacing facing { get; set; }

        /// <summary>
        /// Returns the hitbox of this attack relativ to the position of the user.
        /// </summary>
        /// <param name="relPosition">The position of the user of this attack.</param>
        public BoundingBox getHitbox(Vector3 relPosition)
        {
            var X = relPosition.X;
            var Y = relPosition.Y;
            var Z = relPosition.Z;

            var xOffset = (facing == ObjectFacing.Right) ? _offset.X : -_offset.X;

            var hitbox = new BoundingBox(
                  new Vector3(X - _size.X / 2f + xOffset,
                              Y - _size.Y / 2f + _offset.Y,
                              Z - _size.Z / 2f + _offset.Z),
                  new Vector3(X + _size.X / 2f + xOffset,
                              Y + _size.Y / 2f + _offset.Y,
                              Z + _size.Z / 2f + _offset.Z));

            return hitbox;
        }
    }
}