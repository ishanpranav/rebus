// Ishan Pranav's REBUS: Zone.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [DataContract]
    [Index(nameof(Q), nameof(R))]
    [Table(nameof(Zone))]
    public class Zone
    {
        [DataMember(Order = 0)]
        public int Q { get; set; }

        [DataMember(Order = 1)]
        public int R { get; set; }

        public HexPoint Location
        {
            get
            {
                return new HexPoint(Q, R);
            }
        }

        [DataMember(Order = 2)]
        public int PlayerId { get; set; }

#nullable disable
        public Player Player { get; set; }
#nullable enable

        [DataMember(Order = 3)]
        public ICollection<Unit> Units { get; set; } = new HashSet<Unit>();
    }
}
