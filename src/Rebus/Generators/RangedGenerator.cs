// Ishan Pranav's REBUS: RangedGenerator.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Generators
{
    internal abstract class RangedGenerator
    {
        protected abstract int Radius { get; }

        public virtual IEnumerable<HexPoint> CreateRange(HexPoint center)
        {
            return center.Range(Radius);
        }
    }
}
