// Ishan Pranav's REBUS: VisionCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    [Guid("6C4E330D-29AD-498B-B6A9-CB45724B0A32")]
    internal sealed class VisionCommand : Command
    {
        private readonly MessageBuilder _messageBuilder;
        private readonly DbRepository _repository;

        public VisionCommand(MessageBuilder messageBuilder, DbRepository repository)
        {
            _messageBuilder = messageBuilder;
            _repository = repository;
        }

        public static async Task<IWritable?> ExecuteAsync(MessageBuilder messageBuilder, DbRepository repository, IConcept player, IConcept subject, IConcept target)
        {
            messageBuilder.SetPrompt(player, subject);
            messageBuilder.Append(target);
            messageBuilder.Append(target.VisualDescription);

            IReadOnlyCollection<Concept> visibleContents = await repository.GetVisibleContentsAsync(target.Id, subject.Id);

            if (visibleContents.Count > 0)
            {
                messageBuilder.Append(visibleContents);

                return await messageBuilder.BuildAsync(ResourceIndex.VisionResponse);
            }
            else
            {
                return await messageBuilder.BuildAsync(ResourceIndex.VisionEmptyResponse);
            }
        }

        protected override async Task<IWritable?> ExecuteAsync()
        {
            IConcept subject = GetConcept(Argument.Subject);

            if (subject.ContainerId is int containerId)
            {
                return await ExecuteAsync(_messageBuilder, _repository, Player, subject, await _repository.GetConceptAsync(containerId));
            }
            else
            {
                return null;
            }
        }
    }
}
