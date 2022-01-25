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
        Task SetConceptAsync(Argument argument, IEnumerable<IToken> adjectives, IToken substantive);

        void SetNumber(Argument argument, int value);
        void SetQuotation(Argument argument, string value);
        void SetReflexive(Argument argument);

        Task SaveChangesAsync();

        IEnumerable<Command> Build();
    }
}
