using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Input
{
    /// <summary>
    /// Handles keyboard input.
    /// </summary>
    internal static class KeyboardHandler
    {
        private static KeyboardState _oldState;
        private static KeyboardState _currentState;

        /// <summary>
        /// Updates the KeyboardHandler's states.
        /// </summary>
        public static void Update()
        {
            _oldState = _currentState;
            _currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Returns if a specific key is pressed.
        /// </summary>
        public static bool KeyPressed(Keys key)
            => (!_oldState.IsKeyDown(key) && _currentState.IsKeyDown(key));

        /// <summary>
        /// Returns if a specific key is being held down.
        /// </summary>
        public static bool KeyDown(Keys key)
            => _currentState.IsKeyDown(key);

        /// <summary>
        /// Returns all keys that pressed right now.
        /// </summary>
        public static Keys[] GetPressedKeys()
            => _currentState.GetPressedKeys();
    }
}