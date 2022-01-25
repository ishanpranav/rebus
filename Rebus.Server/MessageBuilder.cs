// Ishan Pranav's REBUS: MessageBuilder.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class MessageBuilder
    {
        private readonly List<object> _arguments = new List<object>();
        private readonly IDbContextFactory<ResourceContext> _contextFactory;

        private IConcept? _player;
        private IConcept? _subject;

        public MessageBuilder(IDbContextFactory<ResourceContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void SetPrompt(IConcept player, IConcept subject)
        {
            _player = player;
            _subject = subject;
        }

        public void Append(object argument)
        {
            _arguments.Add(argument);
        }

        public async Task<IWritable?> BuildAsync(ResourceIndex index)
        {
            await using (ResourceContext context = await _contextFactory.CreateDbContextAsync())
            {
                Message? result = null;
                string? resource = await context.Resources
                    .Where(x => x.Index == index)
                    .Select(x => x.Value)
                    .OrderBy(x => EF.Functions.Random())
                    .FirstOrDefaultAsync();

                if (resource is not null)
                {
                    result = new Message(_player, _subject, resource, _arguments.ToArray());
                }

                _player = null;
                _subject = null;

                _arguments.Clear();

                return result;
            }
        }
    }
}
