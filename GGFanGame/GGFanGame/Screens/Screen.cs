using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Screens
{
    //Screens is how the game manages screen states.
    //A screen serves a function like MainMenu or InGame.

    /// <summary>
    /// The identification of a screen.
    /// </summary>
    public enum Identification
    {
        MainMenu,
        InGame
    }

    /// <summary>
    /// The base class for all screens in the game.
    /// </summary>
    public abstract class Screen
    {
        private Identification _identification; 

        public Screen(Identification identification)
        {
            _identification = identification;
        }

        /// <summary>
        /// The identification of this screen.
        /// </summary>
        /// <returns></returns>
        public Identification identification
        {
            get { return _identification; }
        }

        /// <summary>
        /// Draws the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void draw(GameTime gameTime);

        /// <summary>
        /// Updates the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void update(GameTime gameTime);

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