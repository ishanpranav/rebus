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
                return (await context.Players.SingleOrDefaultAsync(x => x.UserId == userId)) ?? (await CreatePlayerAsync(context, name, userId));
            }
        }

        private static async Task<Player> CreatePlayerAsync(UniverseContext context, string name, string userId)
        {
            StringExpressionWriter nameWriter = new StringExpressionWriter();

            nameWriter.Write(new string(name
                .Where(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x))
                .ToArray()));

            Player result = new Player()
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
                                Value = nameWriter.ToString()
                            }
                        }
                    }
                }
            };

            await context.AddAsync(result);
            await context.SaveChangesAsync();

            return result;
        }
    }
}
