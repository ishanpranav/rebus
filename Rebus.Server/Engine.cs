// Ishan Pranav's REBUS: Engine.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Rebus.Server
{
    internal class Engine : IEngine
    {
        private readonly ILogger<Engine> _logger;
        private readonly IAsyncSupportInitialize _initializer;
        private readonly DbRepository _repository;
        private readonly CommandRepository _commandRepository;
        private readonly Parser _parser;
        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly XmlSerializer _xmlExpressionSerializer = new XmlSerializer(typeof(Expression));
        private readonly Dictionary<string, Executor> _executorsByPlayerTag = new Dictionary<string, Executor>();

        public Engine(ILogger<Engine> logger, IAsyncSupportInitialize initializer, DbRepository repository, CommandRepository commandRepository, Parser parser, XmlWriterSettings xmlWriterSettings)
        {
            this._logger = logger;
            this._initializer = initializer;
            this._repository = repository;
            this._commandRepository = commandRepository;
            this._parser = parser;
            this._xmlWriterSettings = xmlWriterSettings;
        }

        public Task InitializeAsync()
        {
            return this._initializer.InitializeAsync();
        }

        public async Task InterpretAsync(string playerTag, string value, ExpressionWriter writer)
        {
            try
            {
                Expression expression = await this._parser.ParseAsync(value);

                expression.WriteLine(writer);

                CommandBuilder commandBuilder = new CommandBuilder(playerTag, this._repository, this._commandRepository);

                await expression.InterpretAsync(commandBuilder);

                await using (StringWriter stringWriter = new StringWriter())
                await using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, this._xmlWriterSettings))
                {
                    this._xmlExpressionSerializer.Serialize(xmlWriter, expression);

                    this._logger.LogInformation("{Expression}", stringWriter);
                }

                if (!this._executorsByPlayerTag.TryGetValue(playerTag, out Executor? executor))
                {
                    executor = new Executor();

                    this._executorsByPlayerTag[playerTag] = executor;
                }

                foreach (Command command in commandBuilder.Build())
                {
                    IWritable result = await executor.ExecuteAsync(command);

                    result.Write(writer);
                }
            }
            catch (RebusException rebusException)
            {
                rebusException.Write(writer);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception, "Exception");
            }
        }
    }
}
