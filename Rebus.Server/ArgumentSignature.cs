// Ishan Pranav's REBUS: ArgumentSignature.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(ArgumentSignature))]
    [Index(nameof(Argument), nameof(Type), IsUnique = true)]
    internal sealed class ArgumentSignature
    {
        public int Id { get; set; }
        public Argument Argument { get; set; }
        public ArgumentType Type { get; set; }

        public ICollection<CommandSignature> Commands { get; set; } = new HashSet<CommandSignature>();
    }
}
