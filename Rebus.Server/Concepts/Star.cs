// Ishan Pranav's REBUS: Star.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Concepts
{
    [Table(nameof(Star))]
    [Index(nameof(Q), nameof(R), IsUnique = true)]
    internal sealed class Star : Concept
    {
        public int Q { get; set; }
        public int R { get; set; }

        public override HexPoint Region
        {
            get
            {
                return new HexPoint(Q, R);
            }
        }
    }
}
