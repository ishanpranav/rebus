// Ishan Pranav's REBUS: UndoCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rebus.Server.Commands
{
    [Guid("86EEE5CE-5FFF-4420-9B3F-5A4DC96B54A0")]
    internal sealed class UndoCommand : Command, IExecutorProvider
    {
#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public UndoCommand() { }
        private UndoCommand(ArgumentSet arguments) : base(arguments) { }

        public override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            if (Executor.Unexecutables.TryPop(out IUnexecutable? result))
            {
                return result.UnexecuteAsync();
            }
            else
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new UndoCommand(arguments);
        }
    }
}
