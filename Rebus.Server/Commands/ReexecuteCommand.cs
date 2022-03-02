// Ishan Pranav's REBUS: ReexecuteCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Rebus.Server.Concepts;

namespace Rebus.Server.Commands
{
    [Guid("AD7EF788-9CE6-4EA0-9A5B-7C3CFF1FB146")]
    internal sealed class ReexecuteCommand : Command, IExecutorProvider
    {
#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public ReexecuteCommand() { }
        private ReexecuteCommand(Player player) : base(player) { }

        public override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            if (Executor.Executable is null)
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
            else
            {
                return Executor.ExecuteAsync(Executor.Executable);
            }
        }

        protected override Command CreateCommand(Player player)
        {
            return new ReexecuteCommand(player);
        }
    }
}
