// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public abstract class Generator
    {
        protected abstract int Radius { get; }
        protected INamer Namer { get; }

        protected Generator(INamer namer)
        {
            Namer = namer;
        }

        public virtual IEnumerable<HexPoint> CreateRange(HexPoint center)
        {
            return center.Range(Radius);
        }

        public abstract void Generate(HexPoint center, Map map);
    }
}
