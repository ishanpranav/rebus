// Ishan Pranav's REBUS: Command.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Command))]
    [Index(nameof(Guid), IsUnique = true)]
    internal sealed class Command
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public ICollection<CommandSignature> Signatures { get; set; } = new HashSet<CommandSignature>();
    }
}
