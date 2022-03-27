// Ishan Pranav's REBUS: HexPointToken.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Tokens
{
    /// <summary>
    /// Represents a token containing a hexagonal coordinate.
    /// </summary>
    public class HexPointToken : IToken
    {
        /// <summary>
        /// Gets the hexagonal coordinate.
        /// </summary>
        /// <value>The hexagonal coordinate.</value>
        public HexPoint HexPoint { get; }

        /// <inheritdoc/>
        public TokenTypes Type
        {
            get
            {
                return TokenTypes.Substantive;
            }
        }

        /// <inheritdoc/>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexPointToken"/> class.
        /// </summary>
        /// <param name="value">The hexagonal coordinate.</param>
        public HexPointToken(HexPoint value)
        {
            HexPoint = value;

            ExpressionWriter writer = new ExpressionWriter();

            value.Write(writer);

            Value = writer.ToString();
        }

        /// <inheritdoc/>
        public void Write(ExpressionWriter writer)
        {
            writer.Write(HexPoint);
        }
    }
}
