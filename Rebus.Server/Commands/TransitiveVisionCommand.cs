// Ishan Pranav's REBUS: TransitiveVisionCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    [Guid("72DEFA85-F66F-4049-A40C-C05C7496985A")]
    internal sealed class TransitiveVisionCommand : Command
    {
        private readonly MessageBuilder _messageBuilder;
        private readonly DbRepository _repository;

        public TransitiveVisionCommand(MessageBuilder messageBuilder, DbRepository repository)
        {
            _messageBuilder = messageBuilder;
            _repository = repository;
        }

        protected override Task<IWritable?> ExecuteAsync()
        {
            return VisionCommand.ExecuteAsync(_messageBuilder, _repository, Player, GetConcept(Argument.Subject), GetConcept(Argument.DirectObject));
        }
    }
}
