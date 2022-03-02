// Ishan Pranav's REBUS: ServiceCollectionExtensions.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.EditDistances;
using Rebus.Server.Commands;

namespace Rebus.Server
{
    public static class ServiceCollectionExtensions
    {
        internal const string AppSettingsJsonFile = "AppSettings.json";

        public static IServiceCollection AddRebus(this IServiceCollection source)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddUserSecrets(typeof(ServiceCollectionExtensions).Assembly)
                .AddJsonFile(AppSettingsJsonFile)
                .Build();

            return source
                .AddLogging(x => x
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information))
                .AddDbContextFactory<RebusDbContext, RebusDbContextFactory>()
                .AddSingleton<IConfiguration>(configurationRoot)
                .AddSingleton(Random.Shared)
                .AddSingleton<Parser>()
                .AddSingleton<IEditDistance, DamerauLevenshteinEditDistance>()
                .AddSingleton<IEngine, Engine>()
                .AddSingleton<Command, VisionCommand>()
                .AddSingleton<Command, TransitiveVisionCommand>()
                .AddSingleton<Command, RedoCommand>()
                .AddSingleton<Command, ReexecuteCommand>()
                .AddSingleton<Command, UndoCommand>();
            //.AddSingleton<Command, TerminateCommand>();
        }
    }
}
