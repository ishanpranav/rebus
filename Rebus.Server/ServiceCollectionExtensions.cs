// Ishan Pranav's REBUS: ServiceCollectionExtensions.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Rebus.Commands.System;
using Rebus.Server.Commands;

namespace Rebus.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRebus(this IServiceCollection source, IConfiguration configuration)
        {
            return source
                .AddLogging(x => x
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information))
                .AddDbContextFactory<UniverseContext>(x => x
                    .UseSqlite(new SqliteConnectionStringBuilder()
                    {
                        DataSource = configuration.GetConnectionString(nameof(UniverseContext))
                    }.ConnectionString))
                .AddSingleton(x => new XmlWriterSettings()
                {
                    Async = true,
                    Indent = true,
                    IndentChars = "    "
                })
                .AddSingleton(x =>
                {
                    JsonNamingPolicy jsonNamingPolicy = JsonNamingPolicy.CamelCase;
                    JsonSerializerOptions result = new JsonSerializerOptions()
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                        PropertyNamingPolicy = jsonNamingPolicy,
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve,
                        WriteIndented = true
                    };

                    result.Converters.Add(new JsonStringEnumConverter(jsonNamingPolicy));

                    return result;
                })
                .AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.GetFullPath(configuration.GetValue<string>(nameof(PhysicalFileProvider)))))
                .AddSingleton(x => new Regex(@"(\w+)|\""([\w\s]*)""", RegexOptions.Compiled))
                .AddSingleton(x => new XmlSerializer(typeof(Expression)))
                .AddSingleton(Random.Shared)
                .AddSingleton<IEngineFactory, EngineFactory>()
                .AddSingleton<IPlayerRepository, PlayerRepository>()
                .AddSingleton<IConceptRepository, ConceptRepository>()
                .AddSingleton<Tokenizer>()
                .AddSingleton<Parser>()
                .AddSingleton<CommandBuilder>()
                .AddSingleton<MessageFactory, ResourceMessageFactory>()
                .AddSingleton<VisionController>()
                .AddSingleton<Command, VisionCommand>()
                .AddSingleton<Command, TransitiveVisionCommand>()
                .AddSingleton<Command, RedoSystemCommand>()
                .AddSingleton<Command, ReexecuteSystemCommand>()
                .AddSingleton<Command, UndoSystemCommand>()
                .AddSingleton<Command, TerminateSystemCommand>();
        }
    }
}
