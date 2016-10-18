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
        private ContentManager _content;

        /// <summary>
        /// Returns the <see cref="ContentManager"/> associated with this <see cref="Screen"/>.
        /// </summary>
        protected ContentManager content
        {
            get
            {
                if (_content == null)
                    _content = new ContentManager(gameInstance.Services, "Content");

                return _content;
            }
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
                    _content?.Dispose();
                }

                _content = null;
                isDisposed = true;
            }
        }
    }
}