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
        public IReadOnlyCollection<Format> Formats { get; }
        public IReadOnlyCollection<Token> Tokens { get; }

        public Seed(IReadOnlyCollection<CommandPrototype> commands, IReadOnlyCollection<Concept> concepts, IReadOnlyCollection<Format> formats, IReadOnlyCollection<Token> tokens)
        {
            Commands = commands;
            Concepts = concepts;
            Formats = formats;
            Tokens = tokens;
        }
    }
}
