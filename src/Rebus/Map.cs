// Ishan Pranav's REBUS: Map.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public class Map
    {
        private readonly Dictionary<HexPoint, string> _names = new Dictionary<HexPoint, string>();
        private readonly Dictionary<HexPoint, RegionType> _regionTypes = new Dictionary<HexPoint, RegionType>();

        public IEnumerable<HexPoint> Discoveries
        {
            get
            {
                return _names.Keys;
            }
        }

        public void Add(HexPoint region, RegionType type)
        {
            _regionTypes.Add(region, type);
        }

        public void Set(HexPoint region, RegionType type, string name)
        {
            _regionTypes[region] = type;
            _names[region] = name;
        }

        public RegionType GetRegionType(HexPoint region)
        {
            return _regionTypes[region];
        }

        public string GetName(HexPoint region)
        {
            return _names[region];
        }
    }
}
