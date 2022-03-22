// Ishan Pranav's REBUS: Repository.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Server.Concepts;
using Rebus.Tokens;

namespace Rebus.Server
{
    internal sealed class Repository : IRepository
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;
        private readonly IEditDistance _editDistance;

        public Repository(IDbContextFactory<RebusDbContext> contextFactory, IEditDistance editDistance)
        {
            _contextFactory = contextFactory;
            _editDistance = editDistance;
        }

        public async Task<Player> CreatePlayerAsync(string userId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                Player? result = await context.Players.SingleOrDefaultAsync(x => x.UserId == userId);

                if (result is null)
                {
                    result = new Player()
                    {
                        UserId = userId
                    };

                    await context.AddAsync(result);
                    await context.SaveChangesAsync();
                }

                if (!await context.Spacecraft.AnyAsync(x => x.PlayerId == result.Id))
                {
                    Spacecraft spacecraft = new Spacecraft()
                    {
                        Player = result,
                        Article = await CreateTokenAsync(context, TokenTypes.Article, "the"),
                        Substantive = await CreateTokenAsync(context, TokenTypes.Substantive, "spacecraft")
                    };

                    spacecraft.Adjectives.Add(new Adjective()
                    {
                        Token = await CreateTokenAsync(context, TokenTypes.Adjective, $"N{result.Sequence}")
                    });

                    result.Sequence++;

                    await context.Spacecraft.AddAsync(spacecraft);
                    await context.SaveChangesAsync();
                }

                return result;
            }
        }

        public IEnumerable<Fleet> GetFleets(int playerId)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Spacecraft
                    .Where(x => x.PlayerId == playerId)
                    .OrderBy(x => x.Substantive.Value)
                    .AsWritable()
                    .AsEnumerable()
                    .GroupBy(x => x.Region)
                    .Select(x => new Fleet(playerId, x))
                    .ToArray();
            }
        }

        public async Task RenameAsync(Concept concept, string name)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                string[] words = new string(name
                    .Where(x => char.IsLetterOrDigit(x) || x == ' ')
                    .ToArray())
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                context.Update(concept);

                context.RemoveRange(concept.Adjectives);

                await context.SaveChangesAsync();

                for (int i = 0; i < words.Length; i++)
                {
                    Token? token = await context.Tokens.FindAsync(words[i]);

                    if (token is null)
                    {
                        if (i == words.Length - 1)
                        {
                            token = new Token()
                            {
                                Type = TokenTypes.Substantive,
                                Value = words[i]
                            };

                            concept.Substantive = token;
                        }
                        else
                        {
                            token = new Token()
                            {
                                Type = TokenTypes.Adjective,
                                Value = words[i]
                            };

                            concept.Adjectives.Add(new Adjective()
                            {
                                Token = token
                            });
                        }

                        await context.Tokens.AddAsync(token);
                    }
                    else
                    {
                        if (token.Type.HasFlag(TokenTypes.Adjective))
                        {
                            concept.Adjectives.Add(new Adjective()
                            {
                                Token = token
                            });
                        }
                        else if (token.Type.HasFlag(TokenTypes.Substantive))
                        {
                            concept.Substantive = token;
                        }
                        else if (token.Type.HasFlag(TokenTypes.Article))
                        {
                            concept.Article = token;
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<int> GetWealthAsync(int playerId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                Player? player = await context.Players.FindAsync(playerId);

                if (player is null)
                {
                    return 0;
                }
                else
                {
                    return player.Wealth;
                }
            }
        }

        public async Task SetWealthAsync(int playerId, int value)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                Player? player = await context.Players.FindAsync(playerId);

                if (player is not null)
                {
                    player.Wealth = value;

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task AddNavigationAsync(int playerId, HexPoint region)
        {
            RebusDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.Navigations.AddAsync(new Navigation()
                {
                    PlayerId = playerId,
                    Q = region.Q,
                    R = region.R
                });

                await context.SaveChangesAsync();
            }
            catch (DbUpdateException) { }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<Player?> GetPlayerAsync(int id)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.Players.FindAsync(id);
            }
        }

        public IEnumerable<Token>? GetSuggestions(TokenTypes? expectedType, string actualValue)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                IQueryable<Token>? results = null;

                results = context.Tokens;

                if (expectedType is not null)
                {
                    results = results.Where(x => x.Type.HasFlag(expectedType.Value));
                }

                return results
                    .AsEnumerable()
                    .GroupBy(x => _editDistance.Calculate(x.Value, actualValue))
                    .Where(x => x.Key < 3)
                    .MinBy(x => x.Key);
            }
        }

        private static async Task<Token> CreateTokenAsync(RebusDbContext context, TokenTypes type, string value)
        {
            Token? token = await context.Tokens.FindAsync(value);

            if (token is null)
            {
                token = new Token()
                {
                    Type = type,
                    Value = value
                };

                await context.Tokens.AddAsync(token);
                await context.SaveChangesAsync();
            }
            else if (!token.Type.HasFlag(type))
            {
                token.Type |= type;

                await context.SaveChangesAsync();
            }

            return token;
        }

        public IEnumerable<Concept> GetConcepts(Argument argument, int playerId, IReadOnlyCollection<IToken> adjectives, IToken substantive)
        {
            if (substantive is HexPointToken hexPointToken)
            {
                yield return new Location(hexPointToken);
            }
            else
            {
                using (RebusDbContext context = _contextFactory.CreateDbContext())
                {
                    IQueryable<Concept> concepts = context.Concepts
                        .AsWritable()
                        .Where(x => x.Substantive.Value == substantive.Value);

                    if (argument is Argument.Subject)
                    {
                        concepts = concepts
                            .OfType<Spacecraft>()
                            .Where(x => x.PlayerId == playerId);
                    }

                    foreach (Concept concept in concepts)
                    {
                        if (adjectives.All(adjective => concept.Adjectives.Any(x => adjective.Value == x.TokenValue)))
                        {
                            yield return concept;
                        }
                    }
                }
            }
        }

        public Command? GetCommand(string verb, string? adverb, IDictionary<Guid, Command> commandsByGuid, ArgumentSet arguments)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                return context.CommandSignatures
                    .Where(signature => signature.VerbValue == verb && signature.AdverbValue == adverb)
                    .Select(x => x.Guid)
                    .AsEnumerable()
                    .Select(x => commandsByGuid[x])
                    .SingleOrDefault(x => x.Matches(arguments));
            }
        }

        public async Task<IFeature?> GetStarAsync(HexPoint region)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.Stars.SingleOrDefaultAsync(x => x.Q == region.Q && x.R == region.R);
            }
        }

        public async Task<object?> GetPlanetAsync(HexPoint region)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.Planets.SingleOrDefaultAsync(x => x.Q == region.Q && x.R == region.R);
            }
        }
    }
}
