// Ishan Pranav's REBUS: ServiceCollectionExtensions.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.Commands.System;
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
                .AddSingleton(x => new XmlWriterSettings()
                {
                    Async = true,
                    Indent = true,
                    IndentChars = "    "
                })
                .AddSingleton<IConfiguration>(configurationRoot)
                .AddSingleton(x => new XmlSerializer(typeof(Expression)))
                .AddSingleton(Random.Shared)
                .AddSingleton<IPlayerRepository, PlayerRepository>()
                .AddSingleton<Tokenizer>()
                .AddSingleton<Parser>()
                .AddSingleton<CommandBuilder>()
                .AddSingleton<MessageFactory, ResourceMessageFactory>()
                .AddSingleton<IEngine, Engine>()
                .AddSingleton<Command, VisionCommand>()
                .AddSingleton<Command, TransitiveVisionCommand>()
                .AddSingleton<Command, RedoSystemCommand>()
                .AddSingleton<Command, ReexecuteSystemCommand>()
                .AddSingleton<Command, UndoSystemCommand>()
                .AddSingleton<Command, TerminateSystemCommand>();
        }
    }
}
