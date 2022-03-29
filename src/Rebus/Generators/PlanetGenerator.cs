// Ishan Pranav's REBUS: PlanetGenerator.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Generators
{
    public class PlanetGenerator : Generator
    {
        private readonly IPlanetNamer _namer;

        public PlanetGenerator(IPlanetNamer namer)
        {
            _namer = namer;
        }

        public override IEnumerable<HexPoint> CreateRange(HexPoint center)
        {
            yield return center;
        }

        public override void Generate(HexPoint center, Map map)
        {
            map.Set(center, RegionType.Planetary, _namer.NamePlanet(center));
        }
    }
}
