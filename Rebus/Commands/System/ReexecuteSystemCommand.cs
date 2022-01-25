// Ishan Pranav's REBUS: ReexecuteSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Commands.System
{
    [Guid("AD7EF788-9CE6-4EA0-9A5B-7C3CFF1FB146")]
    public class ReexecuteSystemCommand : SystemCommand
    {
        protected internal override Task<IWritable?> ExecuteAsync()
        {
            return Executor.ReexecuteAsync();
        }
    }
}
