// Ishan Pranav's REBUS: VisionCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Rebus.Server.Concepts;

namespace Rebus.Server.Commands
{
    [Guid("6C4E330D-29AD-498B-B6A9-CB45724B0A32")]
    internal sealed class VisionCommand : Command
    {
        private readonly Controller _controller;
        private readonly DbRepository _repository;

        public VisionCommand(Controller controller, DbRepository repository)
        {
            _controller = controller;
            _repository = repository;
        }

        private VisionCommand(ArgumentSet arguments, Controller controller, DbRepository repository) : base(arguments)
        {
            _controller = controller;
            _repository = repository;
        }

        public override bool Matches(ArgumentSet arguments)
        {
            return arguments.Count == 1 && arguments.IsPlayerOrConcept(Argument.Subject);
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            IEnumerable<Fleet> fleets;

            if (Arguments.IsPlayer(Argument.Subject))
            {
                fleets = _repository.GetFleets(Arguments.PlayerId);
            }
            else
            {
                fleets = Arguments
                    .GetConcepts(Argument.Subject)
                    .OfType<Spacecraft>()
                    .GroupBy(x => x.Region)
                    .Select(x => new Fleet(Arguments.PlayerId, x));
            }

            foreach (Fleet fleet in fleets)
            {
                _controller.Report(writer, fleet);

                await _controller.ViewRegionAsync(writer, fleet.Region);
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new VisionCommand(arguments, _controller, _repository);
        }
    }
}
