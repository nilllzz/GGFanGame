namespace GGFanGame.Game
{
    /// <summary>
    /// Different behaviours of the bounding box of a <see cref="StageObject"/>.
    /// </summary>
    internal enum GroundRelation
    {
        /// <summary>
        /// Things that face away from the ground in a 90° angle, for example a fence.
        /// </summary>
        Upright,
        /// <summary>
        /// Things that lay flat on the ground, like streets.
        /// </summary>
        Flat
    }
}
