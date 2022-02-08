// Ishan Pranav's REBUS: Tokenizer.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class Tokenizer
    {
        private const char DoubleQuoteChar = '"';

        private readonly Regex _regex;
        private readonly IDbContextFactory<UniverseContext> _contextFactory;

        public Tokenizer(Regex regex, IDbContextFactory<UniverseContext> contextFactory)
        {
            _regex = regex;
            _contextFactory = contextFactory;
        }

        public async IAsyncEnumerable<Token> TokenizeAsync(string value)
        {
            foreach (Match match in _regex.Matches(value))
            {
                Group quotationGroup = match.Groups[2];

                if (quotationGroup.Length > 0)
                {
                    yield return new Token()
                    {
                        Type = TokenTypes.Quotation,
                        Value = quotationGroup.Value
                    };
                }
                else
                {
                    string lexeme = match.Groups[1].Value;

                    if (int.TryParse(lexeme, out int number))
                    {
                        yield return new Token()
                        {
                            Type = TokenTypes.Number,
                            Value = number.ToString("n")
                        };
                    }
                    else
                    {
                        await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
                        {
                            yield return (await context.Tokens.SingleOrDefaultAsync(x => x.Value == lexeme)) ?? new Token()
                            {
                                Value = lexeme
                            };
                        }
                    }
                }
            }
        }
    }
}
