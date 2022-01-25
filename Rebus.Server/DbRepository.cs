// Ishan Pranav's REBUS: DbRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Rebus.Expressions;

namespace Rebus.Server
{
    internal sealed class DbRepository
    {
        private readonly IDbContextFactory<UniverseContext> _contextFactory;
        private readonly MessageBuilder _messageBuilder;

        public DbRepository(IDbContextFactory<UniverseContext> contextFactory, MessageBuilder messageBuilder)
        {
            _contextFactory = contextFactory;
            _messageBuilder = messageBuilder;
        }

        public async Task<IReadOnlyCollection<Concept>> GetVisibleContentsAsync(int containerId, int viewerId)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                IQueryable<Concept> contents = context
                    .IncludeUniverse()
                    .Where(x => x.ContainerId == containerId);

                if (!await contents.AnyAsync(x => x.Reflective))
                {
                    contents = contents.Where(x => x.Id != viewerId);
                }

                return await contents.ToArrayAsync();
            }
        }

        public async Task<Concept> GetConceptAsync(int id)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context
                    .IncludeUniverse()
                    .SingleAsync(x => x.Id == id);
            }
        }

        public async Task<Concept> GetConceptAsync(IToken? article, IEnumerable<IToken> adjectives, IToken substantive)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                ILookup<int, ConceptSignature> signaturesByCount = context.ConceptSignatures
                    .Include(x => x.Article)
                    .Include(x => x.Substantive)
                    .Where(x => x.Substantive.Value == substantive.Value)
                    .Include(signature => signature.Adjectives)
                    .ToLookup(signature => adjectives.Count(adjective => signature.Adjectives.Any(x => adjective.Value == x.Value)));
                IEnumerable<int> conceptIds = signaturesByCount[adjectives.Count()].Select(x => x.ConceptId);
                Concept? result;

                try
                {
                    result = await context
                        .IncludeUniverse()
                        .SingleOrDefaultAsync(x => conceptIds.Contains(x.Id));
                }
                catch
                {
                    appendMessage();

                    _messageBuilder.Append(signaturesByCount[signaturesByCount.Max(x => x.Key)].First());

                    throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.ConceptMultipleMatchesException));
                }

                if (result is null)
                {
                    appendMessage();

                    throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.ConceptNoMatchesException));
                }
                else
                {
                    return result;
                }

                void appendMessage()
                {
                    _messageBuilder.Append(new NounExpression(Argument.Subject, article, adjectives, substantive));
                }
            }
        }

        public async Task<Token> GetTokenAsync(string value)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                return (await context.Tokens.SingleOrDefaultAsync(x => x.Value == value)) ?? new Token()
                {
                    Value = value
                };
            }
        }
    }
}
