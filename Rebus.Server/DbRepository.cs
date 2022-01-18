// Ishan Pranav's REBUS: Repository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Rebus.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rebus.Server
{
    internal class DbRepository
    {
        private readonly IDbContextFactory<UniverseContext> _contextFactory;
        private readonly MessageBuilder<DbRepository> _messageBuilder;

        public DbRepository(IDbContextFactory<UniverseContext> contextFactory, MessageBuilder<DbRepository> messageBuilder)
        {
            this._contextFactory = contextFactory;
            this._messageBuilder = messageBuilder;
        }

        public async Task<IConcept> GetPlayerAsync(string tag)
        {
            await using (UniverseContext context = await this._contextFactory.CreateDbContextAsync())
            {
                Concept? result = await context
                    .IncludeUniverse()
                    .SingleOrDefaultAsync(x => x.Tag == tag);

                if (result is null)
                {
                    result = new Concept()
                    {
                        ContainerId = 1,
                        Characteristics = Characteristics.Agent,
                        Tag = tag,
                        Signatures = new ConceptSignature[]
                        {
                            new ConceptSignature()
                            {
                                Substantive = new Token()
                                {
                                    Type = TokenTypes.Substantive,
                                    Value = tag
                                }
                            }
                        }
                    };

                    await context.AddAsync(result);
                    await context.SaveChangesAsync();
                }

                return result;
            }
        }

        public async Task<IReadOnlyCollection<Concept>> GetVisibleContents(int containerId, int viewerId)
        {
            await using (UniverseContext context = await this._contextFactory.CreateDbContextAsync())
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

        public async Task<Concept> GetConceptAsync(IEnumerable<IToken> adjectives, IToken substantive, Characteristics characteristics)
        {
            await using (UniverseContext context = await this._contextFactory.CreateDbContextAsync())
            {
                ILookup<int, ConceptSignature> signaturesByCount = context.ConceptSignatures
                    .Include(x => x.Substantive)
                    .Where(x => x.Substantive.Value == substantive.Value)
                    .Include(signature => signature.Adjectives)
                    .ToLookup(signature => adjectives.Count(adjective => signature.Adjectives.Any(x => adjective.Value == x.Value)));
                IEnumerable<int> conceptIds = signaturesByCount[adjectives.Count()].Select(x => x.ConceptId);
                IQueryable<Concept> results = context
                    .IncludeUniverse()
                    .Where(concept => conceptIds.Contains(concept.Id));

                if (await results.AnyAsync())
                {
                    Concept? result;

                    try
                    {
                        result = await results
                            .Where(x => x.Characteristics.HasFlag(characteristics))
                            .SingleOrDefaultAsync();
                    }
                    catch
                    {
                        this._messageBuilder.Begin(1, 2);
                        this._messageBuilder.Append(new SubjectExpression(adjectives, substantive));
                        this._messageBuilder.Append(signaturesByCount[signaturesByCount.Max(x => x.Key)].First());

                        throw new RebusException(this._messageBuilder.Build());
                    }
                    
                    this._messageBuilder.Begin(2, 2);
                    this._messageBuilder.Append(new SubjectExpression(adjectives, substantive));
                    this._messageBuilder.Append(characteristics);

                    return result ?? throw new RebusException(this._messageBuilder.Build());
                }
                else
                {
                    this._messageBuilder.Begin(0, 1);
                    this._messageBuilder.Append(new SubjectExpression(adjectives, substantive));

                    throw new RebusException(this._messageBuilder.Build());
                }
            }
        }

        public async Task<Token> GetTokenAsync(string value)
        {
            await using (UniverseContext context = await this._contextFactory.CreateDbContextAsync())
            {
                return (await context.Tokens.SingleOrDefaultAsync(x => x.Value == value)) ?? new Token()
                {
                    Value = value
                };
            }
        }
    }
}
