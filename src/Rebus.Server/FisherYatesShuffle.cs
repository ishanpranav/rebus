// Ishan Pranav's REBUS: FisherYatesShuffle.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus.Server
{
    /// <summary>
    /// Performs the Fisher-Yates algorithm to shuffle collections.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates shuffle - Wikipedia</seealso>
    public class FisherYatesShuffle
    {
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="FisherYatesShuffle"/> class.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        public FisherYatesShuffle(Random random)
        {
            _random = random;
        }

        /// <summary>
        /// Shuffles a collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="values">The collection.</param>
        public void Shuffle<T>(IList<T> values)
        {
            int n = values.Count;

            while (n > 1)
            {
                n--;

                int k = _random.Next(n + 1);

                (values[n], values[k]) = (values[k], values[n]);
            }
        }
    }
}
