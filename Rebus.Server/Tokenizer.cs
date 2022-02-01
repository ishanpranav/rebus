// Ishan Pranav's REBUS: Tokenizer.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rebus.Server
{
    internal sealed class Tokenizer
    {
        private const char DoubleQuoteChar = '"';

        private readonly Regex _regex;
        private readonly Repository _repository;

        public Tokenizer(Regex regex, Repository repository)
        {
            _regex = regex;
            _repository = repository;
        }

        public async IAsyncEnumerable<Token> TokenizeAsync(string value)
        {
            foreach (Match match in _regex.Matches(value))
            {
                string lexeme = match.Groups[0].Value;
                int lastIndex = lexeme.Length - 1;

                if (lexeme.Length > 2 && lexeme[0] == DoubleQuoteChar && lexeme[lastIndex] == DoubleQuoteChar)
                {
                    yield return new Token()
                    {
                        Type = TokenTypes.Quotation,
                        Value = lexeme.Substring(1, lastIndex - 1)
                    };
                }
                else if (int.TryParse(lexeme, out _))
                {
                    yield return new Token()
                    {
                        Type = TokenTypes.Number,
                        Value = lexeme
                    };
                }
                else
                {
                    yield return await _repository.GetTokenAsync(lexeme);
                }
            }
        }
    }
}
