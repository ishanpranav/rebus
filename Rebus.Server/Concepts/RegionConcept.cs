// Ishan Pranav's REBUS: RegionConcept.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus.Server.Concepts
{
    internal sealed class RegionConcept : VisibleConcept
    {
        public ICollection<SpacecraftConcept> Spacecraft { get; set; } = new HashSet<SpacecraftConcept>();

        
    }
}
