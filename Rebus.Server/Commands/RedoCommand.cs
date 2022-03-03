// Ishan Pranav's REBUS: RedoCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rebus.Server.Commands
{
    [Guid("5A0EE5FA-D968-49A2-AA29-8131A6B39AA9")]
    internal sealed class RedoCommand : Command, IExecutorProvider
    {
        public RedoCommand() { }
        private RedoCommand(ArgumentSet arguments) : base(arguments) { }

#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            if (Executor.Executables.TryPop(out IExecutable? result))
            {
                if (result is IUnexecutable unexecutable)
                {
                    Executor.Unexecutables.Push(unexecutable);
                }

                return result.ExecuteAsync();
            }
            else
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new RedoCommand(arguments);
        }
    }
}
