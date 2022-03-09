// Ishan Pranav's REBUS: NavigationCommand.cs
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
    [Guid("58D34F6D-B48D-49BC-88D7-0777E9039ABF")]
    internal sealed class NavigationCommand : Command
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public NavigationCommand(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private NavigationCommand(ArgumentSet arguments, IDbContextFactory<RebusDbContext> contextFactory) : base(arguments)
        {
            _contextFactory = contextFactory;
        }

        public override bool Matches(ArgumentSet arguments)
        {
            return arguments.Count == 2 && arguments.IsConcept(Argument.Subject) && arguments.TryGetConcepts(Argument.DirectObject, out IReadOnlyCollection<Concept>? directObjects) && directObjects.Count == 1 && directObjects.First() is ILocation;
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                ILocation location = Arguments
                    .GetConcepts(Argument.DirectObject)
                    .OfType<ILocation>()
                    .First();

                foreach (IGrouping<HexPoint, IMobile> grouping in Arguments
                    .GetConcepts(Argument.Subject)
                    .OfType<IMobile>()
                    .GroupBy(x => x.Region))
                {
                    (await context.CreateMessageAsync(resource: 19, grouping)).Write(writer);

                    await foreach (HexPoint region in location.NavigateAsync(writer, context, grouping.Key))
                    {
                        foreach (IMobile mobile in grouping)
                        {
                            mobile.Region = region;

                            context.Update(mobile);
                        }
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new NavigationCommand(arguments, _contextFactory);
        }
    }
}
