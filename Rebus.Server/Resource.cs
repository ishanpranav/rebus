// Ishan Pranav's REBUS: Resource.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Resource))]
    [Index(nameof(Index), nameof(Value), IsUnique = true)]
    internal sealed class Resource
    {
        public int Id { get; set; }
        public ResourceIndex Index { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
