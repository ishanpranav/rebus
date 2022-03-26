// Ishan Pranav's REBUS: Resource.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Resource))]
    [Index(nameof(Key), nameof(Arguments), nameof(Value), IsUnique = true)]
    internal sealed class Resource
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public int Arguments { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
