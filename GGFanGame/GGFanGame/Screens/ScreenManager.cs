using Microsoft.Xna.Framework;

namespace GGFanGame.Screens
{
    /// <summary>
    /// A class to manage game states as screens.
    /// </summary>
    internal class ScreenManager
    {
        //We only ever need a single ScreenManager, so we do a singleton here:
        private static ScreenManager _instance;

        /// <summary>
        /// Returns the singleton instance of the APIManager.
        /// </summary>
        public static ScreenManager getInstance() => _instance ?? (_instance = new ScreenManager());

        /// <summary>
        /// The currently active screen instance.
        /// </summary>
        public Screen currentScreen { get; private set; }

        /// <summary>
        /// Sets a new screen as active screen.
        /// </summary>
        /// <param name="newScreen">The new screen.</param>
        public void setScreen(Screen newScreen)
        {
            currentScreen?.close();

            currentScreen = newScreen;

            currentScreen.open();
        }

        /// <summary>
        /// Updates the current screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void updateScreen(GameTime gameTime)
        {
            currentScreen?.update();
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void drawScreen(GameTime gameTime)
        {
            currentScreen?.draw();
        }
    }
}