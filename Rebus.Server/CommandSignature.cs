// Ishan Pranav's REBUS: CommandSignature.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rebus.Server
{
    [Table(nameof(CommandSignature))]
    internal sealed class CommandSignature
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string VerbValue { get; set; } = string.Empty;
        public string? AdverbValue { get; set; }
    }
}
