// Ishan Pranav's REBUS: RedoSystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Commands.System
{
    [Guid("5A0EE5FA-D968-49A2-AA29-8131A6B39AA9")]
    public class RedoSystemCommand : SystemCommand
    {
        protected internal override Task<IWritable?> ExecuteAsync()
        {
            return Executor.RedoAsync();
        }
    }
}
