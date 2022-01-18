// Ishan Pranav's REBUS: UniverseInitializer.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Rebus.Server
{
    internal class Initializer : IAsyncSupportInitialize
    {
        private readonly IDbContextFactory<UniverseContext> _contextFactory;
        private readonly JsonRepository _jsonRepository;
        private readonly CommandRepository _commandRepository;

        public Initializer(IDbContextFactory<UniverseContext> contextFactory, JsonRepository jsonRepository, CommandRepository commandRepository)
        {
            this._contextFactory = contextFactory;
            this._jsonRepository = jsonRepository;
            this._commandRepository = commandRepository;
        }

        public async Task InitializeAsync()
        {
            await using (UniverseContext context = await this._contextFactory.CreateDbContextAsync())
            {
                if (await context.Database.EnsureCreatedAsync())
                {
                    await context.Tokens.AddRangeAsync(await this._jsonRepository.GetAsync<Token>());
                    await context.SaveChangesAsync();
                    await context.Concepts.AddRangeAsync(await this._jsonRepository.GetAsync<Concept>());
                    await context.SaveChangesAsync();

                    foreach (string verb in this._commandRepository.GetVerbs())
                    {
                        await UpdateTokenAsync(context, TokenTypes.Verb, verb);
                    }

                    foreach (string adverb in this._commandRepository.GetAdverbs())
                    {
                        await UpdateTokenAsync(context, TokenTypes.Adverb, adverb);
                    }
                }
            }
        }

        private static async Task UpdateTokenAsync(UniverseContext context, TokenTypes type, string value)
        {
            Token? token = await context.Tokens.SingleOrDefaultAsync(x => x.Value == value);

            if (token is null)
            {
                await context.Tokens.AddAsync(new Token()
                {
                    Type = type,
                    Value = value
                });
            }
            else
            {
                token.Type |= type;
            }

            await context.SaveChangesAsync();
        }
    }
}
