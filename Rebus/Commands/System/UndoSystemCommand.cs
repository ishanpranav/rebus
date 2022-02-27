// Ishan Pranav's REBUS: UndoSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rebus.Commands.System
{
    [Guid("86EEE5CE-5FFF-4420-9B3F-5A4DC96B54A0")]
    public class UndoSystemCommand : SystemCommand
    {
        protected internal override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            if (!Executor.Terminated && Executor.OperationCommands.TryPop(out OperationCommand? result))
            {
                return result.UnexecuteAsync();
            }
            else
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
        }
    }
}
