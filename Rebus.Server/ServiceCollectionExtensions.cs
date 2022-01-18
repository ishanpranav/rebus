// Ishan Pranav's REBUS: ServiceCollectionExtensions.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.Commands.System;
using Rebus.Server.Commands;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Rebus.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRebus(this IServiceCollection source)
        {
            PathProvider pathProvider = new PathProvider(Path.GetFullPath(@"..\..\..\..\"));

            return source
                .AddLocalization()
                .AddLogging(x => x.AddConsole())
                .AddDbContextFactory<UniverseContext>(x => x
                    .UseSqlite(new SqliteConnectionStringBuilder()
                    {
                        DataSource = pathProvider.GetPath(typeof(UniverseContext), "db")
                    }.ConnectionString)
                    .UseAllCheckConstraints())
                .AddSingleton(typeof(MessageBuilder<>))
                .AddSingleton(pathProvider)
                .AddSingleton(new Random(0))
                .AddSingleton(new XmlWriterSettings()
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
                        WriteIndented = true
                    };

                    result.Converters.Add(new JsonStringEnumConverter(jsonNamingPolicy));

                    return result;
                })
                .AddSingleton(new Regex(@"(\w+)|\""([\w\s]*)""", RegexOptions.Compiled))
                .AddSingleton<JsonRepository>()
                .AddSingleton<DbRepository>()
                .AddSingleton<CommandRepository>()
                .AddSingleton<IAsyncSupportInitialize, Initializer>()
                .AddSingleton<ITokenizer, Tokenizer>()
                .AddSingleton<Parser>()
                .AddSingleton<IEngine, Engine>()
                .AddSingleton<Command, VisionCommand>()
                .AddSingleton<Command, RedoSystemCommand>()
                .AddSingleton<Command, ReexecuteSystemCommand>()
                .AddSingleton<Command, UndoSystemCommand>();
        }
    }
}
