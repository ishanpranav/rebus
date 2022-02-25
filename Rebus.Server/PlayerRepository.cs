// Ishan Pranav's REBUS: PlayerRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.ExpressionWriters;

namespace Rebus.Server
{
    internal sealed class PlayerRepository : IPlayerRepository
    {
        private readonly IDbContextFactory<UniverseContext> _contextFactory;

        public PlayerRepository(IDbContextFactory<UniverseContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IPlayer> GetPlayerAsync(string name, string userId)
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                Player? result = await context.Players.SingleOrDefaultAsync(x => x.UserId == userId);

                if (result is null)
                {
                    StringExpressionWriter writer = new StringExpressionWriter();

                    foreach (char @char in name)
                    {
                        if (char.IsLetterOrDigit(@char) || char.IsWhiteSpace(@char))
                        {
                            writer.Write(@char);
                        }
                    }

                    string[] words = writer
                        .ToString()
                        .Split();
                    int lastIndex = words.Length - 1;
                    Token[] adjectives = new Token[lastIndex];

                    for (int i = 0; i < lastIndex; i++)
                    {
                        adjectives[i] = new Token()
                        {
                            Type = TokenTypes.Adjective,
                            Value = words[i]
                        };
                    }

                    result = new Player()
                    {
                        UserId = userId,
                        Concept = new Concept()
                        {
                            ContainerId = 1,
                            Characteristics = Characteristics.Agent,
                            Signatures = new ConceptSignature[]
                            {
                                new ConceptSignature()
                                {
                                    Substantive = new Token()
                                    {
                                        Type = TokenTypes.Substantive,
                                        Value = words[lastIndex]
                                    },
                                    Adjectives = adjectives
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
    }
}
