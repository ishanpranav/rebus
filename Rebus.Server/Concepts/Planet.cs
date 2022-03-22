// Ishan Pranav's REBUS: Planet.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Concepts
{
    [Table(nameof(Planet))]
    [Index(nameof(Q), nameof(R), IsUnique = true)]
    internal sealed class Planet : Concept
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
