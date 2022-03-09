// Ishan Pranav's REBUS: RenameCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Commands
{
    [Guid("45DA897D-68E8-47D5-A946-76FDE16B826B")]
    internal sealed class RenameCommand : Command
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public RenameCommand(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private RenameCommand(ArgumentSet arguments, IDbContextFactory<RebusDbContext> contextFactory) : base(arguments)
        {
            _contextFactory = contextFactory;
        }

        public override bool Matches(ArgumentSet arguments)
        {
            return arguments.IsPlayer(Argument.Subject) && arguments.IsQuotation(Argument.DirectObject) && arguments.TryGetConcepts(Argument.IndirectObject, out IReadOnlyCollection<Concept>? indirectObjects) && indirectObjects.Count == 1;
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            Concept indirectObject = Arguments
                .GetConcepts(Argument.Subject)
                .First();
            string quotation = Arguments.GetQuotation(Argument.DirectObject);

            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                string[] words = new string(quotation
                    .Where(x => char.IsLetterOrDigit(x))
                    .ToArray())
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                indirectObject.SubstantiveValue = words[words.Length - 1];

                for (int i = 0; i < words.Length - 1; i++)
                {
                    indirectObject.Adjectives.Add(new Adjective()
                    {
                        TokenValue = words[i]
                    });
                }

                await updateAsync(TokenTypes.Substantive, indirectObject.SubstantiveValue);

                foreach (Adjective adjective in indirectObject.Adjectives)
                {
                    await updateAsync(TokenTypes.Adjective, adjective.TokenValue);
                }

                context.Update(indirectObject);

                await context.SaveChangesAsync();

                async Task updateAsync(TokenTypes tokenType, string tokenValue)
                {
                    Token? token = await context.Tokens.FindAsync(tokenValue);

                    if (token is null)
                    {
                        await context.Tokens.AddAsync(new Token()
                        {
                            Type = tokenType,
                            Value = tokenValue
                        });
                        await context.SaveChangesAsync();
                    }
                    else if (!token.Type.HasFlag(tokenType))
                    {
                        token.Type |= tokenType;

                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new RenameCommand(arguments, _contextFactory);
        }
    }
}
