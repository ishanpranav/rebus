// Ishan Pranav's REBUS: FormatMessageFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

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

        public async Task<IWritable> CreateMessageAsync(IConcept? player, IConcept? subject, string resource, params object[] arguments)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                return new Message(player, subject, await context.Formats
                    .Where(x => x.Key == resource && x.Arguments == arguments.Length)
                    .Select(x => x.Value)
                    .OrderBy(x => EF.Functions.Random())
                    .FirstOrDefaultAsync() ?? resource, arguments);
            }
        }

        public override Task<IWritable> CreateMessageAsync(string resource, params object[] arguments)
        {
            return CreateMessageAsync(player: null, subject: null, resource, arguments);
        }
    }
}
