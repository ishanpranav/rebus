// Ishan Pranav's REBUS: LevenshteinEditDistance.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.EditDistances
{
    /// <summary>
    /// An <see cref="IEditDistance"/> used to calculate the edit distance between two strings using the Levenshtein algorithm.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href=" https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance">Damerau–Levenshtein distance - Wikipedia</seealso>
    public class LevenshteinEditDistance : IEditDistance
    {
        public int Calculate(string a, string b)
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

        protected virtual void RestrictedDistance(int[,] matrix, int i, int j, string value1, string value2)
        {
            // if a[i] = b[j] then
            //   cost := 0
            // else
            //   cost := 1

            int cost = Convert.ToInt32(value1[i - 1] != value2[j - 1]);

            // d[i, j] := minimum(d[i-1, j] + 1, d[i, j-1] + 1, d[i-1, j-1] + cost)

            matrix[i, j] = Math.Min(matrix[i - 1, j] + 1, Math.Min(matrix[i, j - 1] + 1, matrix[i - 1, j - 1] + cost));
        }
    }
}
