// Ishan Pranav's REBUS: ZoneResult.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.Serialization;

namespace Rebus
{
    [DataContract]
    public class ZoneResult
    {
        [DataMember(Order = 0)]
        public Zone Value { get; }

        [DataMember(Order = 1)]
        public Depth Depth { get; }

        [DataMember(Order = 2)]
        public string? Name { get; }

        public ZoneResult(Zone value, Depth depth)
        {
            Value = value;
            Depth = depth;
        }

        public ZoneResult(Zone value, Depth depth, string? name) : this(value, depth)
        {
            Name = name;
        }
    }
}
