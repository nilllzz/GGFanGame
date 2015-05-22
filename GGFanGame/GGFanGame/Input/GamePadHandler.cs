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
        private static GamePadState[] _oldStates = new GamePadState[4];
        private static GamePadState[] _currentStates = new GamePadState[4];

        /// <summary>
        /// Updates the GamePadHandler's states.
        /// </summary>
        public static void update()
        {
            for (int i = 0; i < _oldStates.Length; i++)
            {
                _oldStates[i] = _currentStates[i];
            }

            _currentStates[0] = GamePad.GetState(PlayerIndex.One);
            _currentStates[1] = GamePad.GetState(PlayerIndex.Two);
            _currentStates[2] = GamePad.GetState(PlayerIndex.Three);
            _currentStates[3] = GamePad.GetState(PlayerIndex.Four);
        }

        /// <summary>
        /// Returns if a specific button on the first GamePad is pressed.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool buttonPressed(Buttons button)
        {
            return buttonPressed(button, PlayerIndex.One);
        }

        /// <summary>
        /// Returns if a specific button on a GamePad is pressed.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public static bool buttonPressed(Buttons button, PlayerIndex playerIndex)
        {
            int index = (int)playerIndex;
            return (!_oldStates[index].IsButtonDown(button) && _currentStates[index].IsButtonDown(button));
        }

        /// <summary>
        /// Returns is a button is currently being held down on the first GamePad.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool buttonDown(Buttons button)
        {
            return buttonDown(button, PlayerIndex.One);
        }

        /// <summary>
        /// Returns is a button is currently being held down on a GamePad.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public static bool buttonDown(Buttons button, PlayerIndex playerIndex)
        {
            int index = (int)playerIndex;
            return _currentStates[index].IsButtonDown(button);
        }
    }
}