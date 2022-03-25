// Ishan Pranav's REBUS: NavigationCommand.cs
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
    [Guid("58D34F6D-B48D-49BC-88D7-0777E9039ABF")]
    internal sealed class NavigationCommand : Command
    {
        private readonly Controller _controller;

        public NavigationCommand(Controller controller)
        {
            _controller = controller;
        }

        private NavigationCommand(ArgumentSet arguments, Controller controller) : base(arguments)
        {
            _controller = controller;
        }

        public override bool Matches(ArgumentSet arguments)
        {
            return arguments.Count == 2 && arguments.TryGetConcepts(Argument.Subject, out IReadOnlyCollection<Concept>? subjects) && subjects.All(x => x is Spacecraft) && arguments.TryGetConcepts(Argument.DirectObject, out IReadOnlyCollection<Concept>? directObjects) && directObjects.Count == 1;
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            foreach (IGrouping<HexPoint, Spacecraft> grouping in Arguments
                .GetConcepts(Argument.Subject)
                .OfType<Spacecraft>()
                .GroupBy(x => x.Region))
            {
                Fleet fleet = new Fleet(Arguments.PlayerId, grouping);

                _controller.Report(writer, fleet);

                await _controller.NavigateAsync(
                    writer,
                    fleet,
                    Arguments
                        .GetConcepts(Argument.DirectObject)
                        .First().Region);
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new NavigationCommand(arguments, _controller);
        }
    }
}
