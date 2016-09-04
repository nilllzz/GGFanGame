using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Screens
{
    /// <summary>
    /// A class to manage game states as screens.
    /// </summary>
    class ScreenManager
    {
        //We only ever need a single ScreenManager, so we do a singleton here:
        private static ScreenManager _instance = null;

        /// <summary>
        /// Returns the singleton instance of the APIManager.
        /// </summary>
        public static ScreenManager getInstance()
        {
            if (_instance == null)
                _instance = new ScreenManager();

            return _instance;
        }

        //The currently active screen instance.
        private Screen _currentScreen = null;

        /// <summary>
        /// Sets a new screen as active screen.
        /// </summary>
        /// <param name="newScreen">The new screen.</param>
        public void setScreen(Screen newScreen)
        {
            if (_currentScreen != null)
                _currentScreen.close();

            _currentScreen = newScreen;

            _currentScreen.open();
        }

        /// <summary>
        /// The currently active screen instance.
        /// </summary>
        public Screen currentScreen
        {
            get { return _currentScreen; }
        }

        /// <summary>
        /// Updates the current screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void updateScreen(GameTime gameTime)
        {
            if (_currentScreen != null)
                _currentScreen.update();
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void drawScreen(GameTime gameTime)
        {
            if (_currentScreen != null)
                _currentScreen.draw();
        }
    }
}