// Ishan Pranav's REBUS: HexPointPathfinder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Pathfinders
{
    public class HexPointPathfinder : AStarPathfinder<HexPoint>
    {
        protected override int Heuristic(HexPoint source, HexPoint destination)
        {
            return HexPoint.Distance(source, destination);
        }

        protected override IEnumerable<HexPoint> Neighbors(HexPoint value)
        {
            return value.Neighbors();
        }
    }
}
