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

#nullable disable
        [Key]
        public string Value { get; set; }
#nullable enable

        public ICollection<Adjective> Adjectives { get; set; } = new HashSet<Adjective>();

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
