// Ishan Pranav's REBUS: VisionCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Rebus.Server.Commands
{
    [Guid("6C4E330D-29AD-498B-B6A9-CB45724B0A32")]
    internal sealed class VisionCommand : Command
    {
        private readonly IConceptRepository _repository;
        private readonly VisionController _controller;

        public VisionCommand(IConceptRepository repository, VisionController controller)
        {
            _repository = repository;
            _controller = controller;
        }

        protected override async IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            IConcept subject = GetConcept(Argument.Subject);

            if (subject.ContainerId is int containerId)
            {
                await foreach (IWritable result in _controller.ViewAsync(Player, subject, await _repository.GetConceptAsync(containerId)))
                {
                    yield return result;
                }
            }
        }
    }
}
