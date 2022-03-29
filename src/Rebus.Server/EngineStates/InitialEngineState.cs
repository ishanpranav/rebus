// Ishan Pranav's REBUS: InitialEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Rebus.Server.EngineStates.Credential;

namespace Rebus.Server.EngineStates
{
    internal sealed class InitialEngineState : IEngineState
    {
        public async Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            Version? version = Assembly
                .GetEntryAssembly()?
                .GetName().Version ?? new Version();

            writer.Write(context.Engine.Localizer["Header", version.Major, version.Minor, version.Build, RuntimeInformation.FrameworkDescription]);

            Player player = await context.Engine.Repository.CreatePlayerAsync(value);

            if (string.IsNullOrWhiteSpace(player.Credential))
            {
                writer.Write(context.Engine.Localizer["FirstWelcome", player.UserId]);

                context.State = new SetCredentialEngineState(player);
            }
            else
            {
                writer.Write(context.Engine.Localizer["SecondWelcome", player.UserId]);

                context.State = new GetCredentialEngineState(player);
            }
        }
    }
}
