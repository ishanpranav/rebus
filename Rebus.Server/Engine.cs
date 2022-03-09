// Ishan Pranav's REBUS: Engine.cs
// Copyright (c) Ishan Pranav. All rights reserved.
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

        public ILogger Logger { get; }
        public IDbContextFactory<RebusDbContext> DbContextFactory { get; }
        public IEnumerable<Command> Commands { get; }
        public IEditDistance EditDistance { get; }

        public Engine(ILogger<Engine<T>> logger, IDbContextFactory<RebusDbContext> dbContextFactory, IEditDistance editDistance, IEnumerable<Command> commands)
        {
            Logger = logger;
            DbContextFactory = dbContextFactory;
            EditDistance = editDistance;
            Commands = commands;
        }

        public async Task InterpretAsync(T user, string value, ExpressionWriter writer)
        {
            writer.WriteLine();

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
                await using (RebusDbContext dbContext = await context.Engine.DbContextFactory.CreateDbContextAsync())
                {
                    Token[]? suggestions = null;
                    TokenTypes? expectedType = rebusSpellingException.ExpectedType;
                    string? actualValue = rebusSpellingException.ActualValue;
                    int resource = 4;

                    if (actualValue is not null)
                    {
                        IQueryable<Token> tokens = dbContext.Tokens;

                        if (expectedType is not null)
                        {
                            tokens = tokens.Where(x => x.Type.HasFlag(expectedType.Value));

                            resource = 3;
                        }

                        suggestions = tokens
                            .AsEnumerable()
                            .GroupBy(x => context.Engine.EditDistance.Calculate(x.Value, actualValue))
                            .Where(x => x.Key < 3)
                            .MinBy(x => x.Key)?
                            .ToArray();
                    }

                (await dbContext.CreateMessageAsync(resource, expectedType, suggestions)).Write(writer);
                }
            }
            catch (RebusException rebusException)
            {
                await using (RebusDbContext dbContext = await context.Engine.DbContextFactory.CreateDbContextAsync())
                {
                    (await dbContext.CreateMessageAsync(rebusException.Resource, rebusException.GetArguments())).Write(writer);
                }
            }
            catch (Exception exception)
            {
                context.Engine.Logger.LogError(exception, "Exception");
            }
        }

        public void LogExpression(Expression value)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, _xmlWriterSettings))
                {
                    _xmlSerializer.Serialize(xmlWriter, value);
                }

                Logger.LogInformation("{Expression}", stringWriter);
            }
        }
    }
}
