// Ishan Pranav's REBUS: IToken.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Defines a lexical token.
    /// </summary>
    public interface IToken : IWritable
    {
        /// <summary>
        /// Gets the token type.
        /// </summary>
        /// <value>A bitwise combination of the enumeration values that specifies the token type.</value>
        TokenTypes Type { get; }

        /// <summary>
        /// Gets the token value.
        /// </summary>
        /// <value>The string value.</value>
        string Value { get; }
    }
}
