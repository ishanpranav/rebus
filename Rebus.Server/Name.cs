// Ishan Pranav's REBUS: CorpusEntry.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rebus.Server
{
    [Table(nameof(Name))]
    internal class Name
    {
        [Key]
        public string Value { get; set; } = string.Empty;
    }
}
