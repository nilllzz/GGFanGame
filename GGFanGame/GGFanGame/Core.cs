using System;
using GGFanGame;

/// <summary>
/// Provides access to the root singleton objects of the program.
/// </summary>
internal static class Core
{
    private static GameController _instance;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        using (_instance = new GameController())
            _instance.Run();
    }

    /// <summary>
    /// The global game instance.
    /// </summary>
    internal static GameController GameInstance => _instance;

    /// <summary>
    /// Gets a component of the game.
    /// </summary>
    internal static T GetComponent<T>() where T : Microsoft.Xna.Framework.IGameComponent
        => GameInstance.GetComponent<T>();
}
