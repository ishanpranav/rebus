// Ishan Pranav's REBUS: Token.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Token))]
    [Index(nameof(Value), IsUnique = true)]
    internal sealed class Token : Writable, IToken
    {
        public int Id { get; set; }
        public TokenTypes Type { get; set; }
        public string Value { get; set; } = string.Empty;

        public ICollection<ConceptSignature> Signatures { get; set; } = new HashSet<ConceptSignature>();

        public override void Write(ExpressionWriter writer)
        {
            if (Type.HasFlag(TokenTypes.Symbol))
            {
                writer.Write("use the ");
                writer.Write(Value.ToUpper());
                writer.Write(" shortcut");
            }
            else
            {
                writer.Write(Value);
            }
        }
    }
}
