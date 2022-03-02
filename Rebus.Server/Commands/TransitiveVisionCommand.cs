// Ishan Pranav's REBUS: TransitiveVisionCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Rebus.Server.Concepts;

namespace Rebus.Server.Commands
{
    [Guid("72DEFA85-F66F-4049-A40C-C05C7496985A")]
    internal sealed class TransitiveVisionCommand : Command
    {
        public TransitiveVisionCommand() { }
        private TransitiveVisionCommand(Player player) : base(player) { }

        public override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            return AsyncEnumerable.Empty<IWritable>();
        }

        protected override Command CreateCommand(Player player)
        {
            return new TransitiveVisionCommand(player);
        }
    }
}
