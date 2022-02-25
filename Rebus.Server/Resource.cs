// Ishan Pranav's REBUS: Resource.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Resource))]
    [Index(nameof(Key), nameof(Arguments))]
    internal sealed class Resource
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int Arguments { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
