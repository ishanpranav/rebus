// Ishan Pranav's REBUS: QuotationToken.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Tokens
{
    public class QuotationToken : IToken
    {
        public TokenTypes Type
        {
            get
            {
                return TokenTypes.Quotation;
            }
        }

        public string Value { get; }

        public QuotationToken(string value)
        {
            Value = value;
        }

        public void Write(ExpressionWriter writer)
        {
            using (writer.CreateScope(ScopeType.Quotation))
            {
                writer.Write(Value);
            }
        }
    }
}
