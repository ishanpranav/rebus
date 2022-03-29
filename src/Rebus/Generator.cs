// Ishan Pranav's REBUS: Generator.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public abstract class Generator
    {
        public abstract IEnumerable<HexPoint> CreateRange(HexPoint center);

        public abstract void Generate(HexPoint center, Map map);
    }
}
