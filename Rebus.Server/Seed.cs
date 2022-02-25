// Ishan Pranav's REBUS: Seed.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server
{
    internal sealed class Seed
    {
        public IReadOnlyCollection<CommandPrototype> Commands { get; }
        public IReadOnlyCollection<Concept> Concepts { get; }
        public IReadOnlyCollection<Resource> Resources { get; }
        public IReadOnlyCollection<Token> Tokens { get; }

        public Seed(IReadOnlyCollection<CommandPrototype> commands, IReadOnlyCollection<Concept> concepts, IReadOnlyCollection<Resource> resources, IReadOnlyCollection<Token> tokens)
        {
            Commands = commands;
            Concepts = concepts;
            Resources = resources;
            Tokens = tokens;
        }
    }
}
