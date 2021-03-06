// Ishan Pranav's REBUS: HexPointSearch.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server.Searches
{
    /// <summary>
    /// Performs the A* search algorithm to find paths between <see cref="HexPoint"/> values.
    /// </summary>
    public class HexPointSearch : AStarSearch<HexPoint>
    {
        /// <inheritdoc/>
        protected override int Heuristic(HexPoint source, HexPoint destination)
        {
            return HexPoint.Distance(source, destination);
        }

        /// <inheritdoc/>
        protected override IEnumerable<HexPoint> GetNeighbors(HexPoint value)
        {
            return value.Neighbors();
        }
    }
}
