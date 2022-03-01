// Ishan Pranav's REBUS: NumberToken.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.Tokens
{
    public class NumberToken : IToken
    {
        private readonly int _number;

        public TokenTypes Type
        {
            get
            {
                return TokenTypes.Number;
            }
        }

        public string Value
        {
            get
            {
                return _number.ToString("n");
            }
        }

        public NumberToken(int value)
        {
            _number = value;
        }

        public void Write(ExpressionWriter writer)
        {
            writer.Write(_number);
        }
    }
}
