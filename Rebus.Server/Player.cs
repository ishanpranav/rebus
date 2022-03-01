// Ishan Pranav's REBUS: Player.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Player))]
    [Index(nameof(UserId), IsUnique = true)]
    [Guid("FA616059-61DE-42DB-AC8C-153F18270424")]
    internal sealed class Player : IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

#nullable disable
        [Required]
        public Concept Concept { get; set; }
#nullable enable

        public int ConceptId { get; set; }

        public ICollection<Spacecraft> Spacecraft { get; set; } = new HashSet<Spacecraft>();

        public void Write(ExpressionWriter writer)
        {
            Concept.Write(writer);
        }
    }
}
