/// <summary>
/// Helper class to provide a global game instance that can be imported with "using static".
/// </summary>
internal static class GameProvider
{
    /// <summary>
    /// The global game instance.
    /// </summary>
    internal static GGFanGame.GameController GameInstance => GGFanGame.GameController.GetInstance();
}
