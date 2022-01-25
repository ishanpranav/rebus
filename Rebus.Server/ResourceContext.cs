// Ishan Pranav's REBUS: ResourceContext.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class ResourceContext : DbContext
    {
#nullable disable
        public DbSet<Resource> Resources { get; set; }
        public DbSet<CommandSignature> CommandSignatures { get; set; }
#nullable enable

        public ResourceContext(DbContextOptions<ResourceContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (Database.IsSqlite())
            {
                modelBuilder
                    .Entity<CommandSignature>(x =>
                    {
                        x.Property(x => x.Verb)
                         .UseCollation(Collations.SqliteIgnoreCase);

                        x.Property(x => x.Adverb)
                         .UseCollation(Collations.SqliteIgnoreCase);
                    });
            }
        }
    }
}
