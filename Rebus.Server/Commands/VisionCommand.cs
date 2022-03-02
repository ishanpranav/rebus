// Ishan Pranav's REBUS: VisionCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Rebus.Server.Concepts;

namespace Rebus.Server.Commands
{
    [Guid("6C4E330D-29AD-498B-B6A9-CB45724B0A32")]
    internal sealed class VisionCommand : Command
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public VisionCommand(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private VisionCommand(Player player, IDbContextFactory<RebusDbContext> contextFactory) : base(player)
        {
            _contextFactory = contextFactory;
        }

        public override async IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            if (IsReflexive(Argument.Subject))
            {
                await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
                {
                    foreach (IGrouping<HexPoint, Spacecraft> grouping in context.Spacecraft
                        .Where(x => x.PlayerId == Player.Id)
                        .AsEnumerable()
                        .GroupBy(x => x.Location))
                    {
                        yield return await context.CreateMessageAsync(resource: 1, grouping.Key, grouping.ToArray());
                    }
                }
            }
        }

        protected override Command CreateCommand(Player player)
        {
            return new VisionCommand(player, _contextFactory);
        }
    }
}
