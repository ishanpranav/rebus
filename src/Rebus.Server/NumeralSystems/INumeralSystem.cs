// Ishan Pranav's REBUS: INumeralSystem.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Rebus.Server.NumeralSystems
{
    /// <summary>
    /// Defines a numeral system.
    /// </summary>
    public interface INumeralSystem
    {
        /// <summary>
        /// Gets the numeral that represents the specified integer.
        /// </summary>
        /// <param name="value">The integral value.</param>
        /// <param name="result">When this method returns, contains the numeral that represents the specified integer, if the number system can represent it; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the number system can represent the specified integer; otherwise, <see langword="false"/>.</returns>
        bool TryGetNumeral(int value, [NotNullWhen(true)] out string? result);
    }
}
