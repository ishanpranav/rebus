// Ishan Pranav's REBUS: Format.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Format))]
    [Index(nameof(Resource), nameof(Arguments))]
    internal sealed class Format
    {
        public int Id { get; set; }
        public int Resource { get; set; }
        public int Arguments { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
