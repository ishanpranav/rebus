// Ishan Pranav's REBUS: EngineFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Rebus.Server
{
    internal sealed class EngineFactory : IEngineFactory
    {
        private readonly ILogger<Engine> _logger;
        private readonly IDbContextFactory<UniverseContext> _contextFactory;
        private readonly Tokenizer _tokenizer;
        private readonly Parser _parser;
        private readonly CommandBuilder _commandBuilder;
        private readonly IFileProvider _fileProvider;
        private readonly XmlSerializer _xmlSerializer;
        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public EngineFactory(ILogger<Engine> logger, IDbContextFactory<UniverseContext> contextFactory, Tokenizer tokenizer, Parser parser, CommandBuilder commandBuilder, IFileProvider fileProvider, XmlSerializer xmlSerializer, XmlWriterSettings xmlWriterSettings, JsonSerializerOptions jsonSerializerOptions)
        {
            _logger = logger;
            _contextFactory = contextFactory;
            _tokenizer = tokenizer;
            _parser = parser;
            _commandBuilder = commandBuilder;
            _fileProvider = fileProvider;
            _xmlSerializer = xmlSerializer;
            _xmlWriterSettings = xmlWriterSettings;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<IEngine> CreateEngineAsync()
        {
            await using (UniverseContext context = await _contextFactory.CreateDbContextAsync())
            {
                if (await context.Database.EnsureCreatedAsync())
                {
                    IFileInfo fileInfo = _fileProvider.GetFileInfo("Rebus.json");

                    try
                    {
                        await using (Stream stream = fileInfo.CreateReadStream())
                        {
                            Seed? seed = await JsonSerializer.DeserializeAsync<Seed>(stream, _jsonSerializerOptions);

                            if (seed is not null)
                            {
                                await context.Formats.AddRangeAsync(seed.Formats);
                                await context.Concepts.AddRangeAsync(seed.Concepts);
                                await context.SaveChangesAsync();

                                await context
                                    .Set<CommandPrototype>()
                                    .AddRangeAsync(seed.Commands);

                                await context.SaveChangesAsync();

                                await context.Tokens.AddRangeAsync(seed.Tokens);
                                await context.SaveChangesAsync();
                            }
                        }
                    }
                    catch
                    {
                        await context.Database.EnsureDeletedAsync();

                        throw;
                    }
                }
            }

            return new Engine(_logger, _tokenizer, _parser, _commandBuilder, _xmlSerializer, _xmlWriterSettings);
        }
    }
}
