// Ishan Pranav's REBUS: TokenFactory.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal class TokenFactory : ITokenFactory
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public TokenFactory(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IToken> CreateTokenAsync(string value)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return (await context.Tokens.FindAsync(value)) ?? new Token()
                {
                    Value = value
                };
            }
        }
    }
}
