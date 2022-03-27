// Ishan Pranav's REBUS: QuotationToken.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Tokens
{
    /// <summary>
    /// Represents a token containing a string literal.
    /// </summary>
    public class QuotationToken : IToken
    {
        /// <inheritdoc/>
        public TokenTypes Type
        {
            get
            {
                return TokenTypes.Quotation;
            }
        }

        /// <inheritdoc/>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuotationToken"/> class.
        /// </summary>
        /// <param name="value">The string literal.</param>
        public QuotationToken(string value)
        {
            Value = value;
        }

        /// <inheritdoc/>
        public void Write(ExpressionWriter writer)
        {
            writer.Write('"');
            writer.Write(Value);
            writer.Write('"');
        }
    }
}
