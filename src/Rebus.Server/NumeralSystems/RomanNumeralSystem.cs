// Ishan Pranav's REBUS: RomanNumeralSystem.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Rebus.Server.NumeralSystems
{
    /// <summary>
    /// Represents the Roman numeral system.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/RomanNumeralExtensions.cs">Humanizer&apos;s ToRoman(int) function</see>.
    /// </remarks>
    /// <seealso href="https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/RomanNumeralExtensions.cs">Humanizer/RomanNumeralExtensions.cs</seealso>
    public class RomanNumeralSystem : INumeralSystem
    {
        private static readonly string[] s_numerals = new string[]
        {
            "I",
            "IV",
            "V",
            "IX",
            "X",
            "XL",
            "L",
            "XC",
            "C",
            "CD",
            "D",
            "CM",
            "M",
        };
        private static readonly int[] s_integers = new int[]
        {
            1,
            4,
            5,
            9,
            10,
            40,
            50,
            90,
            100,
            400,
            500,
            900,
            1000
        };

        /// <inheritdoc/>
        public bool TryGetNumeral(int value, [NotNullWhen(true)] out string? result)
        {
            if (value < 4000)
            {
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = s_numerals.Length - 1; i >= 0; i--)
                {
                    int integer = s_integers[i];

                    while (value / integer > 0)
                    {
                        stringBuilder.Append(s_numerals[i]);

                        value -= integer;
                    }
                }

                result = stringBuilder.ToString();

                return true;
            }
            else
            {
                result = null;

                return false;
            }
        }
    }
}
