// Ishan Pranav's REBUS: VisionCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Commands
{
    [Guid("6C4E330D-29AD-498B-B6A9-CB45724B0A32")]
    internal sealed class VisionCommand : Rebus.Command
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public VisionCommand(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected override async IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            IConcept subject = GetConcept(Argument.Subject);

            if (subject.Is<Player>(out _))
            {
                await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
                {
                    foreach (IGrouping<HexPoint, Spacecraft> grouping in context.Spacecraft
                        .Where(x => x.PlayerId == PlayerId)
                        .AsEnumerable()
                        .GroupBy(x => x.Location))
                    {

                        yield return await context.CreateMessageAsync(resource: 4, grouping.Key, grouping);
                    }
                }
            }
        }
    }
}
