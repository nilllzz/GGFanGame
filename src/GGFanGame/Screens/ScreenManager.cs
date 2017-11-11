using Microsoft.Xna.Framework;

namespace GGFanGame.Screens
{
    /// <summary>
    /// A class to manage game states as screens.
    /// </summary>
    internal class ScreenManager : IGameComponent
    {
        void IGameComponent.Initialize() { }

        /// <summary>
        /// The currently active screen instance.
        /// </summary>
        internal Screen CurrentScreen { get; private set; }

        /// <summary>
        /// Sets a new screen as active screen.
        /// </summary>
        /// <param name="newScreen">The new screen.</param>
        internal void SetScreen(Screen newScreen)
        {
            if (newScreen.ReplacePrevious)
                CurrentScreen?.Close();

            CurrentScreen = newScreen;

            CurrentScreen.Open();
        }

        /// <summary>
        /// Updates the current screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        internal void UpdateScreen(GameTime gameTime)
        {
            CurrentScreen?.Update(gameTime);
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        internal void DrawScreen(GameTime gameTime)
        {
            CurrentScreen?.Draw(gameTime);
        }
    }
}
