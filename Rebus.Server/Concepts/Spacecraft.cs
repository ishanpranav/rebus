// Ishan Pranav's REBUS: Spacecraft.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Concepts
{
    internal sealed class Spacecraft : Concept, IMobile, IViewer
    {
        public async Task ViewAsync(ExpressionWriter writer, RebusDbContext context)
        {
            (await context.CreateMessageAsync(resource: 17, await context.Planets
                .AsWritable()
                .SingleOrDefaultAsync(x => x.Q == Q && x.R == R))).Write(writer);

            foreach (HexPoint neighbor in Region.Neighbors())
            {
                Star? star = await context.Stars
                    .AsWritable()
                    .SingleOrDefaultAsync(x => x.Q == neighbor.Q && x.R == neighbor.R);

                if (star is not null)
                {
                    (await context.CreateMessageAsync(resource: 18, star)).Write(writer);
                }
            }
        }
    }
}
