// Ishan Pranav's REBUS: ResourceMessageFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class ResourceMessageFactory : MessageFactory
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public ResourceMessageFactory(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public override async Task<IWritable> CreateMessageAsync(IConcept? player, IConcept? subject, int resource, params object[] arguments)
        {
            arguments = arguments
                .Where(x => x is not null && (x is not string @string || !string.IsNullOrWhiteSpace(@string)) && (x is not IEnumerable enumerable || enumerable
                    .Cast<object>()
                    .Any()))
                .ToArray();

            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                int count = arguments.Length;

                return new Message(player, subject, await context.Resources
                    .Where(x => x.Key == resource && x.Arguments == count)
                    .Select(x => x.Value)
                    .OrderBy(x => EF.Functions.Random())
                    .FirstOrDefaultAsync() ?? $"ERROR ({resource} : {count})", arguments);
            }
        }
    }
}
