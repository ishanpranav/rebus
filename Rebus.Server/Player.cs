// Ishan Pranav's REBUS: Player.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Player))]
    [Index(nameof(UserId), IsUnique = true)]
    internal sealed class Player : IPlayer
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        public int ConceptId { get; set; }

        [Required]
        public Concept? Concept { get; set; }
    }
}
