// Ishan Pranav's REBUS: Navigation.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Navigation))]
    [Index(nameof(Q), nameof(R), nameof(PlayerId), IsUnique = true)]
    internal sealed class Navigation
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Q { get; set; }
        public int R { get; set; }

        public HexPoint Region
        {
            get
            {
                return new HexPoint(Q, R);
            }
        }
    }
}
