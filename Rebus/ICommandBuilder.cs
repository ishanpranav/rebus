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
        IArgumentSet Arguments { get; }

        Task SetPlayerAsync(int id);

        void Set(Argument argument, IReadOnlyCollection<IToken> adjectives, IToken substantive);

        void MoveNext();
    }
}
