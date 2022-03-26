// Ishan Pranav's REBUS: DbPathfinder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rebus.Pathfinders;

namespace Rebus.Server
{
    internal sealed class DbPathfinder : HexPointPathfinder
    {
        private readonly int _playerId;
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public DbPathfinder(int playerId, IDbContextFactory<RebusDbContext> contextFactory)
        {
            _playerId = playerId;
            _contextFactory = contextFactory;
        }

        protected override IEnumerable<HexPoint> Neighbors(HexPoint value)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                foreach (HexPoint neighbor in value.Neighbors())
                {
                    if (context.Navigations.Any(x => x.PlayerId == _playerId && x.Q == value.Q && x.R == value.R))
                    {
                        yield return neighbor;
                    }
                }
            }
        }
    }
}
