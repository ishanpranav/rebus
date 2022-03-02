// Ishan Pranav's REBUS: ICommandBuilder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public interface ICommandBuilder
    {
        IToken? Verb { get; set; }
        IToken? Adverb { get; set; }

        Task SetPlayerAsync(int player);

        void SetNumber(Argument argument, int value);
        void SetQuotation(Argument argument, string value);
        void SetReflexive(Argument argument);
        void SetConceptSignature(Argument argument, IEnumerable<IToken> adjectives, IToken substantive);

        Task SaveChangesAsync();
    }
}
