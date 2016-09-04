namespace GGFanGame.Screens
{
    //Screens is how the game manages screen states.
    //A screen serves a function like MainMenu or InGame.

    /// <summary>
    /// The base class for all screens in the game.
    /// </summary>
    abstract class Screen
    {
        /// <summary>
        /// If this screen is the currently active screen.
        /// </summary>
        protected bool isCurrentScreen
        {
            get
            {
                if (ScreenManager.getInstance().currentScreen != null)
                    return ScreenManager.getInstance().currentScreen.GetType() == GetType();
                else
                    return false;
            }
        }

        /// <summary>
        /// Draws the screen.
        /// </summary>
        public abstract void draw();

        /// <summary>
        /// Updates the screen.
        /// </summary>
        public abstract void update();

        /// <summary>
        /// Gets called when the screen gets closed by the ScreenManager.
        /// </summary>
        public virtual void close() { }

        /// <summary>
        /// Gets called when hte screen gets opened by the ScreenManager.
        /// </summary>
        public virtual void open() { }
    }
}