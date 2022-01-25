// Ishan Pranav's REBUS: UniverseContext.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rebus.Server
{
    internal sealed class UniverseContext : DbContext
    {
#nullable disable
        public DbSet<ConceptSignature> ConceptSignatures { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<Player> Players { get; internal set; }
        public DbSet<Token> Tokens { get; set; }
#nullable enable

        public UniverseContext(DbContextOptions options) : base(options) { }

        public async Task UpdateTokenAsync(TokenTypes type, string value)
        {
            Token? token = await Tokens.SingleOrDefaultAsync(x => x.Value == value);

            if (token is null)
            {
                await Tokens.AddAsync(new Token()
                {
                    Type = type,
                    Value = value
                });
            }
            else
            {
                token.Type |= type;
            }

            await SaveChangesAsync();
        }

        public IOrderedQueryable<Concept> IncludeUniverse()
        {
            return Concepts
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Article)
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Substantive)
                .AsSplitQuery()
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Adjectives)
                .OrderBy(x => x.Characteristics);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Concept>()
                .HasOne<Concept>()
                .WithMany()
                .HasForeignKey(x => x.ContainerId);

            modelBuilder.Entity<ConceptSignature>(coceptSignature =>
            {
                coceptSignature.HasOne(x => x.Article);
                coceptSignature.HasOne(x => x.Substantive);

                coceptSignature
                    .HasMany(x => x.Adjectives)
                    .WithMany(x => x.Signatures);
            });

            EntityTypeBuilder<Token> token = modelBuilder.Entity<Token>();
            EntityTypeBuilder<Player> player = modelBuilder.Entity<Player>();

            if (Database.IsSqlite())
            {
                player
                    .Property(x => x.UserId)
                    .UseCollation(Collations.SqliteIgnoreCase);

                token
                    .Property(x => x.Value)
                    .UseCollation(Collations.SqliteIgnoreCase);
            }
        }
    }
}
