// Ishan Pranav's REBUS: Player.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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

        public ICollection<Concept> Concepts { get; set; } = new HashSet<Concept>();

        public Player() { }

        public Player(Concept concept)
        {
            Concepts.Add(concept);
        }

        public MicrocomputerConcept GetConcept()
        {
            return Concepts
                .OfType<MicrocomputerConcept>()
                .Single();
        }
    }
}
