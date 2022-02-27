// Ishan Pranav's REBUS: Concept.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Rebus.Server
{
    [Table(nameof(Concept))]
    internal abstract class Concept : Writable, ICloneable, IConcept
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

#nullable disable
        [Required]
        public Player Player { get; set; }
#nullable enable

        public ICollection<ConceptSignature> Signatures { get; set; } = new HashSet<ConceptSignature>();

        public override void Write(ExpressionWriter writer)
        {
            Signatures
                .OrderByDescending(x => x.Substantive.Value.Length)
                .ThenByDescending(x => x.Adjectives.Count)
                .FirstOrDefault()?
                .Write(writer);
        }

        public Concept CreateConcept()
        {
            Concept result = (Concept)Clone();

            result.Id = 0;
            result.Signatures = new HashSet<ConceptSignature>(Signatures);

            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
