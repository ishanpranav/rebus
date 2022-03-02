// Ishan Pranav's REBUS: RedoCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Rebus.Server.Concepts;

namespace Rebus.Server.Commands
{
    [Guid("5A0EE5FA-D968-49A2-AA29-8131A6B39AA9")]
    internal sealed class RedoCommand : Command, IExecutorProvider
    {
        public RedoCommand() { }
        private RedoCommand(Player player) : base(player) { }

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

        protected override Command CreateCommand(Player player)
        {
            return new RedoCommand(player);
        }
    }
}
