// Ishan Pranav's REBUS: NumberToken.cs
// Copyright (c) Ishan Pranav. All rights reserved.
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
                return _number.ToString("n0");
            }
        }

        public NumberToken(int value)
        {
            _number = value;
        }

        public void Write(ExpressionWriter writer)
        {
            writer.Write(_number, word: null);
        }
    }
}
