// Ishan Pranav's REBUS: VisionCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Server.Concepts;

namespace Rebus.Server.Commands
{
    [Guid("6C4E330D-29AD-498B-B6A9-CB45724B0A32")]
    internal sealed class VisionCommand : Command
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public VisionCommand(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private VisionCommand(ArgumentSet arguments, IDbContextFactory<RebusDbContext> contextFactory) : base(arguments)
        {
            _contextFactory = contextFactory;
        }

        public override bool Matches(ArgumentSet arguments)
        {
            return arguments.Count == 1 && arguments.IsPlayerOrConcept(Argument.Subject);
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                IEnumerable<Concept> subjects;

                if (Arguments.IsPlayer(Argument.Subject))
                {
                    subjects = context
                        .GetKnownViewers()
                        .Where(x => x.PlayerId == Arguments.Player.Id)
                        .OrderBy(x => x.SubstantiveValue)
                        .AsWritable();
                }
                else
                {
                    subjects = Arguments.GetConcepts(Argument.Subject);
                }

                foreach (IGrouping<HexPoint, IViewer> grouping in subjects
                    .OfType<IViewer>()
                    .GroupBy(x => x.Region))
                {
                    (await context.CreateMessageAsync(resource: 19, grouping)).Write(writer);

                    await grouping
                        .First()
                        .ViewAsync(writer, context);
                }
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new VisionCommand(arguments, _contextFactory);
        }
    }
}
