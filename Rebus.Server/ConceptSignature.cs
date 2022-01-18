// Ishan Pranav's REBUS: ConceptSignature.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Rebus.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rebus.Server
{
    internal class ConceptSignature : Writable
    {
        public int Id { get; set; }
        public int ConceptId { get; set; }

#nullable disable
        [Required]
        public Token Substantive { get; set; }
#nullable enable

        public ICollection<Token> Adjectives { get; set; } = new HashSet<Token>();

        public override void Write(ExpressionWriter writer)
        {
            new SubjectExpression(this.Adjectives, this.Substantive).Write(writer);
        }
    }
}
