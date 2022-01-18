// Ishan Pranav's REBUS: Concept.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Rebus.Server
{
    internal class Concept : Writable, IConcept
    {
        public int Id { get; set; }
        public int? ContainerId { get; set; }
        public string Tag { get; set; } = String.Empty;
        public bool Reflective { get; set; }
        public Characteristics Characteristics { get; set; }

        public ICollection<ConceptSignature> Signatures { get; set; } = new HashSet<ConceptSignature>();

        public override void Write(ExpressionWriter writer)
        {
            ConceptSignature? signature = this.Signatures
                .OrderByDescending(x => x.Substantive.Value.Length)
                .ThenByDescending(x => x.Adjectives.Count)
                .FirstOrDefault();

            if (signature is not null)
            {
                signature.Write(writer);
            }
        }
    }
}
