// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rebus.Server.Discord
{
    internal static class Program
    {
        private static async Task Main()
        {
            ServiceCollection services = new ServiceCollection();

            await using (ServiceProvider serviceProvider = services
                .AddRebus()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                {
                    LogLevel = LogSeverity.Verbose,
                    GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.DirectMessages
                }))
                .AddSingleton(x => x
                    .GetRequiredService<IConfiguration>()
                    .GetRequiredSection(nameof(DiscordOptions))
                    .Get<DiscordOptions>())
                .AddSingleton<Startup>()
                .BuildServiceProvider())
            {
                await serviceProvider
                    .GetRequiredService<Startup>()
                    .StartAsync();
            }
        }
    }
}
