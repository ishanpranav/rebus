// Ishan Pranav's REBUS: InitialEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.EngineStates
{
    internal sealed class InitialEngineState : IEngineState
    {
        public async Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            await using (RebusDbContext dbContext = await context.Engine.DbContextFactory.CreateDbContextAsync())
            {
                (await dbContext.CreateMessageAsync(resource: 6)).Write(writer);
                (await dbContext.CreateMessageAsync(resource: 7)).Write(writer);

                Version? version = Assembly
                    .GetEntryAssembly()?
                    .GetName().Version ?? new Version();

                (await dbContext.CreateMessageAsync(resource: 11, version.Major, version.Minor, version.Build, RuntimeInformation.FrameworkDescription)).Write(writer);
                (await dbContext.CreateMessageAsync(resource: 12)).Write(writer);

                Player? player = await dbContext.Players.SingleOrDefaultAsync(x => x.UserId == value);

                if (player is null)
                {
                    player = new Player()
                    {
                        UserId = value
                    };

                    (await dbContext.CreateMessageAsync(resource: 13, value)).Write(writer);

                    context.State = new SetCredentialEngineState(player);
                }
                else
                {
                    (await dbContext.CreateMessageAsync(resource: 14, value)).Write(writer);

                    context.State = new CredentialEngineState(player);
                }
            }
        }
    }
}
