// Ishan Pranav's REBUS: MicrocomputerConcept.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Concepts
{
    internal sealed class MicrocomputerConcept : Concept
    {
        public async IAsyncEnumerable<IWritable> ViewAsync(RebusDbContext context)
        {
            foreach (IGrouping<RegionConcept, SpacecraftConcept> grouping in context
                .Query<SpacecraftConcept>()
                .Where(x => x.PlayerId == PlayerId)
                .Include(x => x.Region)
                .GroupBy(x => x.Region))
            {
                yield return new Message(null, null, "Region {0}", new object[] { grouping.Key });

                foreach (SpacecraftConcept spacecraft in grouping)
                {
                    yield return new Message(null, null, "Spacecraft {0}", new object[] { spacecraft });
                }
            }

            await Task.CompletedTask;
        }
    }
}
