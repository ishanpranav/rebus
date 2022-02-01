// Ishan Pranav's REBUS: Repository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class Repository
    {
        private readonly IDbContextFactory<UniverseContext> _contextFactory;
        private readonly MessageFactory _messageFactory;

        public Repository(IDbContextFactory<UniverseContext> contextFactory, MessageFactory messageFactory)
        {
            _contextFactory = contextFactory;
            _messageFactory = messageFactory;
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

        public async Task<Concept> GetConceptAsync(IEnumerable<IToken> adjectives, IToken substantive)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                ILookup<int, ConceptSignature> signaturesByCount = context.ConceptSignatures
                    .Include(signature => signature.Substantive)
                    .Where(signature => signature.Substantive.Value == substantive.Value)
                    .Include(signature => signature.Article)
                    .Include(signature => signature.Adjectives)
                    .AsSplitQuery()
                    .Include(signature => signature.Concepts)
                    .ToLookup(signature => adjectives.Count(adjective => signature.Adjectives.Any(x => adjective.Value == x.Value)));

                ConceptSignature? signature;

                try
                {
                    signature = signaturesByCount[adjectives.Count()].SingleOrDefault();
                }
                catch
                {
                    throw await _messageFactory.CreateExceptionAsync("MultipleMatchesConcept");
                }

                if (signature is null)
                {
                    throw await _messageFactory.CreateExceptionAsync("NoMatchesConcept");
                }
                else
                {
                    return await context
                        .IncludeUniverse()
                        .SingleAsync(x => x.Id == signature.Concepts.First().Id);
                }
            }
        }

        public async Task<CommandSignature> GetCommandAsync(IToken verb, IToken? adverb, object?[] arguments)
        {
            CommandSignature? result;

            try
            {
                await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
                {
                    int argumentCount = arguments.Length;

                    result = context.CommandSignatures
                        .Where(signature => signature.Verb.Value == verb.Value && signature.Adverb == adverb)
                        .Include(signature => signature.Command)
                        .Include(signature => signature.Arguments)
                        .AsEnumerable()
                        .SingleOrDefault(signature =>
                        {
                            ArgumentSignature?[] argumentSignatures = new ArgumentSignature?[argumentCount];

                            foreach (ArgumentSignature argumentSignature in signature.Arguments)
                            {
                                argumentSignatures[(int)argumentSignature.Argument] = argumentSignature;
                            }

                            for (int i = 0; i < argumentCount; i++)
                            {
                                object? argument = arguments[i];

                                if (argumentSignatures[i] is ArgumentSignature argumentSignature)
                                {
                                    if (argument is null)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        switch (argumentSignature.Type)
                                        {
                                            case ArgumentType.Concept:
                                                if (argument is not IConcept concept || !concept.Characteristics.HasFlag(argumentSignature.Constraint))
                                                {
                                                    return false;
                                                }
                                                break;

                                            case ArgumentType.Number:
                                                if (argument is not int)
                                                {
                                                    return false;
                                                }
                                                break;

                                            case ArgumentType.Quotation:
                                                if (argument is not string)
                                                {
                                                    return false;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else if (argument is not null)
                                {
                                    return false;
                                }
                            };

                            return true;
                        });
                }
            }
            catch
            {
                throw await _messageFactory.CreateExceptionAsync("MultipleMatchesCommand");
            }

            return result ?? throw await _messageFactory.CreateExceptionAsync("NoMatchesCommand");
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
