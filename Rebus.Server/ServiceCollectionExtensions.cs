// Ishan Pranav's REBUS: ServiceCollectionExtensions.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Rebus.EditDistances;
using Rebus.Server.Commands;

namespace Rebus.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRebus(this IServiceCollection source)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddUserSecrets(typeof(ServiceCollectionExtensions).Assembly)
                .Build();

            return source
                .AddLogging(x => x
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information))
                .AddLocalization()
                .AddDbContextFactory<RebusDbContext, RebusDbContextFactory>()
                .AddSingleton(typeof(IEngine<>), typeof(Engine<>))
                .AddSingleton<IFormatProvider, ExpressionFormatProvider>()
                .AddSingleton<IStringLocalizer, DbStringLocalizer>()
                .AddSingleton<IConfiguration>(configurationRoot)
                .AddSingleton<Parser>()
                .AddSingleton<Repository>()
                .AddSingleton<IRepository>(x => x.GetRequiredService<Repository>())
                .AddSingleton<Controller>()
                .AddSingleton<ITokenFactory, TokenFactory>()
                .AddSingleton<IEditDistance, DamerauLevenshteinEditDistance>()
                .AddSingleton<IPathfinderFactory<HexPoint>, DbPathfinderFactory>()
                .AddSingleton<Command, NavigationCommand>()
                .AddSingleton<Command, RedoCommand>()
                .AddSingleton<Command, ReexecuteCommand>()
                .AddSingleton<Command, RenameCommand>()
                .AddSingleton<Command, UndoCommand>()
                .AddSingleton<Command, VisionCommand>();
        }
    }
}
