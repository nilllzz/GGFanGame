using System;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.HUD
{
    /// <summary>
    /// Class to represent the bubbles in the HUD.
    /// </summary>
    internal class Bubble
    {
        private readonly Random _rnd;

        private bool
            _sinking = true,
            _growing = true;

        public Vector2 Position;
        public float Size;

        public Bubble(Vector2 position, float size, int seed)
        {
            Position = position;
            Size = size;
            
             _rnd = new Random(seed);

            if (size > 13)
                _growing = false;

            if (position.X > 80)
                _sinking = false;
        }

        /// <summary>
        /// Update size/position.
        /// </summary>
        public void Update(float timeDelta)
        {
            if (_growing)
            {
                Size += 0.75f * timeDelta;
                if (Size >= 45)
                    _growing = false;
            }
            else
            {
                Size -= 0.75f * timeDelta;
                if (Size <= 15)
                    _growing = true;
            }

            if (_sinking)
            {
                Position.Y += 0.1f * timeDelta;
                if (Position.Y >= -2f)
                    _sinking = false;
            }
            else
            {
                Position.Y -= 0.1f * timeDelta;
                if (Position.Y <= -15f)
                    _sinking = true;
            }
        }
    }
}
