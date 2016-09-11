using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Content
{
    /// <summary>
    /// The resource manager for <see cref="SpriteFont"/> objects.
    /// </summary>
    class FontManager : ResourceManager<SpriteFont>
    {
        public FontManager()
        {
            defaultFolder = "Fonts";
        }
    }
}