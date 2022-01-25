// Ishan Pranav's REBUS: EngineFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Rebus.Server
{
    internal sealed class EngineFactory : IEngineFactory
    {
        private readonly ILogger<Engine> _logger;
        private readonly IDbContextFactory<UniverseContext> _universeContextFactory;
        private readonly IDbContextFactory<ResourceContext> _resourceContextFactory;
        private readonly CommandBuilder _commandBuilder;
        private readonly FileRepository _fileRepository;
        private readonly Parser _parser;
        private readonly MessageBuilder _messageBuilder;
        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public EngineFactory(ILogger<Engine> logger, IDbContextFactory<UniverseContext> universeContextFactory, IDbContextFactory<ResourceContext> resourceContextFactory, CommandBuilder commandBuilder, FileRepository fileRepository, Parser parser, MessageBuilder messageBuilder, XmlWriterSettings xmlWriterSettings, JsonSerializerOptions jsonSerializerOptions)
        {
            _logger = logger;
            _universeContextFactory = universeContextFactory;
            _resourceContextFactory = resourceContextFactory;
            _commandBuilder = commandBuilder;
            _fileRepository = fileRepository;
            _parser = parser;
            _messageBuilder = messageBuilder;
            _xmlWriterSettings = xmlWriterSettings;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<IEngine> CreateEngineAsync()
        {
            Seed? seed;

            await using (FileStream fileStream = File.OpenRead(_fileRepository.GetPath(typeof(Seed), "json")))
            {
                seed = await JsonSerializer.DeserializeAsync<Seed>(fileStream, _jsonSerializerOptions);
            }

            if (seed is not null)
            {
                await using (UniverseContext universeContext = await _universeContextFactory.CreateDbContextAsync())
                {
                    if (await universeContext.Database.EnsureCreatedAsync())
                    {
                        await universeContext.Concepts.AddRangeAsync(seed.Concepts);
                        await universeContext.SaveChangesAsync();
                        await universeContext.Tokens.AddRangeAsync(seed.Tokens);
                        await universeContext.SaveChangesAsync();

                        await using (ResourceContext resourceContext = await _resourceContextFactory.CreateDbContextAsync())
                        {
                            foreach (string verb in resourceContext.CommandSignatures.Select(x => x.Verb))
                            {
                                await universeContext.UpdateTokenAsync(TokenTypes.Verb, verb);
                            }

                            foreach (string adverb in resourceContext.CommandSignatures
                                .Select(x => x.Adverb)
                                .Where(x => x != null)
                                .OfType<string>())
                            {
                                await universeContext.UpdateTokenAsync(TokenTypes.Adverb, adverb);
                            }
                        }
                    }
                }
            }

            return new Engine(_logger, _commandBuilder, _parser, _messageBuilder, _xmlWriterSettings);
        }
    }
}
