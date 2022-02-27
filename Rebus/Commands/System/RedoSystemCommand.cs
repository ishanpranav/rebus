// Ishan Pranav's REBUS: RedoSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rebus.Commands.System
{
    [Guid("5A0EE5FA-D968-49A2-AA29-8131A6B39AA9")]
    public class RedoSystemCommand : SystemCommand
    {
        protected internal override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            if (!Executor.Terminated && Executor.Commands.TryPop(out Command? result))
            {
                if (result is OperationCommand operationCommand)
                {
                    Executor.OperationCommands.Push(operationCommand);
                }

                return result.ExecuteAsync();
            }
            else
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
        }
    }
}
