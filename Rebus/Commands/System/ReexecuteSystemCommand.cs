// Ishan Pranav's REBUS: ReexecuteSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rebus.Commands.System
{
    [Guid("AD7EF788-9CE6-4EA0-9A5B-7C3CFF1FB146")]
    public class ReexecuteSystemCommand : SystemCommand
    {
        protected internal override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            if (Executor.Terminated || Executor.Command is null)
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
            else
            {
                return Executor.ExecuteAsync(Executor.Command);
            }
        }
    }
}
