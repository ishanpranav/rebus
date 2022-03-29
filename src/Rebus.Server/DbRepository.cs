// Ishan Pranav's REBUS: DbRepository.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rebus.Server.Concepts;
using Rebus.Tokens;

namespace Rebus.Server
{
    internal sealed class DbRepository : IPathfinderFactory<HexPoint>, IPlanetNamer, IRepository, ISpacecraftNamer, IStringLocalizer, ITokenFactory
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public LocalizedString this[string name]
        {
            get
            {
                string? resource = GetResource(name, arguments: 0);

                if (resource is null)
                {
                    return new LocalizedString(name, name, resourceNotFound: true);
                }
                else
                {
                    return new LocalizedString(name, resource);
                }
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                string? resource = GetResource(name, arguments.Length);

                if (resource is null)
                {
                    return new LocalizedString(name, name, resourceNotFound: true);
                }
                else
                {
                    return new LocalizedString(name, string.Format(new ExpressionFormatProvider(), resource, arguments));
                }
            }
        }

        public DbRepository(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public string NameSpacecraft()
        {
            return Name(NameTypes.Spacecraft);
        }

        public string NamePlanet(HexPoint region)
        {
            return Name(NameTypes.Planet);
        }

        private string Name(NameTypes type)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Tokens
                    .Where(x => x.NameType.HasFlag(type))
                    .Select(x => x.Value)
                    .OrderBy(x => EF.Functions.Random())
                    .First();
            }
        }

        public IToken CreateToken(string value)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Tokens.Find(value) ?? new Token()
                {
                    Value = value
                };
            }
        }

        public IPathfinder<HexPoint> CreatePathfinder(int playerId)
        {
            return new DbPathfinder(playerId, _contextFactory);
        }

        private string? GetResource(string name, int arguments)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Resources
                    .Where(x => x.Key == name && x.Arguments == arguments)
                    .Select(x => x.Value)
                    .OrderBy(x => EF.Functions.Random())
                    .FirstOrDefault();
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Resources
                    .AsEnumerable()
                    .Select(x => new LocalizedString(x.Key, x.Value))
                    .ToArray();
            }
        }

        public async Task SetCredentialAsync(Player player, string value)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                player.Credential = value;

                context.Update(player);

                await context.SaveChangesAsync();
            }
        }

        public async Task<Player> CreatePlayerAsync(string userId)
        {
            Player? result;
            Spacecraft? spacecraft = null;

            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                result = await context.Players.SingleOrDefaultAsync(x => x.UserId == userId);

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
                    spacecraft = new Spacecraft()
                    {
                        Player = result
                    };

                    await context.Spacecraft.AddAsync(spacecraft);
                    await context.SaveChangesAsync();
                }
            }

            if (spacecraft is not null)
            {
                await RenameAsync(spacecraft, NameSpacecraft());
            }

            return result;
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

        public async Task AddNavigationAsync(int playerId, HexPoint region, Generator generator)
        {
            Dictionary<Concept, string> concepts = new Dictionary<Concept, string>();

            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                IQueryable<Navigation> navigations = context.Navigations.Where(x => x.Q == region.Q && x.R == region.R);

                if (await navigations.AnyAsync())
                {
                    if (!await navigations.AnyAsync(x => x.PlayerId == playerId))
                    {
                        await addAsync();
                    }
                }
                else
                {
                    Map map = new Map();

                    foreach (HexPoint point in generator.CreateRange(region))
                    {
                        if (await context.Planets.AnyAsync(x => x.Q == point.Q && x.R == point.R))
                        {
                            map.Add(point, RegionType.Planetary);
                        }
                        else if (await context.Stars.AnyAsync(x => x.Q == point.Q && x.R == point.R))
                        {
                            map.Add(point, RegionType.Stellar);
                        }
                        else if (await context.Navigations.AnyAsync(x => x.Q == point.Q && x.R == point.R))
                        {
                            map.Add(point, RegionType.Empty);
                        }
                        else
                        {
                            map.Add(point, RegionType.None);
                        }
                    }

                    generator.Generate(region, map);

                    foreach (HexPoint discovery in map.Discoveries)
                    {
                        switch (map.GetRegionType(discovery))
                        {
                            case RegionType.Planetary:
                                Planet planet = new Planet()
                                {
                                    Q = discovery.Q,
                                    R = discovery.R
                                };

                                concepts.Add(planet, map.GetName(discovery));

                                await context.Planets.AddAsync(planet);
                                await context.SaveChangesAsync();
                                break;

                            case RegionType.Stellar:
                                Star star = new Star()
                                {
                                    Q = discovery.Q,
                                    R = discovery.R
                                };

                                concepts.Add(star, map.GetName(discovery));

                                await context.Stars.AddAsync(star);
                                await context.SaveChangesAsync();
                                break;
                        }
                    }

                    await addAsync();
                }

                async Task addAsync()
                {
                    await context.Navigations.AddAsync(new Navigation()
                    {
                        PlayerId = playerId,
                        Q = region.Q,
                        R = region.R
                    });
                    await context.SaveChangesAsync();
                }
            }

            foreach (KeyValuePair<Concept, string> concept in concepts)
            {
                await RenameAsync(concept.Key, concept.Value);
            }
        }

        public IEnumerable<Token>? GetSuggestions(IEditDistance editDistance, TokenTypes? expectedType, string actualValue)
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
                    .GroupBy(x => editDistance.Compute(x.Value, actualValue))
                    .Where(x => x.Key < 3)
                    .MinBy(x => x.Key);
            }
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

                    if (argument == Argument.Subject)
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
                return await context.Stars
                    .AsWritable()
                    .SingleOrDefaultAsync(x => x.Q == region.Q && x.R == region.R);
            }
        }

        public async Task<object?> GetPlanetAsync(HexPoint region)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.Planets
                    .AsWritable()
                    .SingleOrDefaultAsync(x => x.Q == region.Q && x.R == region.R);
            }
        }
    }
}
