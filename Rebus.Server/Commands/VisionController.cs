// Ishan Pranav's REBUS: VisionController.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server.Commands
{
    internal sealed class VisionController
    {
        private readonly IConceptRepository _repository;
        private readonly MessageFactory _messageFactory;

        public VisionController(IConceptRepository repository, MessageFactory messageFactory)
        {
            _repository = repository;
            _messageFactory = messageFactory;
        }

        public async IAsyncEnumerable<IWritable> ViewAsync(IConcept player, IConcept subject, IConcept target)
        {
            yield return await _messageFactory.CreateMessageAsync(player, subject, Resources.VisualDescription, new object[]
            {
                target,
                target.VisualDescription
            });

            IReadOnlyCollection<IConcept> visibleContents = await _repository.GetVisibleContentsAsync(target.Id, subject.Id);

            if (visibleContents.Count > 0)
            {
                yield return await _messageFactory.CreateMessageAsync(player, subject, Resources.VisualDescriptionContents, new object[]
                {
                    visibleContents
                });
            }
        }
    }
}
