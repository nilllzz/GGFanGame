using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GGFanGame.Input
{
    /// <summary>
    /// Handles keyboard input.
    /// </summary>
    class KeyboardHandler
    {
        private static KeyboardState _oldState;
        private static KeyboardState _currentState;

        /// <summary>
        /// Updates the KeyboardHandler's states.
        /// </summary>
        public static void update()
        {
            _oldState = _currentState;
            _currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Returns if a specific key is pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool keyPressed(Keys key)
        {
            return (!_oldState.IsKeyDown(key) && _currentState.IsKeyDown(key));
        }

        /// <summary>
        /// Returns if a specific key is being held down.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool keyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns all keys that pressed right now.
        /// </summary>
        /// <returns></returns>
        public static Keys[] getPressedKeys()
        {
            return _currentState.GetPressedKeys();
        }
    }
}