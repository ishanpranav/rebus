// Ishan Pranav's REBUS: IEditDistance.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Defines a method for calcuating the edit distance between strings.
    /// </summary>
    public interface IEditDistance
    {
        /// <summary>
        /// Computes the edit distance between two strings.
        /// </summary>
        /// <param name="a">The first string.</param>
        /// <param name="b">The second string.</param>
        /// <returns>The edit distance between <paramref name="a"/> and <paramref name="b"/>.</returns>
        int Compute(string a, string b);
    }
}
