// Ishan Pranav's REBUS: Fleet.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rebus.Server.Concepts;

namespace Rebus
{
    internal sealed class Fleet : IEnumerable<ISpacecraft>, ISpacecraft
    {
        private readonly HexPoint _region;
        private readonly IEnumerable<Spacecraft> _spacecraft;

        public int PlayerId { get; }

        public HexPoint Region
        {
            get
            {
                return _region;
            }
            set
            {
                foreach (Spacecraft spacecraft in _spacecraft)
                {
                    spacecraft.Q = value.Q;
                    spacecraft.R = value.R;
                }
            }
        }

        public Fleet(int playerId, IGrouping<HexPoint, Spacecraft> grouping)
        {
            PlayerId = playerId;
            _region = grouping.Key;
            _spacecraft = grouping;
        }

        public IEnumerator<ISpacecraft> GetEnumerator()
        {
            return _spacecraft.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
