// Ishan Pranav's REBUS: Direction.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server.Concepts
{
    internal sealed class Direction : Concept, ILocation
    {
        public async IAsyncEnumerable<HexPoint> NavigateAsync(ExpressionWriter writer, RebusDbContext context, HexPoint source)
        {
            (await context.CreateMessageAsync(resource: 20, Region)).Write(writer);

            yield return source + Region;
        }
    }
}
