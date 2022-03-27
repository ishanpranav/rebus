// Ishan Pranav's REBUS: LevenshteinEditDistance.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.EditDistances
{
    /// <summary>
    /// Computes the Levenshtein edit distance using the Wagner–Fischer algorithm.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Wagner%E2%80%93Fischer_algorithm">Wagner–Fischer algorithm - Wikipedia</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance - Wikipedia</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance">Damerau–Levenshtein distance - Wikipedia</seealso>
    public class LevenshteinEditDistance : IEditDistance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LevenshteinEditDistance"/> class.
        /// </summary>
        public LevenshteinEditDistance() { }

        /// <inheritdoc/>
        public int Compute(string a, string b)
        {
            // algorithm OSA-distance is
            // input: strings a[1..length(a)], b[1..length(b)]
            // output: distance, integer

            // let d[0..length(a), 0..length(b)] be a 2 - d array of integers, dimensions length(a)+1, length(b) + 1

            int[,] matrix = new int[a.Length + 1, b.Length + 1];

            // for i := 0 to length(a) inclusive do

            for (int i = 0; i <= a.Length; i++)
            {
                // d[i, 0] := i

                matrix[i, 0] = i;
            }

            // for j := 0 to length(b) inclusive do

            for (int i = 0; i <= b.Length; i++)
            {
                // d[0, j] := j

                matrix[0, i] = i;
            }

            // for i := 1 to length(a) inclusive do

            for (int i = 1; i <= a.Length; i++)
            {
                // for j := 1 to length(b) inclusive do

                for (int j = 1; j <= b.Length; j++)
                {
                    RestrictedDistance(matrix, i, j, a, b);
                }
            }

            // return d[length(a), length(b)]

            return matrix[a.Length, b.Length];
        }

        /// <summary>
        /// Performs the restricted distance function.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="i">The index in the first dimension.</param>
        /// <param name="j">The index in the second dimension.</param>
        /// <param name="a">The first string.</param>
        /// <param name="b">The second string.</param>
        protected virtual void RestrictedDistance(int[,] matrix, int i, int j, string a, string b)
        {
            // if a[i] = b[j] then
            //   cost := 0
            // else
            //   cost := 1

            // d[i, j] := minimum(d[i-1, j] + 1, d[i, j-1] + 1, d[i-1, j-1] + cost)

            matrix[i, j] = Math.Min(matrix[i - 1, j] + 1, Math.Min(matrix[i, j - 1] + 1, matrix[i - 1, j - 1] + Convert.ToInt32(a[i - 1] != b[j - 1])));
        }
    }
}
