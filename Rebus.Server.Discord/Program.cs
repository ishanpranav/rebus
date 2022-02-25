// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

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
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddUserSecrets(typeof(Program).Assembly)
                .AddJsonFile("AppSettings.json")
                .Build();
            ServiceCollection services = new ServiceCollection();

            await using (ServiceProvider serviceProvider = services
                .AddRebus(configurationRoot)
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                {
                    LogLevel = LogSeverity.Verbose,
                    GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.DirectMessages
                }))
                .AddSingleton(configurationRoot
                    .GetRequiredSection(nameof(StartupOptions))
                    .Get<StartupOptions>())
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
