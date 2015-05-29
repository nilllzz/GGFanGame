using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace GGFanGame.Content
{
    class MusicManager : ResourceManager<Song>
    {
        public MusicManager(GGGame game) : base(game)
        {

        }
    }
}