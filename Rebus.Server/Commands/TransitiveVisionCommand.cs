// Ishan Pranav's REBUS: TransitiveVisionCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rebus.Server.Commands
{
    [Guid("72DEFA85-F66F-4049-A40C-C05C7496985A")]
    internal sealed class TransitiveVisionCommand : Rebus.Command
    {
        protected override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            return AsyncEnumerable.Empty<IWritable>();
        }
    }
}
