// Ishan Pranav's REBUS: Tokenizer.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rebus.Server
{
    internal class Tokenizer : ITokenizer
    {
        private const char DoubleQuoteChar = '"';

        private readonly Regex _regex;
        private readonly DbRepository _repository;

        public Tokenizer(Regex regex, DbRepository repository)
        {
            this._regex = regex;
            this._repository = repository;
        }

        public async IAsyncEnumerable<IToken> TokenizeAsync(string value)
        {
            foreach (Match match in this._regex.Matches(value))
            {
                string lexeme = match.Groups[1].Value;

                if (lexeme.StartsWith(DoubleQuoteChar) || lexeme.EndsWith(DoubleQuoteChar))
                {
                    yield return new Token()
                    {
                        Type = TokenTypes.Quotation,
                        Value = lexeme.Substring(1, lexeme.Length - 1)
                    };
                }
                else if (Int32.TryParse(lexeme, out _))
                {
                    yield return new Token()
                    {
                        Type = TokenTypes.Number,
                        Value = lexeme
                    };
                }
                else
                {
                    yield return await this._repository.GetTokenAsync(lexeme);
                }
            }
        }
    }
}
