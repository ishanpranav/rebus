// Ishan Pranav's REBUS: Tokenizer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Rebus.Tokens;

namespace Rebus
{
    public class Tokenizer
    {
        private readonly ITokenFactory _tokenFactory;
        private readonly MatchCollection _matches;

        public Tokenizer(ITokenFactory tokenFactory, string value)
        {
            _tokenFactory = tokenFactory;
            _matches = Regex.Matches(value.Replace("-", string.Empty), pattern: @"(\w+)|\""([\w\s]*)""", RegexOptions.Compiled);
        }

        public async IAsyncEnumerable<IToken> TokenizeAsync()
        {
            foreach (Match match in _matches)
            {
                Group quotationGroup = match.Groups[2];

                if (quotationGroup.Length > 0)
                {
                    yield return new QuotationToken(quotationGroup.Value);
                }
                else
                {
                    string lexeme = match.Groups[1].Value;

                    if (int.TryParse(lexeme, out int number))
                    {
                        yield return new NumberToken(number);
                    }
                    else if (HexPoint.TryParse(lexeme, out HexPoint hexPoint))
                    {
                        yield return new HexPointToken(hexPoint);
                    }
                    else
                    {
                        yield return await _tokenFactory.CreateTokenAsync(lexeme);
                    }
                }
            }
        }
    }
}
