// Ishan Pranav's REBUS: Engine.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Rebus.Exceptions;

namespace Rebus.Server
{
    internal sealed class Engine<T> : IEngine, IEngine<T> where T : notnull
    {
        private readonly XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "    "
        };
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Expression));
        private readonly Dictionary<T, EngineContext> _contextsByUser = new Dictionary<T, EngineContext>();
        private readonly ILogger<Engine<T>> _logger;
        private readonly IEnumerable<Command> _commands;
        private readonly ITokenFactory _tokenFactory;

        public Repository Repository { get; }
        public Controller Controller { get; }
        public IStringLocalizer Localizer { get; }

        public Engine(ILogger<Engine<T>> logger, Repository repository, Controller controller, IEnumerable<Command> commands, ITokenFactory tokenFactory, IStringLocalizer localizer)
        {
            _logger = logger;
            Repository = repository;
            Controller = controller;
            _commands = commands;
            _tokenFactory = tokenFactory;
            Localizer = localizer;
        }

        public async Task InterpretAsync(T user, string value, ExpressionWriter writer)
        {
            if (!_contextsByUser.TryGetValue(user, out EngineContext? context))
            {
                context = new EngineContext(engine: this);

                _contextsByUser[user] = context;
            }

            try
            {
                await context.State.InterpretAsync(context, value, writer);
            }
            catch (RebusSpellingException rebusSpellingException)
            {
                if (rebusSpellingException.ActualValue is null)
                {
                    if (rebusSpellingException.ExpectedType is null)
                    {
                        writer.Write(Localizer["UnexpectedParsingError"]);
                    }
                    else
                    {
                        writer.Write(Localizer["UnexpectedParsingError", rebusSpellingException.ExpectedType]);
                    }
                }
                else
                {
                    IEnumerable<Token>? suggestions = Repository.GetSuggestions(rebusSpellingException.ExpectedType, rebusSpellingException.ActualValue);

                    if (rebusSpellingException.ExpectedType is null)
                    {
                        if (suggestions is null)
                        {
                            writer.Write(Localizer["UnexpectedParsingError"]);
                        }
                        else
                        {
                            writer.Write(Localizer["UnexpectedParsingError", suggestions]);
                        }
                    }
                    else
                    {
                        if (suggestions is null)
                        {
                            writer.Write(Localizer["ExpectedParsingError", rebusSpellingException.ExpectedType]);
                        }
                        else
                        {
                            writer.Write(Localizer["ExpectedParsingError", rebusSpellingException.ExpectedType, suggestions]);
                        }
                    }
                }
            }
            catch (RebusException rebusException)
            {
                writer.Write(rebusException.Message);
            }
            catch (Exception exception)
            {
                writer.Write(exception.Message);

                _logger.LogError(exception, "Exception");
            }
        }

        public async Task InterpretAsync(int playerId, Executor executor, string value, ExpressionWriter writer)
        {
            List<IToken> tokens = new List<IToken>();
            ExpressionWriter tokenWriter = new ExpressionWriter();

            await foreach (IToken token in new Tokenizer(_tokenFactory, value).TokenizeAsync())
            {
                tokenWriter.Write('(');
                tokenWriter.Write(token.Type
                    .ToString()
                    .ToUpper());
                tokenWriter.Write(' ');
                tokenWriter.Write(token.Value.ToLower());
                tokenWriter.Write(") ");

                tokens.Add(token);
            }

            _logger.LogInformation("{Tokens}", tokenWriter);

            Expression expression = new Parser(tokens, Localizer).Parse();

            CommandBuilder commandBuilder = new CommandBuilder(Repository, _commands, Localizer);

            await commandBuilder.SetPlayerAsync(playerId);

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, _xmlWriterSettings))
                {
                    _xmlSerializer.Serialize(xmlWriter, expression);
                }

                _logger.LogInformation("{Expression}", stringWriter);
            }

            await expression.InterpretAsync(commandBuilder);

            foreach (Command command in commandBuilder.Build())
            {
                await executor.ExecuteAsync(command, writer);

                writer.WriteLine();
            }
        }
    }
}
