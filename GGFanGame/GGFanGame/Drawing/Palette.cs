using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Drawing
{
    /// <summary>
    /// A class to work with texture palettes.
    /// </summary>
    internal static class Palette
    {
        /* 
        As clarification on how palettes are supposed to work:
        The palette is a texture that is two pixels high.
        The first row contains the original colors that are being replaced in the texture.
        The second row contains the replacement colors for the colors above them.
        So if you want to replace 5 colors from a texture, your palette should be 5x2 pixels large.

        All other colors will be left untouched.
        */

        /// <summary>
        /// Stores replacement colors from the palette.
        /// </summary>
        private struct PaletteColor
        {
            public PaletteColor(Color originalColor, Color newColor)
            {
                this.originalColor = originalColor;
                this.newColor = newColor;
            }

            /// <summary>
            /// The color on the original texture.
            /// </summary>
            public Color originalColor { get; }

            /// <summary>
            /// The replacement for the original color.
            /// </summary>
            public Color newColor { get; }
        }

        /// <summary>
        /// Applies a palette to a texture.
        /// </summary>
        /// <param name="originalTexture">The original texture that the palette should get applied to.</param>
        /// <param name="paletteTexture">The palette texture to apply.</param>
        public static Texture2D ApplyPalette(Texture2D originalTexture, Texture2D paletteTexture)
        {
            if (paletteTexture.Height != 2)
                throw new PaletteTextureSizeException(paletteTexture);

            var paletteColors = new PaletteColor[paletteTexture.Width];
            var paletteTextureData = new Color[paletteTexture.Width * paletteTexture.Height];

            var originalColorData = new Color[originalTexture.Width * originalTexture.Height];

            //Create a resulting texture we return and a corresponding color array to store the data in.
            var resultTexture = new Texture2D(GameInstance.GraphicsDevice, originalTexture.Width, originalTexture.Height);
            var resultColorData = new Color[resultTexture.Width * resultTexture.Height];

            //We get the color data from the palette and original texture here and store them in arrays.
            paletteTexture.GetData(paletteTextureData);
            originalTexture.GetData(originalColorData);

            //Loop through the palette texture and get all the replacement colors:
            for (var x = 0; x < paletteTexture.Width; x++)
            {
                var originalColor = paletteTextureData[x];
                var newColor = paletteTextureData[x + paletteTexture.Width]; //the replacing color is below the original color.

                paletteColors[x] = new PaletteColor(originalColor, newColor);
            }

            //Loop through the original texture and search if there's a replacement.
            //if not, just put the original color.
            for (var i = 0; i < originalColorData.Length; i++)
            {
                var foundReplacement = false;

                foreach (var color in paletteColors)
                {
                    if (color.originalColor == originalColorData[i])
                    {
                        resultColorData[i] = color.newColor;
                        foundReplacement = true;
                    }
                }

                if (!foundReplacement)
                {
                    resultColorData[i] = originalColorData[i];
                }
            }

            resultTexture.SetData(resultColorData);

            return resultTexture;
        }
    }

    /// <summary>
    /// An exception for when the input palette has a wrong format.
    /// </summary>
    internal class PaletteTextureSizeException : Exception
    {
        const string MESSAGE = "The input palette didn't have the correct format. Its height is {0}, but is supposed to be 2.";

        public PaletteTextureSizeException(Texture2D paletteTexture) : base(string.Format(MESSAGE, paletteTexture.Height.ToString())) { }
    }
}