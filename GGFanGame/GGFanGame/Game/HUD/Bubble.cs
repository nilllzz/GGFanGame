using System;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.HUD
{
    /// <summary>
    /// Class to represent the bubbles in the HUD.
    /// </summary>
    internal class Bubble
    {
        public Vector2 position;
        public float size;

        private bool 
            _sinking = true, 
            _growing = true;

        private readonly Random _rnd;

        public Bubble(Vector2 position, float size, int seed)
        {
            this.position = position;
            this.size = size;

            _rnd = new Random(seed);

            if (size > 13)
                _growing = false;

            if (position.X > 80)
                _sinking = false;
        }

        /// <summary>
        /// Update size/position.
        /// </summary>
        public void Update()
        {
            if (_growing)
            {
                size += 0.75f;
                if (size >= 45)
                    _growing = false;
            }
            else
            {
                size -= 0.75f;
                if (size <= 15)
                    _growing = true;
            }

            if (_sinking)
            {
                position.Y += 0.1f;
                if (position.Y >= -2f)
                    _sinking = false;
            }
            else
            {
                position.Y -= 0.1f;
                if (position.Y <= -15f)
                    _sinking = true;
            }
        }
    }
}
