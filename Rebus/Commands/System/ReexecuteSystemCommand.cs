// Ishan Pranav's REBUS: ReexecuteSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Commands.System
{
    [RebusCommand("g")]
    [RebusCommand("again")]
    [RebusCommand("reexecute")]
    public class ReexecuteSystemCommand : SystemCommand
    {
        protected internal override async Task<IWritable> ExecuteAsync()
        {
            await this.Executor.ReexecuteAsync();

            return null;
        }
    }
}
