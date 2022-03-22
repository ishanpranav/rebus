// Ishan Pranav's REBUS: Spacecraft.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Concepts
{
    [Table(nameof(Spacecraft))]
    [Index(nameof(Q), nameof(R))]
    internal sealed class Spacecraft : Concept, ISpacecraft
    {
        public int Q { get; set; }
        public int R { get; set; }

#nullable disable
        [Required]
        public Player Player { get; set; }
#nullable enable

        public int PlayerId { get; set; }

        public override HexPoint Region
        {
            get
            {
                return new HexPoint(Q, R);
            }
        }

        HexPoint ISpacecraft.Region
        {
            get
            {
                return Region;
            }
            set
            {
                Q = value.Q;
                R = value.R;
            }
        }
    }
}
