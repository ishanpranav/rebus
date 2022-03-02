// Ishan Pranav's REBUS: Token.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rebus.Server
{
    [Table(nameof(Token))]
    internal sealed class Token : Writable, IToken
    {
        public TokenTypes Type { get; set; }

        [Key]
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
