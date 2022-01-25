// Ishan Pranav's REBUS: CommandSignature.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Index(nameof(Verb))]
    [Table(nameof(CommandSignature))]
    internal sealed class CommandSignature : Writable
    {
        public int Id { get; set; }

#nullable disable
        [Required]
        public CommandPrototype Command { get; set; }
#nullable enable

        public string Verb { get; set; } = string.Empty;
        public string? Adverb { get; set; }
        public ICollection<ArgumentSignature> Arguments { get; set; } = new HashSet<ArgumentSignature>();

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(Verb.ToUpper());

            if (Adverb is not null)
            {
                writer.Write(' ');
                writer.Write(Adverb.ToUpper());
            }

            foreach (ArgumentSignature argument in Arguments)
            {
                argument.Write(writer);
            }
        }
    }
}
