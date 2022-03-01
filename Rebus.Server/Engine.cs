// Ishan Pranav's REBUS: Engine.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rebus.Exceptions;
using Rebus.ExpressionWriters;

namespace Rebus.Server
{
    internal sealed class Engine : IEngine
    {
        private readonly ILogger<Engine> _logger;
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;
        private readonly IEditDistance _editDistance;
        private readonly IEnumerable<Rebus.Command> _commands;
        private readonly XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings()
        {
            Async = true,
            Indent = true,
            IndentChars = "    "
        };
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Expression));
        private readonly Dictionary<string, Executor> _executorsByUserId = new Dictionary<string, Executor>();

        public Engine(ILogger<Engine> logger, IDbContextFactory<RebusDbContext> contextFactory, IEditDistance editDistance, IEnumerable<Rebus.Command> commands)
        {
            _logger = logger;
            _contextFactory = contextFactory;
            _editDistance = editDistance;
            _commands = commands;
        }

        public bool IsActive(string userId)
        {
            return _executorsByUserId.ContainsKey(userId);
        }

        public async Task InterpretAsync(string userId, string value, ExpressionWriter writer)
        {
            try
            {
                Executor? executor;
                IEnumerable<Rebus.Command> commands;

                await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
                {
                    if (!_executorsByUserId.TryGetValue(userId, out executor))
                    {
                        Player? player = await context.Players.SingleOrDefaultAsync(x => x.UserId == userId);

                        if (player is null)
                        {
                            player = new Player()
                            {
                                UserId = userId
                            };

                            player.Spacecraft.Add(new Spacecraft());

                            await context.AddAsync(player);
                            await context.SaveChangesAsync();
                        }

                        executor = new Executor(player.Id);

                        _executorsByUserId[userId] = executor;
                    }

                    IAsyncEnumerable<IToken> tokens = new Tokenizer(context, value).TokenizeAsync();
                    StringExpressionWriter tokenWriter = new StringExpressionWriter();

                    await foreach (IToken token in tokens)
                    {
                        using (tokenWriter.CreateScope(ScopeType.Parenthetical))
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

                    Expression expression = await new Parser(tokens).ParseAsync();

                    expression.WriteLine(writer);

                    CommandBuilder commandBuilder = new CommandBuilder(context, _commands);

                    commandBuilder.SetPlayer(executor.Id);

                    await using (StringWriter stringWriter = new StringWriter())
                    {
                        await using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, _xmlWriterSettings))
                        {
                            _xmlSerializer.Serialize(xmlWriter, expression);
                        }

                        _logger.LogInformation("{Expression}", stringWriter);
                    }

                    await expression.InterpretAsync(commandBuilder);

                    commands = commandBuilder.Build();
                }

                foreach (Rebus.Command command in commands)
                {
                    await foreach (IWritable result in executor.ExecuteAsync(command))
                    {
                        result.Write(writer);

                        writer.WriteLine();
                    }

                    writer.WriteLine();
                }

                if (executor.Terminated)
                {
                    _executorsByUserId.Remove(userId);
                }
            }
            catch (RebusSpellingException rebusSpellingException)
            {
                await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
                {
                    string[]? suggestions = null;
                    TokenTypes expectedType = rebusSpellingException.ExpectedType;
                    string? actualValue = rebusSpellingException.ActualValue;

                    if (actualValue is not null)
                    {
                        suggestions = context.Tokens
                            .Where(x => x.Type.HasFlag(expectedType))
                            .Select(x => x.Value)
                            .AsEnumerable()
                            .GroupBy(x => _editDistance.Calculate(x, actualValue))
                            .Where(x => x.Key < 3)
                            .MinBy(x => x.Key)?
                            .ToArray();
                    }

                (await context.CreateMessageAsync(resource: 3, expectedType, actualValue)).Write(writer);
                }
            }
            catch (RebusException rebusException)
            {
                await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
                {
                    (await context.CreateMessageAsync(rebusException.Resource, rebusException.GetArguments())).Write(writer);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception");
            }
        }
    }
}
