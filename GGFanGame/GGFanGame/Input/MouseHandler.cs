using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GGFanGame.Input
{
    /// <summary>
    /// Handles mouse input.
    /// </summary>
    internal sealed class MouseHandler : IGameComponent
    {
        void IGameComponent.Initialize() { }

        private MouseState _oldState, _currentState;

        /// <summary>
        /// Updates the MouseHandler's states.
        /// </summary>
        internal void Update()
        {
            _oldState = _currentState;
            _currentState = Mouse.GetState();
        }

        /// <summary>
        /// Returns if a specific mouse button is pressed.
        /// </summary>
        internal bool ButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (_oldState.LeftButton == ButtonState.Released && _currentState.LeftButton == ButtonState.Pressed);
                case MouseButton.Right:
                    return (_oldState.RightButton == ButtonState.Released && _currentState.RightButton == ButtonState.Pressed);
                case MouseButton.Middle:
                    return (_oldState.MiddleButton == ButtonState.Released && _currentState.MiddleButton == ButtonState.Pressed);
            }
            return false;
        }

        /// <summary>
        /// Returns if a specific mouse button is being held down.
        /// </summary>
        internal bool ButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (_currentState.LeftButton == ButtonState.Pressed);
                case MouseButton.Right:
                    return (_currentState.RightButton == ButtonState.Pressed);
                case MouseButton.Middle:
                    return (_currentState.MiddleButton == ButtonState.Pressed);
            }
            return false;
        }

        /// <summary>
        /// Returns the position of the mouse in the window.
        /// </summary>
        internal Point MousePosition()
            => new Point(_currentState.X, _currentState.Y);
    }
}