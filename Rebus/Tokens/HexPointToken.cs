// Ishan Pranav's REBUS: HexPointToken.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Tokens
{
    public class HexPointToken : IToken
    {
        public HexPoint HexPoint { get; }

        public TokenTypes Type
        {
            get
            {
                return TokenTypes.Substantive;
            }
        }

        public string Value { get; }

        public HexPointToken(HexPoint value)
        {
            HexPoint = value;

            ExpressionWriter writer = new ExpressionWriter();

            value.Write(writer);

            Value = writer.ToString();
        }

        public void Write(ExpressionWriter writer)
        {
            writer.Write(HexPoint);
        }
    }
}
