// Ishan Pranav's REBUS: CommandSignature.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rebus.Server
{
    [Table(nameof(CommandSignature))]
    internal sealed class CommandSignature
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

#nullable disable
        [Required]
        public Token Verb { get; set; }
#nullable enable

        public Token? Adverb { get; set; }
    }
}
