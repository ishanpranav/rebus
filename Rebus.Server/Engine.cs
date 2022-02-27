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
using Rebus.ExpressionWriters;

namespace Rebus.Server
{
    internal sealed class Engine : IEngine
    {
        private readonly ILogger<Engine> _logger;
        private readonly Tokenizer _tokenizer;
        private readonly Parser _parser;
        private readonly CommandBuilder _commandBuilder;
        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly XmlSerializer _xmlSerializer;
        private readonly Dictionary<int, Executor> _executorsByPlayer = new Dictionary<int, Executor>();

        public Engine(ILogger<Engine> logger, Tokenizer tokenizer, Parser parser, CommandBuilder commandBuilder, XmlSerializer xmlSerializer, XmlWriterSettings xmlWriterSettings)
        {
            _logger = logger;
            _tokenizer = tokenizer;
            _parser = parser;
            _commandBuilder = commandBuilder;
            _xmlSerializer = xmlSerializer;
            _xmlWriterSettings = xmlWriterSettings;
        }

        public async Task<bool> InterpretAsync(int player, string value, ExpressionWriter writer)
        {
            if (!_executorsByPlayer.TryGetValue(player, out Executor? executor))
            {
                executor = new Executor();

                _executorsByPlayer[player] = executor;
            }

            try
            {
                IAsyncEnumerable<Token> tokens = _tokenizer.TokenizeAsync(value);
                StringExpressionWriter tokenWriter = new StringExpressionWriter();

                await foreach (Token token in tokens)
                {
                    using (tokenWriter.BeginScope(ScopeTypes.Parenthetical))
                    {
                        tokenWriter.Write(token.Type
                            .ToString()
                            .ToUpper());
                        tokenWriter.Write(' ');
                        tokenWriter.Write(token.Value.ToLower());
                    }

                    tokenWriter.Write(' ');
                }

                _logger.LogInformation("{Tokens}", tokenWriter);

                Expression expression = await _parser.ParseAsync(tokens);

                expression.WriteLine(writer);

                _commandBuilder.SetPlayer(player);

                _logger.LogInformation("{Sentence}", expression);

                await using (StringWriter stringWriter = new StringWriter())
                {
                    await using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, _xmlWriterSettings))
                    {
                        _xmlSerializer.Serialize(xmlWriter, expression);
                    }

                    _logger.LogInformation("{Expression}", stringWriter);
                }

                await expression.InterpretAsync(_commandBuilder);

                foreach (Command command in _commandBuilder.Build())
                {
                    await foreach (IWritable result in executor.ExecuteAsync(command))
                    {
                        _logger.LogInformation("{Result}", result);

                        result.Write(writer);

                        writer.WriteLine();
                    }

                    writer.WriteLine();
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

            if (executor.Terminated)
            {
                return !_executorsByPlayer.Remove(player);
            }
            else
            {
                return true;
            }
        }
    }
}
