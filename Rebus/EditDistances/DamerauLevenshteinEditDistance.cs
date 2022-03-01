// Ishan Pranav's REBUS: DamerauLevenshteinEditDistance.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.EditDistances
{
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
