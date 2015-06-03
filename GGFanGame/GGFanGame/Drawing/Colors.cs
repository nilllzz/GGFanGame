using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Drawing
{
    /// <summary>
    /// Color managemend for the players.
    /// </summary>
    sealed class Colors
    {
        public static readonly Color oneUpColor = new Color(103, 204, 252);
        public static readonly Color twoUpColor = new Color(245, 204, 43);
        public static readonly Color threeUpColor = new Color(215, 71, 213);
        public static readonly Color fourUpColor = new Color(215, 67, 110);

        /// <summary>
        /// Returns a color based on the player index
        /// </summary>
        /// <returns></returns>
        public static Color getColor(PlayerIndex playerIndex)
        {
            switch (playerIndex)
            {
                case PlayerIndex.One:
                    return oneUpColor;
                case PlayerIndex.Two:
                    return twoUpColor;
                case PlayerIndex.Three:
                    return threeUpColor;
                case PlayerIndex.Four:
                    return fourUpColor;
            }

            return default(Color);
        }
    }
}