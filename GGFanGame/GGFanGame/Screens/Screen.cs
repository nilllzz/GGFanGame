using System;
using Microsoft.Xna.Framework.Content;
using static Core;
using Microsoft.Xna.Framework;

namespace GGFanGame.Screens
{
    // Screens is how the game manages screen states.
    // A screen serves a function like MainMenu or InGame.

    /// <summary>
    /// The base class for all screens in the game.
    /// </summary>
    internal abstract class Screen : IDisposable
    {
        private ContentManager _content;

        /// <summary>
        /// Returns the <see cref="ContentManager"/> associated with this <see cref="Screen"/>.
        /// </summary>
        protected ContentManager Content
        {
            get
            {
                if (_content == null)
                    _content = new ContentManager(GameInstance.Services, "Content");

                return _content;
            }
        }

        internal virtual bool ReplacePrevious { get; } = true;

        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// If this screen is the currently active screen.
        /// </summary>
        protected bool IsCurrentScreen
        {
            get
            {
                if (GetComponent<ScreenManager>().CurrentScreen != null)
                    return GetComponent<ScreenManager>().CurrentScreen.GetType() == GetType();
                else
                    return false;
            }
        }

        /// <summary>
        /// Draws the screen.
        /// </summary>
        public abstract void Draw(GameTime time);

        /// <summary>
        /// Updates the screen.
        /// </summary>
        public abstract void Update(GameTime time);

        /// <summary>
        /// Gets called when the screen gets closed by the ScreenManager.
        /// </summary>
        public virtual void Close() => Dispose();

        /// <summary>
        /// Gets called when hte screen gets opened by the ScreenManager.
        /// </summary>
        public virtual void Open() { }

        public void Dispose()
        {
            Dispose(true);
        }

        ~Screen()
        {
            Dispose(false);
        }

        /// <summary>
        /// Base screen dispose implementation. Disposes of the ContentManager and all loaded content.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _content?.Dispose();
                }

                _content = null;
                IsDisposed = true;
            }
        }
    }
}
