// Ishan Pranav's REBUS: UndoSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Commands.System
{
    [Guid("86EEE5CE-5FFF-4420-9B3F-5A4DC96B54A0")]
    public class UndoSystemCommand : SystemCommand
    {
        protected internal override Task<IWritable?> ExecuteAsync()
        {
            return Executor.UndoAsync();
        }
    }
}
