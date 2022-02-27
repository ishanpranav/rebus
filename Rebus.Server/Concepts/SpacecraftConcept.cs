// Ishan Pranav's REBUS: SpacecraftConcept.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Rebus.Server.Concepts
{
    internal sealed class SpacecraftConcept : VisibleConcept
    {
        public int RegionId { get; set; }

#nullable disable
        [Required]
        public RegionConcept Region { get; set; }
#nullable enable
    }
}
