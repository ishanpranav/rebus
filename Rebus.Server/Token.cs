// Ishan Pranav's REBUS: Token.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Rebus.Server
{
    [Index(nameof(Value), IsUnique = true)]
    internal class Token : Writable, IToken
    {
        public int Id { get; set; }
        public TokenTypes Type { get; set; }
        public string Value { get; set; } = String.Empty;

        public ICollection<ConceptSignature> Signatures { get; set; } = new HashSet<ConceptSignature>();

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(this.Value);
        }
    }
}