// Ishan Pranav's REBUS: CommandSignature.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rebus.Server
{
    [Table(nameof(CommandSignature))]
    internal sealed class CommandSignature
    {
        public int Id { get; set; }

#nullable disable
        [Required]
        public Command Command { get; set; }

        [Required]
        public Token Verb { get; set; }
#nullable enable

        public Token? Adverb { get; set; }

        public ICollection<ArgumentSignature> Arguments { get; set; } = new HashSet<ArgumentSignature>();
    }
}
