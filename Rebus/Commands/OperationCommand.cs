// Ishan Pranav's REBUS: OperationCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Commands
{
    public abstract class OperationCommand : Command
    {
        protected internal abstract Task<IWritable> UnexecuteAsync();
    }
}
