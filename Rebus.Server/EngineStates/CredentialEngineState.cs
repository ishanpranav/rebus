// Ishan Pranav's REBUS: CredentialEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Rebus.Server.EngineStates
{
    internal sealed class CredentialEngineState : IEngineState
    {
        private readonly Player _player;

        public CredentialEngineState(Player player)
        {
            _player = player;
        }

        public async Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            await using (RebusDbContext dbContext = await context.Engine.DbContextFactory.CreateDbContextAsync())
            {
                if (_player.Credential.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    context.State = new InterpretationEngineState(_player);

                    (await dbContext.CreateMessageAsync(resource: 15)).Write(writer);
                }
                else
                {
                    (await dbContext.CreateMessageAsync(resource: 16)).Write(writer);
                }
            }
        }
    }
}
