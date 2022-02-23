// Ishan Pranav's REBUS: OperationCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Commands
{
    public abstract class OperationCommand : Command
    {
        protected internal abstract IAsyncEnumerable<IWritable> UnexecuteAsync();
    }
}
