// Ishan Pranav's REBUS: Player.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Concepts
{
    [Table(nameof(Player))]
    [Index(nameof(UserId), IsUnique = true)]
    [Guid("FA616059-61DE-42DB-AC8C-153F18270424")]
    internal sealed class Player : Concept
    {
        public string UserId { get; set; } = string.Empty;

        public ICollection<Spacecraft> Spacecraft { get; set; } = new HashSet<Spacecraft>();
    }
}
