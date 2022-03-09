// Ishan Pranav's REBUS: Command.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server
{
    internal abstract class Command : IExecutable
    {
        public ArgumentSet Arguments { get; }

        public abstract Task ExecuteAsync(ExpressionWriter writer);

        protected Command()
        {
            Arguments = new ArgumentSet();
        }

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
