// Ishan Pranav's REBUS: SetCredentialEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Rebus.Server.Concepts;

namespace Rebus.Server.EngineStates
{
    internal sealed class SetCredentialEngineState : IEngineState
    {
        private readonly Player _player;

        public SetCredentialEngineState(Player player)
        {
            _player = player;
        }

        public async Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            _player.Credential = value;

            await using (RebusDbContext dbContext = await context.Engine.DbContextFactory.CreateDbContextAsync())
            {
                Token temporaryToken = new Token()
                {
                    Value = Guid
                        .NewGuid()
                        .ToString()
                };

                await dbContext.Tokens.AddAsync(temporaryToken);

                Spacecraft spacecraft = new Spacecraft()
                {
                    Player = _player,
                    Substantive = temporaryToken
                };

                spacecraft.Adjectives.Add(new Adjective()
                {
                    TokenValue = "Spacecraft"
                });

                await dbContext.Concepts.AddAsync(spacecraft);

                await dbContext.SaveChangesAsync();

                spacecraft.Substantive = new Token()
                {
                    Type = TokenTypes.Substantive,
                    Value = $"N{spacecraft.Id}"
                };

                dbContext.Tokens.Remove(temporaryToken);

                await dbContext.SaveChangesAsync();

                (await dbContext.CreateMessageAsync(resource: 15)).Write(writer);
            }

            context.State = new InterpretationEngineState(_player);
        }
    }
}
