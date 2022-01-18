// Ishan Pranav's REBUS: UniverseOptions.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server
{
    internal class InitializerOptions
    {
        public IReadOnlyCollection<Token> Tokens { get; set; } = new HashSet<Token>();
        public IReadOnlyCollection<Concept> Concepts { get; set; } = new HashSet<Concept>();
    }
}
