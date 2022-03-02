// Ishan Pranav's REBUS: ConceptSignature.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Rebus.Expressions;
using Rebus.Server.Concepts;

namespace Rebus.Server
{
    [Table(nameof(ConceptSignature))]
    [Index(nameof(Priority))]
    internal sealed class ConceptSignature : Writable
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public Token? Article { get; set; }

        public ICollection<Token> Adjectives { get; set; } = new HashSet<Token>();

#nullable disable
        [Required]
        public Token Substantive { get; set; }
#nullable enable

        public Spacecraft? Spacecraft { get; set; }

        public override void Write(ExpressionWriter writer)
        {
            new NounExpression(Argument.None, Article, Adjectives, Substantive).Write(writer);
        }
    }
}
