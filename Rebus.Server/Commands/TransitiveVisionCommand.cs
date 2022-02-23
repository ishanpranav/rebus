// Ishan Pranav's REBUS: TransitiveVisionCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Rebus.Server.Commands
{
    [Guid("72DEFA85-F66F-4049-A40C-C05C7496985A")]
    internal sealed class TransitiveVisionCommand : Command
    {
        private readonly VisionController _controller;

        public TransitiveVisionCommand(VisionController controller)
        {
            _controller = controller;
        }

        protected override IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            return _controller.ViewAsync(Player, GetConcept(Argument.Subject), GetConcept(Argument.DirectObject));
        }
    }
}
