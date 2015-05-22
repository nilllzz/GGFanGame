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
        MainMenu,
        InGame,
        Test,
        PlayerSelect
    }

    /// <summary>
    /// The base class for all screens in the game.
    /// </summary>
    abstract class Screen
    {
        private Identification _identification;
        private GGGame _game;

        public Screen(Identification identification, GGGame game)
        {
            _identification = identification;
            _game = game;
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
        /// The game instance.
        /// </summary>
        /// <returns></returns>
        protected GGGame gameInstance
        {
            get { return _game; }
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