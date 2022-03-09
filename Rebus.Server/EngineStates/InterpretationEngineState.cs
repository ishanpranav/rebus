// Ishan Pranav's REBUS: InterpretationEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rebus.ExpressionWriters;

namespace Rebus.Server.EngineStates
{
    internal sealed class InterpretationEngineState : IEngineState
    {
        private readonly Player _player;

        public InterpretationEngineState(Player player)
        {
            _player = player;
        }

        public async Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            IEnumerable<Command> commands;

            await using (RebusDbContext dbContext = await context.Engine.DbContextFactory.CreateDbContextAsync())
            {
                IAsyncEnumerable<IToken> tokens = new Tokenizer(dbContext, value).TokenizeAsync();
                StringExpressionWriter tokenWriter = new StringExpressionWriter();

                await foreach (IToken token in tokens)
                {
                    tokenWriter.Write('(');
                    tokenWriter.Write(token.Type
                        .ToString()
                        .ToUpper());
                    tokenWriter.Write(' ');
                    tokenWriter.Write(token.Value.ToLower());
                    tokenWriter.Write(") ");
                }

                context.Engine.Logger.LogInformation("{Tokens}", tokenWriter);

                Expression expression = await new Parser(tokens).ParseAsync();

                CommandBuilder commandBuilder = new CommandBuilder(dbContext, context.Engine.Commands);

                await commandBuilder.SetPlayerAsync(_player.Id);

                context.Engine.LogExpression(expression);

                await expression.InterpretAsync(commandBuilder);

                commands = commandBuilder.Build();
            }

            if (context.Executor is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                foreach (Command command in commands)
                {
                    await context.Executor.ExecuteAsync(command, writer);

                    writer.WriteLine();
                }
            }
        }
    }
}
