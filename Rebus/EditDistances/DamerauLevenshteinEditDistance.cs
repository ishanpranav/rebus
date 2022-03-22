// Ishan Pranav's REBUS: DamerauLevenshteinEditDistance.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.EditDistances
{
    /// <summary>
    /// An <see cref="IEditDistance"/> used to calculate the edit distance between two strings using the Damerau–Levenshtein algorithm.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance">Damerau–Levenshtein distance - Wikipedia</seealso>
    public class DamerauLevenshteinEditDistance : LevenshteinEditDistance
    {
        protected override void RestrictedDistance(int[,] matrix, int i, int j, string a, string b)
        {
            base.RestrictedDistance(matrix, i, j, a, b);

            // if i > 1 and j > 1 and a[i] = b[j - 1] and a[i - 1] = b[j] then

            if (i > 1 && j > 1 && a[i - 1] == b[j - 2] && a[i - 2] == b[j - 1])
            {
                // d[i, j] := minimum(d[i, j], d[i - 2, j - 2] + 1)

                matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + 1);
            }
        }
    }
}
