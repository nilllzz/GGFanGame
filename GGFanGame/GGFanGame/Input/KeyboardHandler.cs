﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Input
{
    /// <summary>
    /// Handles keyboard input.
    /// </summary>
    internal sealed class KeyboardHandler : IGameComponent
    {
        void IGameComponent.Initialize() { }

        private KeyboardState _oldState, _currentState;

        /// <summary>
        /// Updates the KeyboardHandler's states.
        /// </summary>
        internal void Update()
        {
            _oldState = _currentState;
            _currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Returns if a specific key is pressed.
        /// </summary>
        internal bool KeyPressed(Keys key)
            => (!_oldState.IsKeyDown(key) && _currentState.IsKeyDown(key));

        /// <summary>
        /// Returns if a specific key is being held down.
        /// </summary>
        internal bool KeyDown(Keys key)
            => _currentState.IsKeyDown(key);

        /// <summary>
        /// Returns all keys that pressed right now.
        /// </summary>
        internal Keys[] GetPressedKeys()
            => _currentState.GetPressedKeys();
    }
}
