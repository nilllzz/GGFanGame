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
}
