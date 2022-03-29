// Ishan Pranav's REBUS: ITokenFactory.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Defines a method for creating tokens.
    /// </summary>
    public interface ITokenFactory
    {
        /// <summary>
        /// Creates a token.
        /// </summary>
        /// <param name="value">The token value.</param>
        /// <returns>A new token instance.</returns>
        IToken CreateToken(string value);
    }
}
