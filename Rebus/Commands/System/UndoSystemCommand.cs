// Ishan Pranav's REBUS: UndoSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Commands.System
{
    [RebusCommand("undo")]
    public class UndoSystemCommand : SystemCommand
    {
        protected internal override async Task<IWritable> ExecuteAsync()
        {
            await this.Executor.UndoAsync();

            return null;
        }
    }
}
