// Ishan Pranav's REBUS: BankCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    [Guid("07BFE879-ABE2-41CA-9A8F-2337E8DEB668")]
    internal sealed class BankCommand : Command
    {
        private readonly Controller _controller;

        public BankCommand(Controller controller)
        {
            _controller = controller;
        }

        private BankCommand(ArgumentSet arguments, Controller controller) : base(arguments)
        {
            _controller = controller;
        }

        public override Task ExecuteAsync(ExpressionWriter writer)
        {
            return _controller.ViewWealthAsync(writer, Arguments.PlayerId);
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new BankCommand(arguments, _controller);
        }
    }
}
