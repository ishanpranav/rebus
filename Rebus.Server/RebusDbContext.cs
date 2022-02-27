// Ishan Pranav's REBUS: RebusDbContext.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rebus.Server.Concepts;

namespace Rebus.Server
{
    internal sealed class RebusDbContext : DbContext
    {
#nullable disable
        public DbSet<CommandSignature> CommandSignatures { get; set; }
        public DbSet<ConceptSignature> ConceptSignatures { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Token> Tokens { get; set; }
#nullable enable

        public RebusDbContext(DbContextOptions options) : base(options) { }

        public IQueryable<TConcept> Query<TConcept>() where TConcept : Concept
        {
            return Set<TConcept>()
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Article)
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Substantive)
                .AsSplitQuery()
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Adjectives);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Concept>()
                .HasDiscriminator<int>("Discriminator")
                .HasValue<MicrocomputerConcept>(1)
                .HasValue<RegionConcept>(2)
                .HasValue<SpacecraftConcept>(3);

            modelBuilder.Entity<ConceptSignature>(coceptSignature =>
            {
                coceptSignature.HasOne(x => x.Article);
                coceptSignature.HasOne(x => x.Substantive);

                coceptSignature
                    .HasMany(x => x.Adjectives)
                    .WithMany(x => x.Signatures);
            });

            if (Database.IsSqlite())
            {
                modelBuilder
                    .Entity<Player>()
                    .Property(x => x.UserId)
                    .UseCollation(Collations.SqliteIgnoreCase);

                modelBuilder
                    .Entity<Token>()
                    .Property(x => x.Value)
                    .UseCollation(Collations.SqliteIgnoreCase);
            }
        }
    }
}
