// Ishan Pranav's REBUS: Repository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class ConceptRepository : IConceptRepository
    {
        private readonly IDbContextFactory<UniverseContext> _contextFactory;

        public ConceptRepository(IDbContextFactory<UniverseContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IReadOnlyCollection<IConcept>> GetVisibleContentsAsync(int containerId, int viewerId)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                IQueryable<Concept> contents = context
                    .IncludeUniverse()
                    .Where(x => x.ContainerId == containerId);

                if (!await contents.AnyAsync(x => x.Reflective))
                {
                    contents = contents.Where(x => x.Id != viewerId);
                }

                return await contents.ToArrayAsync();
            }
        }

        public async Task<IConcept> GetConceptAsync(int id)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context
                    .IncludeUniverse()
                    .SingleAsync(x => x.Id == id);
            }
        }
    }
}
