// Ishan Pranav's REBUS: ConceptSignature.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rebus.Expressions;

namespace Rebus.Server
{
    [Table(nameof(ConceptSignature))]
    internal sealed class ConceptSignature : Writable
    {
        public int Id { get; set; }

        public Token? Article { get; set; }

        public ICollection<Token> Adjectives { get; set; } = new HashSet<Token>();
        public ICollection<Concept> Concepts { get; set; } = new HashSet<Concept>();

#nullable disable
        [Required]
        public Token Substantive { get; set; }
#nullable enable

        public override void Write(ExpressionWriter writer)
        {
            new NounExpression(Argument.None, Article, Adjectives, Substantive).Write(writer);
        }
    }
}
