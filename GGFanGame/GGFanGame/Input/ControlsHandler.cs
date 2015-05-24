using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GGFanGame.Input
{
    /// <summary>
    /// Possible input types for directions.
    /// </summary>
    enum InputDirectionType
    {
        /// <summary>
        /// The WASD keys.
        /// </summary>
        WASD,
        /// <summary>
        /// Arrow keys.
        /// </summary>
        ArrowKeys,
        /// <summary>
        /// The directional pad on a GamePad.
        /// </summary>
        DPad,
        /// <summary>
        /// The left thumbstick on a GamePad.
        /// </summary>
        ThumbStick,
        /// <summary>
        /// Combines all the control types.
        /// </summary>
        All
    }

    /// <summary>
    /// The four main directions for input.
    /// </summary>
    enum InputDirection
    {
        Up,
        Left,
        Down,
        Right,
        None
    }

    /// <summary>
    /// Input types to control accept/dismiss actions.
    /// </summary>
    enum InputControlType
    {
        Keyboard,
        GamePad
    }

    /// <summary>
    /// A class to handle all input methods.
    /// </summary>
    class ControlsHandler
    {
        /// <summary>
        /// Updates all input handlers.
        /// </summary>
        public static void update()
        {
            KeyboardHandler.update();
            GamePadHandler.update();
            MouseHandler.update();
        }

        /*
        This keeps track of the direction that was last pressed.
        That is important since we want to have a feature where you can hold down a direction,
        and it starts to "press" that direction button at an interval (once per 3 frames).
        The pressedKeyDelay is set to wait for 40 frames until the 3 frame interval starts.
        */

        //We have 4 element-long arrays because there can be four players connected. We initialize them with the standard values.
        private static InputDirection[] _lastPressedDirection = new InputDirection[] { InputDirection.None, InputDirection.None, InputDirection.None, InputDirection.None };
        private static float[] _pressedKeyDelay = new float[] { 0f, 0f, 0f, 0f };

        /// <summary>
        /// Resets when no direction is pressed.
        /// </summary>
        /// <param name="direction"></param>
        private static void resetDirectionPressed(PlayerIndex playerIndex, InputDirection direction)
        {
            int i = (int)playerIndex;
            if (direction == _lastPressedDirection[i])
            {
                _pressedKeyDelay[i] = 4f;
                _lastPressedDirection[i] = InputDirection.None;
            }
        }

        /// <summary>
        /// When a different direction is pressed, then reset the wait time to 40 frames.
        /// </summary>
        /// <param name="direction"></param>
        private static void changeDirectionPressed(PlayerIndex playerIndex, InputDirection direction)
        {
            int i = (int)playerIndex;
            if (_lastPressedDirection[i] != direction)
            {
                _pressedKeyDelay[i] = 4f;
                _lastPressedDirection[i] = direction;
            }
        }

        /// <summary>
        /// Checks, if the hold-down-pressed feature is active and updates it.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private static bool holdDownPressed(PlayerIndex playerIndex, InputDirection direction)
        {
            int i = (int)playerIndex;
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
        /// <returns></returns>
        private static bool checkDirectional(bool pressed, PlayerIndex playerIndex, InputDirection direction, Keys WASDKey, Keys arrowKey, Buttons thumbStickDirection, Buttons dPadDirection, InputDirectionType[] inputTypes)
        {
            if (pressed)
                return checkDirectionalPress(playerIndex, direction, WASDKey, arrowKey, thumbStickDirection, dPadDirection, inputTypes);
            else
                return checkDirectionalDown(playerIndex, direction, WASDKey, arrowKey, thumbStickDirection, dPadDirection, inputTypes);
        }

        /// <summary>
        /// Checks if any of the given directions are pressed.
        /// </summary>
        /// <param name="inputTypes">All input types to check.</param>
        /// <returns></returns>
        private static bool checkDirectionalPress(PlayerIndex playerIndex, InputDirection direction, Keys WASDKey, Keys arrowKey, Buttons thumbStickDirection, Buttons dPadDirection, InputDirectionType[] inputTypes)
        {
            //This keeps track if any direction key has been pressed. If not, it will reset the delay at the end.
            bool hasInputDirection = false;

            bool checkForAll = inputTypes.Contains(InputDirectionType.All);

            //The keyboard is always assigned to player one.
            //This might be changed later, if it does, we need to replace the PlayerIndex.One to the designated player.

            //Check for WASD keys.
            if ((inputTypes.Contains(InputDirectionType.WASD) || checkForAll) && playerIndex == PlayerIndex.One)
            {
                if (KeyboardHandler.keyDown(WASDKey))
                {
                    hasInputDirection = true;
                    if (holdDownPressed(PlayerIndex.One, direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (KeyboardHandler.keyPressed(WASDKey))
                        {
                            changeDirectionPressed(PlayerIndex.One, direction);
                            return true;
                        }
                    }
                }
            }

            //Check for arrow keys.
            if ((inputTypes.Contains(InputDirectionType.ArrowKeys) || checkForAll) && playerIndex == PlayerIndex.One)
            {
                if (KeyboardHandler.keyDown(arrowKey))
                {
                    hasInputDirection = true;
                    if (holdDownPressed(PlayerIndex.One, direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (KeyboardHandler.keyPressed(arrowKey))
                        {
                            changeDirectionPressed(PlayerIndex.One, direction);
                            return true;
                        }
                    }
                }
            }

            //Check for the left thumbstick.
            if (inputTypes.Contains(InputDirectionType.ThumbStick) || checkForAll)
            {
                if (GamePadHandler.buttonDown(playerIndex, thumbStickDirection))
                {
                    hasInputDirection = true;
                    if (holdDownPressed(playerIndex, direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (GamePadHandler.buttonPressed(playerIndex, thumbStickDirection))
                        {
                            changeDirectionPressed(playerIndex, direction);
                            return true;
                        }
                    }
                }
            }

            //Check for the dpad.
            if (inputTypes.Contains(InputDirectionType.DPad) || checkForAll)
            {
                if (GamePadHandler.buttonDown(playerIndex, dPadDirection))
                {
                    hasInputDirection = true;
                    if (holdDownPressed(playerIndex, direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (GamePadHandler.buttonPressed(playerIndex, dPadDirection))
                        {
                            changeDirectionPressed(playerIndex, direction);
                            return true;
                        }
                    }
                }
            }

            //When no direction button was pressed, reset and return false.
            if (!hasInputDirection)
            {
                resetDirectionPressed(playerIndex, direction);
            }

            return false;
        }

        /// <summary>
        /// Checks if any of the given directions are down.
        /// </summary>
        /// <param name="inputTypes">All input types to check.</param>
        /// <returns></returns>
        private static bool checkDirectionalDown(PlayerIndex playerIndex, InputDirection direction, Keys WASDKey, Keys arrowKey, Buttons thumbStickDirection, Buttons dPadDirection, InputDirectionType[] inputTypes)
        {
            //When we should check for any input type, check if the handler's inputDown is pressed.

            bool checkForAll = inputTypes.Contains(InputDirectionType.All);

            if ((inputTypes.Contains(InputDirectionType.WASD) || checkForAll) && playerIndex == PlayerIndex.One)
            {
                if (KeyboardHandler.keyDown(WASDKey))
                {
                    return true;
                }
            }
            if ((inputTypes.Contains(InputDirectionType.ArrowKeys) || checkForAll) && playerIndex == PlayerIndex.One)
            {
                if (KeyboardHandler.keyDown(arrowKey))
                {
                    return true;
                }
            }
            if (inputTypes.Contains(InputDirectionType.ThumbStick) || checkForAll)
            {
                if (GamePadHandler.buttonDown(playerIndex, thumbStickDirection))
                {
                    return true;
                }
            }
            if (inputTypes.Contains(InputDirectionType.DPad) || checkForAll)
            {
                if (GamePadHandler.buttonDown(playerIndex, dPadDirection))
                {
                    return true;
                }
            }

            //No handler has the button down? return false.
            return false;
        }

        #region Internal direction checking

        //These methods check the four directions:

        private static bool internalLeft(bool pressed, PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return checkDirectional(pressed, playerIndex, InputDirection.Left, Keys.A, Keys.Left, Buttons.LeftThumbstickLeft, Buttons.DPadLeft, inputTypes);
        }

        private static bool internalRight(bool pressed, PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return checkDirectional(pressed, playerIndex, InputDirection.Right, Keys.D, Keys.Right, Buttons.LeftThumbstickRight, Buttons.DPadRight, inputTypes);
        }

        private static bool internalUp(bool pressed, PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return checkDirectional(pressed, playerIndex, InputDirection.Up, Keys.W, Keys.Up, Buttons.LeftThumbstickUp, Buttons.DPadUp, inputTypes);
        }

        private static bool internalDown(bool pressed, PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return checkDirectional(pressed, playerIndex, InputDirection.Down, Keys.S, Keys.Down, Buttons.LeftThumbstickDown, Buttons.DPadDown, inputTypes);
        }

        #endregion

        #region public interface

        //These methods are the public access to the internal check methods:

        /// <summary>
        /// Checks if left controls are down.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        /// <returns></returns>
        public static bool leftDown(PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return internalLeft(false, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if left controls are pressed.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        /// <returns></returns>
        public static bool leftPressed(PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return internalLeft(true, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if right controls are down.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        /// <returns></returns>
        public static bool rightDown(PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return internalRight(false, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if right controls are pressed.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        /// <returns></returns>
        public static bool rightPressed(PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return internalRight(true, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if up controls are down.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        /// <returns></returns>
        public static bool upDown(PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return internalUp(false, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if up controls are pressed.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        /// <returns></returns>
        public static bool upPressed(PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return internalUp(true, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if down controls are down.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        /// <returns></returns>
        public static bool downDown(PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return internalDown(false, playerIndex, inputTypes);
        }

        /// <summary>
        /// Checks if down controls are pressed.
        /// </summary>
        /// <param name="inputTypes">The input types to check.</param>
        /// <returns></returns>
        public static bool downPressed(PlayerIndex playerIndex, InputDirectionType[] inputTypes)
        {
            return internalDown(true, playerIndex, inputTypes);
        }

        #endregion
    }
}