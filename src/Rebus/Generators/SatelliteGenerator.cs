// Ishan Pranav's REBUS: SatelliteGenerator.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Generators
{
    public class SatelliteGenerator : Generator
    {
        public SatelliteGenerator(INamer namer) : base(namer) { }

        protected override int Radius
        {
            get
            {
                return 1;
            }
        }

        public override void Generate(HexPoint center, Map map)
        {
            foreach (HexPoint region in CreateRange(center))
            {
                map.Set(region, RegionType.Planetary, Namer.Name(degree: 1));
            }
        }
    }
}
