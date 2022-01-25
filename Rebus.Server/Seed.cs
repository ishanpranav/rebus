// Ishan Pranav's REBUS: Seed.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server
{
    internal sealed class Seed
    {
        public IReadOnlyCollection<Token> Tokens { get; } = new HashSet<Token>();
        public IReadOnlyCollection<Concept> Concepts { get; } = new HashSet<Concept>();

        public Seed(IReadOnlyCollection<Token> tokens, IReadOnlyCollection<Concept> concepts)
        {
            Tokens = tokens;
            Concepts = concepts;
        }
    }
}
