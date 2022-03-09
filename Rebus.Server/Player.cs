// Ishan Pranav's REBUS: Player.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Rebus.Server.Concepts;

namespace Rebus.Server
{
    [Table(nameof(Player))]
    [Index(nameof(UserId), IsUnique = true)]
    internal sealed class Player
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Credential { get; set; } = string.Empty;

        public ICollection<Spacecraft> Spacecraft { get; set; } = new HashSet<Spacecraft>();
    }
}
