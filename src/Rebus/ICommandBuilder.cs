// Ishan Pranav's REBUS: ICommandBuilder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public interface ICommandBuilder
    {
        IToken? Verb { get; set; }
        IToken? Adverb { get; set; }
        Argument Argument { get; set; }

        void Add(int number);
        void Add(string quotation);
        void Add(IReadOnlyCollection<IToken> adjectives, IToken substantive);
        void AddReflexive();

        void MoveNext();
    }
}
