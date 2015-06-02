using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GGFanGame.Game.Level;

namespace GGFanGame.Game
{
    class Attack
    {
        private StageObject _origin;
        private bool _knockback;
        private int _health;
        private Vector3 _size;
        private Vector3 _offset;
        private ObjectFacing _facing;

        public Attack(StageObject origin, bool knockback, int health, Vector3 size, Vector3 offset) :
            this(origin, knockback, health, size, offset, ObjectFacing.Left) { }

        public Attack(StageObject origin, bool knockback, int health, Vector3 size, Vector3 offset, ObjectFacing facing)
        {
            _origin = origin;
            _knockback = knockback;
            _health = health;
            _size = size;
            _offset = offset;
            _facing = facing;
        }

        public StageObject origin
        {
            get { return _origin; }
        }

        public bool knockback
        {
            get { return _knockback; }
        }

        public int health
        {
            get { return _health; }
        }

        public ObjectFacing facing
        {
            get { return _facing; }
            set { _facing = value; }
        }

        /// <summary>
        /// Returns the hitbox of this attack relativ to the position of the user.
        /// </summary>
        /// <param name="relPosition">The position of the user of this attack.</param>
        /// <returns></returns>
        public BoundingBox getHitbox(Vector3 relPosition)
        {
            float X = relPosition.X;
            float Y = relPosition.Y;
            float Z = relPosition.Z;

            float xOffset = (_facing == ObjectFacing.Right) ? _offset.X : -_offset.X;

            BoundingBox hitbox = new BoundingBox(
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