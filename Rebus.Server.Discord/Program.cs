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
                .Build();
            ServiceCollection services = new ServiceCollection();

            services
                .AddOptions<StartupOptions>()
                .Bind(configurationRoot.GetSection(nameof(StartupOptions)));

            await using (ServiceProvider serviceProvider = services
                .AddRebus()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000
                }))
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