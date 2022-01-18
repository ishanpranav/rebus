// Ishan Pranav's REBUS: RepositoryContext.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;

namespace Rebus.Server
{
    internal class UniverseContext : DbContext
    {
        private const string SqliteIgnoreCaseCollation = "NOCASE";

#nullable disable
        public DbSet<Token> Tokens { get; set; }
        public DbSet<ConceptSignature> ConceptSignatures { get; set; }
        public DbSet<Concept> Concepts { get; set; }
#nullable enable

        public UniverseContext(DbContextOptions options) : base(options) { }

        public IIncludableQueryable<Concept, ICollection<Token>> IncludeUniverse() 
        {
            return this.Concepts
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Substantive)
                .AsSplitQuery()
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Adjectives);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConceptSignature>(entityTypeBuilder =>
            {
                entityTypeBuilder
                    .HasMany(x => x.Adjectives)
                    .WithMany(x => x.Signatures);

                entityTypeBuilder.HasOne(x => x.Substantive);
            });

            EntityTypeBuilder<Token> tokenEntityType = modelBuilder.Entity<Token>();
            EntityTypeBuilder<Concept> conceptEntityType = modelBuilder.Entity<Concept>();

            if (this.Database.IsSqlite())
            {
                conceptEntityType
                    .Property(x => x.Tag)
                    .UseCollation(SqliteIgnoreCaseCollation);

                tokenEntityType
                    .Property(x => x.Value)
                    .UseCollation(SqliteIgnoreCaseCollation);
            }
        }
    }
}
