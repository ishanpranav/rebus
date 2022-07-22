// Ishan Pranav's REBUS: BayerNumeralSystem.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Rebus.Server.NumeralSystems
{
    /// <summary>
    /// Represents the Greek and Latin numeral system used in Bayer designations.
    /// </summary>
    public class BayerNumeralSystem : INumeralSystem
    {
        public bool TryGetNumeral(int value, [NotNullWhen(true)] out string? result)
        {
            const char greekMin = '\u03b1';
            const char greekMax = '\u03c9';
            const int greekCount = greekMax - greekMin + 1;

            if (value < greekCount)
            {
                result = ((char)(greekMin + value)).ToString();
            }
            else
            {
                value -= greekCount;

                const char latinMin = 'A';

                if (value <= 'Z' - latinMin)
                {
                    result = ((char)(latinMin + value)).ToString();
                }
                else
                {
                    result = null;

                    return false;
                }
            }

            return true;
        }
    }
}
