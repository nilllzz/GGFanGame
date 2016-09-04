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
    enum Identification
    {
        Title,
        InGame,
        Test,
        PlayerSelect,
        LoadSave,
        Transition
    }

    /// <summary>
    /// The base class for all screens in the game.
    /// </summary>
    abstract class Screen
    {
        private Identification _identification;

        public Screen(Identification identification)
        {
            _identification = identification;
        }

        /// <summary>
        /// The identification of this screen.
        /// </summary>
        public Identification identification
        {
            get { return _identification; }
        }
        
        /// <summary>
        /// If this screen is the currently active screen.
        /// </summary>
        protected bool isCurrentScreen
        {
            get
            {
                if (ScreenManager.getInstance().currentScreen != null)
                    return ScreenManager.getInstance().currentScreen.identification == identification;
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