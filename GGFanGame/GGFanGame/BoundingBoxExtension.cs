﻿using Microsoft.Xna.Framework;

namespace GGFanGame
{
    //An extension class for methods that extend BoundingBox.

    internal static class BoundingBoxExtension
    {
        /// <summary>
        /// Adds an offset to this <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="offset">The offset to add.</param>
        internal static BoundingBox Offset(this BoundingBox box, Vector3 offset)
        {
            box.Min += offset;
            box.Max += offset;
            return box;
        }
    }
}