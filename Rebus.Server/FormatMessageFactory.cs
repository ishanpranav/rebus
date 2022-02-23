// Ishan Pranav's REBUS: FormatMessageFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class FormatMessageFactory : MessageFactory
    {
        private readonly IDbContextFactory<UniverseContext> _contextFactory;

        public FormatMessageFactory(IDbContextFactory<UniverseContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public override async Task<IWritable> CreateMessageAsync(IConcept? player, IConcept? subject, int resource, params object[] arguments)
        {
            arguments = arguments
                .Where(x => x is not IEnumerable enumerable || enumerable
                    .Cast<object>()
                    .Any())
                .ToArray();

            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                return new Message(player, subject, await context.Formats
                    .Where(x => x.Resource == resource && x.Arguments == arguments.Length)
                    .Select(x => x.Value)
                    .OrderBy(x => EF.Functions.Random())
                    .FirstOrDefaultAsync() ?? $"ERROR 0x{resource:x}", arguments);
            }
        }
    }
}
