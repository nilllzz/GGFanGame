namespace GGFanGame
{
    /// <summary>
    /// Helper class to provide a global game instance that can be imported with "using static".
    /// </summary>
    static class GameProvider
    {
        /// <summary>
        /// The global game instance.
        /// </summary>
        public static GGGame gameInstance => GGGame.instance;
    }
}
