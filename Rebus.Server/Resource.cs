// Ishan Pranav's REBUS: Resource.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;

namespace Rebus.Server
{
    [Table(nameof(Resource))]
    internal sealed class Resource
    {
        public int Key { get; set; }
        public int Arguments { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
