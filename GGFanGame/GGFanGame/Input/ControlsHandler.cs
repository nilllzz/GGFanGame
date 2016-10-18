using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GGFanGame.Input
{
    /// <summary>
    /// A class to handle all input methods.
    /// </summary>
    internal static class ControlsHandler
    {
        /// <summary>
        /// Updates all input handlers.
        /// </summary>
        public static void Update()
        {
            KeyboardHandler.Update();
            GamePadHandler.Update();
            MouseHandler.Update();
        }

        /*
        This keeps track of the direction that was last pressed.
        That is important since we want to have a feature where you can hold down a direction,
        and it starts to "press" that direction button at an interval (once per 3 frames).
        The pressedKeyDelay is set to wait for 40 frames until the 3 frame interval starts.
        */

        //We have 4 element-long arrays because there can be four players connected. We initialize them with the standard values.
        private static readonly InputDirection[] _lastPressedDirection = { InputDirection.None, InputDirection.None, InputDirection.None, InputDirection.None };
        private static readonly float[] _pressedKeyDelay = { 0f, 0f, 0f, 0f };

        /// <summary>
        /// Resets when no direction is pressed.
        /// </summary>
        private static void ResetDirectionPressed(PlayerIndex playerIndex, InputDirection direction)
        {
            var i = (int)playerIndex;
            if (direction == _lastPressedDirection[i])
            {
                _pressedKeyDelay[i] = 4f;
                _lastPressedDirection[i] = InputDirection.None;
            }
        }

        /// <summary>
        /// When a different direction is pressed, then reset the wait time to 40 frames.
        /// </summary>
        private static void ChangeDirectionPressed(PlayerIndex playerIndex, InputDirection direction)
        {
            var i = (int)playerIndex;
            if (_lastPressedDirection[i] != direction)
            {
                _pressedKeyDelay[i] = 4f;
                _lastPressedDirection[i] = direction;
            }
        }

        /// <summary>
        /// Checks, if the hold-down-pressed feature is active and updates it.
        /// </summary>
        private static bool HoldDownPressed(PlayerIndex playerIndex, InputDirection direction)
        {
            var i = (int)playerIndex;
            if (_lastPressedDirection[i] == direction)
            {
                _pressedKeyDelay[i] -= 0.1f;
                if (_pressedKeyDelay[i] <= 0f)
                {
                    _pressedKeyDelay[i] = 0.3f;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a direction is either pressed or down.
        /// </summary>
        private static bool CheckDirectional(bool pressed, PlayerIndex playerIndex, InputDirection direction, Keys WASDKey, Keys arrowKey, Buttons thumbStickDirection, Buttons dPadDirection, InputDirectionType[] inputTypes)
        {
            if (pressed)
                return CheckDirectionalPress(playerIndex, direction, WASDKey, arrowKey, thumbStickDirection, dPadDirection, inputTypes);
            else
                return CheckDirectionalDown(playerIndex, direction, WASDKey, arrowKey, thumbStickDirection, dPadDirection, inputTypes);
        }

        /// <summary>
        /// Checks if any of the given directions are pressed.
        /// </summary>
        /// <param name="inputTypes">All input types to check.</param>
        private static bool CheckDirectionalPress(PlayerIndex playerIndex, InputDirection direction, Keys WASDKey, Keys arrowKey, Buttons thumbStickDirection, Buttons dPadDirection, InputDirectionType[] inputTypes)
        {
            //This keeps track if any direction key has been pressed. If not, it will reset the delay at the end.
            var hasInputDirection = false;

            var checkForAll = inputTypes.Contains(InputDirectionType.All);

            //The keyboard is always assigned to player one.
            //This might be changed later, if it does, we need to replace the PlayerIndex.One to the designated player.

            //Check for WASD keys.
            if ((inputTypes.Contains(InputDirectionType.WASD) || checkForAll) && playerIndex == PlayerIndex.One)
            {
                if (KeyboardHandler.KeyDown(WASDKey))
                {
                    hasInputDirection = true;
                    if (HoldDownPressed(PlayerIndex.One, direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (KeyboardHandler.KeyPressed(WASDKey))
                        {
                            ChangeDirectionPressed(PlayerIndex.One, direction);
                            return true;
                        }
                    }
                }
            }

            //Check for arrow keys.
            if ((inputTypes.Contains(InputDirectionType.ArrowKeys) || checkForAll) && playerIndex == PlayerIndex.One)
            {
                if (KeyboardHandler.KeyDown(arrowKey))
                {
                    hasInputDirection = true;
                    if (HoldDownPressed(PlayerIndex.One, direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (KeyboardHandler.KeyPressed(arrowKey))
                        {
                            ChangeDirectionPressed(PlayerIndex.One, direction);
                            return true;
                        }
                    }
                }
            }

            //Check for the left thumbstick.
            if (inputTypes.Contains(InputDirectionType.ThumbStick) || checkForAll)
            {
                if (GamePadHandler.ButtonDown(playerIndex, thumbStickDirection))
                {
                    hasInputDirection = true;
                    if (HoldDownPressed(playerIndex, direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (GamePadHandler.ButtonPressed(playerIndex, thumbStickDirection))
                        {
                            ChangeDirectionPressed(playerIndex, direction);
                            return true;
                        }
                    }
                }
            }

            //Check for the dpad.
            if (inputTypes.Contains(InputDirectionType.DPad) || checkForAll)
            {
                if (GamePadHandler.ButtonDown(playerIndex, dPadDirection))
                {
                    hasInputDirection = true;
                    if (HoldDownPressed(playerIndex, direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (GamePadHandler.ButtonPressed(playerIndex, dPadDirection))
                        {
                            ChangeDirectionPressed(playerIndex, direction);
                            return true;
                        }
                    }
                }
            }

            //When no direction button was pressed, reset and return false.
            if (!hasInputDirection)
            {
                ResetDirectionPressed(playerIndex, direction);
            }

            return false;
        }

        /// <summary>
        /// Checks if any of the given directions are down.
        /// </summary>
        /// <param name="inputTypes">All input types to check.</param>
        private static bool CheckDirectionalDown(PlayerIndex playerIndex, InputDirection direction, Keys WASDKey, Keys arrowKey, Buttons thumbStickDirection, Buttons dPadDirection, InputDirectionType[] inputTypes)
        {
            //When we should check for any input type, check if the handler's inputDown is pressed.

            var checkForAll = inputTypes.Contains(InputDirectionType.All);

            if ((inputTypes.Contains(InputDirectionType.WASD) || checkForAll) && playerIndex == PlayerIndex.One)
            {
                if (KeyboardHandler.KeyDown(WASDKey))
                {
                    return true;
                }
            }
            if ((inputTypes.Contains(InputDirectionType.ArrowKeys) || checkForAll) && playerIndex == PlayerIndex.One)
            {
                if (KeyboardHandler.KeyDown(arrowKey))
                {
                    return true;
                }
            }
            if (inputTypes.Contains(InputDirectionType.ThumbStick) || checkForAll)
            {
                if (GamePadHandler.ButtonDown(playerIndex, thumbStickDirection))
                {
                    return true;
                }
            }
            if (inputTypes.Contains(InputDirectionType.DPad) || checkForAll)
            {
                if (GamePadHandler.ButtonDown(playerIndex, dPadDirection))
                {
                    return true;
                }
            }

            //No handler has the button down? return false.
            return false;
        }

        #region Internal direction checking

        //These methods check the four directions:

        private static bool InternalLeft(bool pressed, PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return CheckDirectional(pressed, playerIndex, InputDirection.Left, Keys.A, Keys.Left, Buttons.LeftThumbstickLeft, Buttons.DPadLeft, inputTypes);
        }

        private static bool InternalRight(bool pressed, PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return CheckDirectional(pressed, playerIndex, InputDirection.Right, Keys.D, Keys.Right, Buttons.LeftThumbstickRight, Buttons.DPadRight, inputTypes);
        }

        private static bool InternalUp(bool pressed, PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return CheckDirectional(pressed, playerIndex, InputDirection.Up, Keys.W, Keys.Up, Buttons.LeftThumbstickUp, Buttons.DPadUp, inputTypes);
        }

        private static bool InternalDown(bool pressed, PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return CheckDirectional(pressed, playerIndex, InputDirection.Down, Keys.S, Keys.Down, Buttons.LeftThumbstickDown, Buttons.DPadDown, inputTypes);
        }

        #endregion

        #region public interface

        //These methods are the public access to the internal check methods:

        /// <summary>
        /// Checks if up left are down.
        /// </summary>
        public static bool LeftDown(PlayerIndex playerIndex)
        {
            return LeftDown(playerIndex, new InputDirectionType[] { InputDirectionType.All });
        }

        /// <summary>
        /// Checks if left controls are down.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        public static bool LeftDown(PlayerIndex playerIndex, params InputDirectionType[] inputTypes)
        {
            return InternalLeft(false, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if up left are pressed.
        /// </summary>
        public static bool LeftPressed(PlayerIndex playerIndex)
        {
            return LeftPressed(playerIndex, new InputDirectionType[] { InputDirectionType.All });
        }

        /// <summary>
        /// Checks if left controls are pressed.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        public static bool LeftPressed(PlayerIndex playerIndex, params InputDirectionType[] inputTypes)
        {
            return InternalLeft(true, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if right controls are down.
        /// </summary>
        public static bool RightDown(PlayerIndex playerIndex)
        {
            return RightDown(playerIndex, new InputDirectionType[] { InputDirectionType.All });
        }

        /// <summary>
        /// Checks if right controls are down.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        public static bool RightDown(PlayerIndex playerIndex, params InputDirectionType[] inputTypes)
        {
            return InternalRight(false, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if up controls are pressed.
        /// </summary>
        public static bool RightPressed(PlayerIndex playerIndex)
        {
            return RightPressed(playerIndex, new InputDirectionType[] { InputDirectionType.All });
        }

        /// <summary>
        /// Checks if right controls are pressed.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        public static bool RightPressed(PlayerIndex playerIndex, params InputDirectionType[] inputTypes)
        {
            return InternalRight(true, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if up controls are down.
        /// </summary>
        public static bool UpDown(PlayerIndex playerIndex)
        {
            return UpDown(playerIndex, new InputDirectionType[] { InputDirectionType.All });
        }

        /// <summary>
        /// Checks if up controls are down.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        public static bool UpDown(PlayerIndex playerIndex, params InputDirectionType[] inputTypes)
        {
            return InternalUp(false, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if up controls are pressed.
        /// </summary>
        public static bool UpPressed(PlayerIndex playerIndex)
        {
            return UpPressed(playerIndex, new InputDirectionType[] { InputDirectionType.All });
        }

        /// <summary>
        /// Checks if up controls are pressed.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        public static bool UpPressed(PlayerIndex playerIndex, params InputDirectionType[] inputTypes)
        {
            return InternalUp(true, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if down controls are down.
        /// </summary>
        public static bool DownDown(PlayerIndex playerIndex)
        {
            return DownDown(playerIndex, new InputDirectionType[] { InputDirectionType.All });
        }

        /// <summary>
        /// Checks if down controls are down.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        public static bool DownDown(PlayerIndex playerIndex, params InputDirectionType[] inputTypes)
        {
            return InternalDown(false, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if down controls are pressed.
        /// </summary>
        public static bool DownPressed(PlayerIndex playerIndex)
        {
            return DownPressed(playerIndex, new InputDirectionType[] { InputDirectionType.All });
        }

        /// <summary>
        /// Checks if down controls are pressed.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        public static bool DownPressed(PlayerIndex playerIndex, params InputDirectionType[] inputTypes)
        {
            return InternalDown(true, playerIndex, inputTypes);
        }

        #endregion
    }
}