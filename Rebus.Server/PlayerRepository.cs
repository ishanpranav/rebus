// Ishan Pranav's REBUS: PlayerRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class PlayerRepository : IPlayerRepository
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public PlayerRepository(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> GetPlayerAsync(string userId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                Player? result = await context.Players
                    .Include(x => x.Concepts)
                    .SingleOrDefaultAsync(x => x.UserId == userId);

                if (result is null)
                {
                    result = new Player((await context
                        .Query<Concept>()
                        .SingleAsync(x => x.Id == 1))
                        .CreateConcept())
                    {
                        UserId = userId,
                    };

                    await context.AddAsync(result);
                    await context.SaveChangesAsync();
                }

                return result.GetConcept().Id;
            }
        }
    }
}
