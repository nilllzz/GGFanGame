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
        public static void update()
        {
            _oldState = _currentState;
            _currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Returns if a specific key is pressed.
        /// </summary>
        public static bool keyPressed(Keys key)
        {
            return (!_oldState.IsKeyDown(key) && _currentState.IsKeyDown(key));
        }

        /// <summary>
        /// Returns if a specific key is being held down.
        /// </summary>
        public static bool keyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns all keys that pressed right now.
        /// </summary>
        public static Keys[] getPressedKeys()
        {
            return _currentState.GetPressedKeys();
        }
    }
}