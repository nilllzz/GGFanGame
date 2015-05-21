using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GGFanGame.Input
{
    /// <summary>
    /// Handles GamePad input.
    /// </summary>
    class GamePadHandler
    {
        private static GamePadState _oldState;
        private static GamePadState _currentState;

        /// <summary>
        /// Updates the GamePadHandler's states.
        /// </summary>
        public static void update()
        {
            _oldState = _currentState;
            _currentState = GamePad.GetState(PlayerIndex.One); //We are using player one right now. If we need more players, we need to change this.
        }

        /// <summary>
        /// Returns if a specific button is pressed.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool buttonPressed(Buttons button)
        {
            return (!_oldState.IsButtonDown(button) && _currentState.IsButtonDown(button));
        }

        /// <summary>
        /// Returns is a button is currently being held down.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool buttonDown(Buttons button)
        {
            return _currentState.IsButtonDown(button);
        }
    }
}