// Ishan Pranav's REBUS: Concept.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Rebus.Server
{
    internal abstract class Concept : Writable
    {
        public int Id { get; set; }

        public ICollection<ConceptSignature> Signatures { get; set; } = new HashSet<ConceptSignature>();

        public override void Write(ExpressionWriter writer)
        {
            Signatures
                .MinBy(x => x.Priority)?
                .Write(writer);
        }
    }
}
