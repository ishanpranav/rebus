// Ishan Pranav's REBUS: Command.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus.Server
{
    internal abstract class Command : IExecutable
    {
#nullable disable
        public ArgumentSet Arguments { get; }
#nullable enable

        public Guid Guid
        {
            get
            {
                return GetType().GUID;
            }
        }

        public abstract IAsyncEnumerable<IWritable> ExecuteAsync();

        protected Command() { }

        protected Command(ArgumentSet arguments)
        {
            Arguments = arguments;
        }

        public virtual bool Matches(ArgumentSet arguments)
        {
            return arguments.Count == 1 && arguments.IsPlayer(Argument.Subject);
        }

        public abstract Command CreateCommand(ArgumentSet arguments);
    }
}
