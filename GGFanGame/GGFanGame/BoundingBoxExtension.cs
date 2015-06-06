using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame
{
    static class BoundingBoxExtension
    {
        /// <summary>
        /// Adds an offset to this <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="offset">The offset to add.</param>
        /// <returns></returns>
        public static BoundingBox Offset(this BoundingBox box, Vector3 offset)
        {
            box.Min += offset;
            box.Max += offset;
            return box;
        }
    }
}