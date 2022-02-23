// Ishan Pranav's REBUS: Seed.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server
{
    internal sealed class Seed
    {
        public IReadOnlyCollection<CommandPrototype> Commands { get; set; }
        public IReadOnlyCollection<Concept> Concepts { get; set; }
        public IReadOnlyCollection<Format> Formats { get; set; }
        public IReadOnlyCollection<Token> Tokens { get; set; }
    }
}
