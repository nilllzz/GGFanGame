using System;
using System.Collections.Generic;
using System.Linq;

namespace GGFanGame
{
    /// <summary>
    /// Easy access to ASCII char ranges.
    /// </summary>
    internal static class ASCIIProvider
    {
        private static IEnumerable<char> GetCharsEnumerable(int start, int count)
            => Enumerable.Range(start, count).Select(i => (char)i);

        /// <summary>
        /// Returns an amount of ASCII chars from a starting position.
        /// </summary>
        internal static char[] GetChars(int start, int count)
            => GetCharsEnumerable(start, count).ToArray();

        /// <summary>
        /// Returns multiple ranges of ASCII chars concatenated into a single array.
        /// </summary>
        /// <param name="positions">The positions of the chars in the ASCII table. Item1 is the start index, Item2 the count.</param>
        internal static char[] GetChars((int startIndex, int count)[] positions)
        {
            if (positions == null || positions.Length == 0)
                return new char[0];
            if (positions.Length == 1)
                return GetChars(positions[0].startIndex, positions[0].count);

            IEnumerable<char> chars = GetCharsEnumerable(positions[0].startIndex, positions[0].count);

            for (int i = 1; i < positions.Length; i++)
                chars = chars.Concat(GetCharsEnumerable(positions[i].startIndex, positions[i].count));

            return chars.ToArray();
        }
    }
}
