// Ishan Pranav's REBUS: DbPathfinderFactory.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class DbPathfinderFactory : IPathfinderFactory<HexPoint>
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public DbPathfinderFactory(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IPathfinder<HexPoint> CreatePathfinder(int playerId)
        {
            return new DbPathfinder(playerId, _contextFactory);
        }
    }
}
