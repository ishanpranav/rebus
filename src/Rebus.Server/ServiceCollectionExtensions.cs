// Ishan Pranav's REBUS: ServiceCollectionExtensions.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Rebus.EditDistances;
using Rebus.Generators;
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
                .AddSingleton<DbRepository>()
                .AddSingleton<IPathfinderFactory<HexPoint>>(x => x.GetRequiredService<DbRepository>())
                .AddSingleton<IPlanetNamer>(x => x.GetRequiredService<DbRepository>())
                .AddSingleton<ISpacecraftNamer>(x => x.GetRequiredService<DbRepository>())
                .AddSingleton<IStringLocalizer>(x => x.GetRequiredService<DbRepository>())
                .AddSingleton<ITokenFactory>(x => x.GetRequiredService<DbRepository>())
                .AddSingleton<IConfiguration>(configurationRoot)
                .AddSingleton<Parser>()
                .AddSingleton<DbRepository>()
                .AddSingleton<IRepository>(x => x.GetRequiredService<DbRepository>())
                .AddSingleton<Controller>()
                .AddSingleton<IEditDistance, DamerauLevenshteinEditDistance>()
                .AddSingleton<Generator, PlanetGenerator>()
                .AddSingleton<Command, BankCommand>()
                .AddSingleton<Command, NavigationCommand>()
                .AddSingleton<Command, RedoCommand>()
                .AddSingleton<Command, ReexecuteCommand>()
                .AddSingleton<Command, RenameCommand>()
                .AddSingleton<Command, UndoCommand>()
                .AddSingleton<Command, VisionCommand>();
        }
    }
}
