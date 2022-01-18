// Ishan Pranav's REBUS: VisionCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    [RebusCommand("look")]
    [RebusCommand("look", "around")]
    internal class VisionCommand : Command
    {
        private readonly MessageBuilder<VisionCommand> _messageBuilder;
        private readonly DbRepository _repository;

        public VisionCommand(MessageBuilder<VisionCommand> messageBuilder, DbRepository repository)
        {
            this._messageBuilder = messageBuilder;
            this._repository = repository;
        }

        protected override async Task<IWritable> ExecuteAsync()
        {
            this._messageBuilder.Begin(0, 1);

            if (this.Subject.ContainerId is int containerId)
            {
                this._messageBuilder.Append(await this._repository.GetVisibleContents(containerId, this.Subject.Id));
            }

            return this._messageBuilder.Build();
        }
    }
}
