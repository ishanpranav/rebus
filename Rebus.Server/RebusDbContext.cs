// Ishan Pranav's REBUS: RebusDbContext.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Server.Concepts;

namespace Rebus.Server
{
    internal sealed class RebusDbContext : DbContext, ITokenFactory
    {
#nullable disable
        public DbSet<CommandSignature> CommandSignatures { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Spacecraft> Spacecraft { get; set; }
        public DbSet<Star> Stars { get; set; }
        public DbSet<Token> Tokens { get; set; }
#nullable enable

        public RebusDbContext(DbContextOptions<RebusDbContext> options) : base(options) { }

        public IQueryable<Concept> GetKnownViewers()
        {
            return Spacecraft;
        }

        public async Task<IWritable> CreateMessageAsync(int resource, params object?[] arguments)
        {
            object[] filteredArguments = arguments
                .OfType<object>()
                .Where(x => x is not IEnumerable enumerable || enumerable
                    .Cast<object>()
                    .Any())
                .ToArray();
            int count = filteredArguments.Length;

            return new Message(await Resources
                .Where(x => x.Key == resource && x.Arguments == count)
                .Select(x => x.Value)
                .OrderBy(x => EF.Functions.Random())
                .FirstOrDefaultAsync() ?? $"ERROR ({resource} : {count})", filteredArguments);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            string? ignoreCaseCollation = null;

            if (Database.IsSqlite())
            {
                ignoreCaseCollation = "NOCASE";
            }

            modelBuilder
                .Entity<Adjective>()
                .HasKey(x => new
                {
                    x.TokenValue,
                    x.ConceptId
                });

            modelBuilder.Entity<Concept>(concept =>
            {
                const string Discriminator = "Type";

                concept
                    .HasDiscriminator<char>(Discriminator)
                    .HasValue<Direction>('D')
                    .HasValue<Planet>('P')
                    .HasValue<Spacecraft>('S')
                    .HasValue<Star>('T');

                concept
                    .Property(Discriminator)
                    .HasDefaultValue(' ');

                concept.HasOne(x => x.Article);
                concept.HasOne(x => x.Substantive);

                concept
                    .HasMany(x => x.Adjectives)
                    .WithOne()
                    .HasForeignKey(x => x.ConceptId);
            });

            modelBuilder.Entity<CommandSignature>(commandSignature =>
            {
                commandSignature
                    .HasOne<Token>()
                    .WithMany()
                    .HasForeignKey(x => x.VerbValue);

                commandSignature
                    .HasOne<Token>()
                    .WithMany()
                    .HasForeignKey(x => x.AdverbValue);
            });

            modelBuilder.Entity<Player>(player =>
            {
                player
                    .Property(x => x.UserId)
                    .UseCollation(ignoreCaseCollation);

                player
                    .Property(x => x.Credential)
                    .UseCollation(ignoreCaseCollation);
            });

            modelBuilder
                .Entity<Token>()
                .Property(x => x.Value)
                .UseCollation(ignoreCaseCollation);
        }

        public async Task<IToken> CreateTokenAsync(string value)
        {
            return (await Tokens.SingleOrDefaultAsync(x => x.Value == value)) ?? new Token()
            {
                Value = value
            };
        }
    }
}
