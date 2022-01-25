// Ishan Pranav's REBUS: Engine.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace Rebus.Server
{
    internal sealed class Engine : IEngine
    {
        private readonly ILogger<Engine> _logger;
        private readonly CommandBuilder _commandBuilder;
        private readonly Parser _parser;
        private readonly MessageBuilder _messageBuilder;
        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly XmlSerializer _xmlExpressionSerializer = new XmlSerializer(typeof(Expression));
        private readonly Dictionary<IPlayer, Executor> _executorsByPlayer = new Dictionary<IPlayer, Executor>();

        public Engine(ILogger<Engine> logger, CommandBuilder commandBuilder, Parser parser, MessageBuilder messageBuilder, XmlWriterSettings xmlWriterSettings)
        {
            _logger = logger;
            _commandBuilder = commandBuilder;
            _parser = parser;
            _messageBuilder = messageBuilder;
            _xmlWriterSettings = xmlWriterSettings;
        }

        public async Task InterpretAsync(IPlayer player, string value, ExpressionWriter writer)
        {
            try
            {
                Expression expression = await _parser.ParseAsync(value);

                expression.WriteLine(writer);

                _commandBuilder.SetPlayer(player);

                _logger.LogInformation("{Sentence}", expression);

                await using (StringWriter stringWriter = new StringWriter())
                await using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, _xmlWriterSettings))
                {
                    _xmlExpressionSerializer.Serialize(xmlWriter, expression);

                    _logger.LogInformation("{Expression}", stringWriter);
                }

                await expression.InterpretAsync(_commandBuilder);

                if (!_executorsByPlayer.TryGetValue(player, out Executor? executor))
                {
                    executor = new Executor();

                    _executorsByPlayer[player] = executor;
                }

                foreach (Command command in _commandBuilder.Build())
                {
                    IWritable? result = await executor.ExecuteAsync(command) ?? await _messageBuilder.BuildAsync(ResourceIndex.Failure);

                    if (result is not null)
                    {
                        _logger.LogInformation("{Result}", result);

                        result.Write(writer);

                        writer.WriteLine();
                    }
                }
            }
            catch (RebusException rebusException)
            {
                rebusException.Write(writer);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception");
            }

            writer.WriteLine();
        }
    }
}
