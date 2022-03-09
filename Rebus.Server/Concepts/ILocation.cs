// Ishan Pranav's REBUS: ILocation.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server.Concepts
{
    internal interface ILocation
    {
        IAsyncEnumerable<HexPoint> NavigateAsync(ExpressionWriter writer, RebusDbContext context, HexPoint source);
    }
}
