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

        void Include(IReadOnlyCollection<IToken> adjectives, IToken substantive);
        void Include(int number);
        void Include(string quotation);
        void IncludeReflexive();

        void MoveNext();
    }
}
