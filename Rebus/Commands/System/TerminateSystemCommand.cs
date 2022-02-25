// Ishan Pranav's REBUS: TerminateSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rebus.Commands.System
{
    [Guid("C11C8380-546C-457A-8D58-1721CA154320")]
    public class TerminateSystemCommand : SystemCommand
    {
        protected internal override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            Executor.Terminate();

            return AsyncEnumerable.Empty<IWritable>();
        }
    }
}
