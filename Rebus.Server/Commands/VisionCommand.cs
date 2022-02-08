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
        private readonly IConceptRepository _repository;

        public VisionCommand(IConceptRepository repository)
        {
            _repository = repository;
        }

        public static async Task<IWritable?> ExecuteAsync(IConceptRepository repository, IConcept player, IConcept subject, IConcept target)
        {
            return new Message(player, subject, "It is {0}, {1}, that contains {2}.", new object[]
            {
                target,
                target.VisualDescription,
                await repository.GetVisibleContentsAsync(target.Id, subject.Id)
            });
        }

        protected override async Task<IWritable?> ExecuteAsync()
        {
            IConcept subject = GetConcept(Argument.Subject);

            if (subject.ContainerId is int containerId)
            {
                return await ExecuteAsync(_repository, Player, subject, await _repository.GetConceptAsync(containerId));
            }
            else
            {
                return null;
            }
        }
    }
}
