// Ishan Pranav's REBUS: Adjective.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rebus.Server
{
    [Table(nameof(Adjective))]
    internal sealed class Adjective
    {
        public int ConceptId { get; set; }

#nullable disable
        [Required]
        public Token Token { get; set; }

        [Required]
        public string TokenValue { get; set; }
#nullable enable
    }
}
