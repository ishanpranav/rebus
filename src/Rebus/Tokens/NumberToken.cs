// Ishan Pranav's REBUS: NumberToken.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Tokens
{
    /// <summary>
    /// Represents a token containing a numeric literal.
    /// </summary>
    public class NumberToken : IToken
    {
        private readonly int _number;

        /// <inheritdoc/>
        public TokenTypes Type
        {
            get
            {
                return TokenTypes.Number;
            }
        }

        /// <inheritdoc/>
        public string Value
        {
            get
            {
                return _number.ToString("n0");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberToken"/> class.
        /// </summary>
        /// <param name="value">The numeric literal.</param>
        public NumberToken(int value)
        {
            _number = value;
        }

        /// <inheritdoc/>
        public void Write(ExpressionWriter writer)
        {
            writer.Write(_number, format: null);
        }
    }
}
