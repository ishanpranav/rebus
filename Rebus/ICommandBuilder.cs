// Ishan Pranav's REBUS: ICommandBuilder.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public interface ICommandBuilder
    {
        Task SetVerbPhraseAsync(IToken verb, IToken? adverb);
        Task SetSubjectAsync(IEnumerable<IToken> adjectives, IToken substantive);
        Task SetDirectObjectAsync(IEnumerable<IToken> adjectives, IToken substantive);

        void SetReflexive();

        void MoveNext();

        IEnumerable<Command> Build();
    }
}