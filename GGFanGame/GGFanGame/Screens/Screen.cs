using System;
using Microsoft.Xna.Framework.Content;
using static GameProvider;

namespace GGFanGame.Screens
{
    //Screens is how the game manages screen states.
    //A screen serves a function like MainMenu or InGame.

    /// <summary>
    /// The base class for all screens in the game.
    /// </summary>
    internal abstract class Screen : IDisposable
    {
        protected ContentManager content { get; private set; }

        /// <summary>
        /// Initializes the <see cref="ContentManager"/> for this screen.
        /// Use only when the screen needs a seperate ContentManager.
        /// </summary>
        protected void initializeContentManager()
        {
            content = new ContentManager(gameInstance.Services, "Content");
        }

        public bool isDisposed { get; protected set; }

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

        public void Dispose()
        {
            dispose(true);
        }
        
        ~Screen()
        {
            dispose(false);
        }

        /// <summary>
        /// Base screen dispose implementation. Disposes of the ContentManager and all loaded content.
        /// </summary>
        protected virtual void dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    content?.Dispose();
                }

                content = null;
                isDisposed = true;
            }
        }
    }
}