// Ishan Pranav's REBUS: RebusDbContext.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Rebus.Server.Concepts;

namespace Rebus.Server
{
    internal sealed class RebusDbContext : DbContext
    {
#nullable disable
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<CommandSignature> CommandSignatures { get; set; }
        public DbSet<Navigation> Navigations { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Spacecraft> Spacecraft { get; set; }
        public DbSet<Star> Stars { get; set; }
        public DbSet<Token> Tokens { get; set; }
#nullable enable

        public RebusDbContext(DbContextOptions<RebusDbContext> options) : base(options) { }

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
                    .UseCollation(ignoreCaseCollation)
                    .HasDefaultValue(string.Empty);

                player
                    .Property(x => x.Credential)
                    .UseCollation(ignoreCaseCollation)
                    .HasDefaultValue(string.Empty);

                player
                    .Property(x => x.Sequence)
                    .HasDefaultValue(1);
            });

            modelBuilder.Entity<Resource>(resource =>
            {
                resource
                    .Property(x => x.Key)
                    .UseCollation(ignoreCaseCollation)
                    .HasDefaultValue(string.Empty);

                resource
                    .Property(x => x.Value)
                    .HasDefaultValue(string.Empty);
            });

            modelBuilder
                .Entity<Token>()
                .Property(x => x.Value)
                .UseCollation(ignoreCaseCollation);
        }
    }
}
