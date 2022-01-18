// Ishan Pranav's REBUS: RedoSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Commands.System
{
    [RebusCommand("redo")]
    public class RedoSystemCommand : SystemCommand
    {
        protected internal override async Task<IWritable> ExecuteAsync()
        {
            await this.Executor.RedoAsync();

            return null;
        }
    }
}
