// Ishan Pranav's REBUS: RebusDbContext.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    public class RebusDbContext : DbContext
    {
        private const string IgnoreCaseCollation = "NOCASE";

#nullable disable
        public DbSet<Player> Players { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Zone> Zones { get; set; }
#nullable enable

        public RebusDbContext(DbContextOptions<RebusDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Zone>(x =>
            {
                const string IdPropertyName = "Id";

                x.Property<int>(IdPropertyName);
                x.HasKey(IdPropertyName);
            });

            if (Database.IsSqlite())
            {
                modelBuilder
                    .Entity<Player>()
                    .Property(x => x.Username)
                    .UseCollation(IgnoreCaseCollation);

                modelBuilder
                    .Entity<Unit>()
                    .Property(x => x.Name)
                    .UseCollation(IgnoreCaseCollation);
            }
        }
    }
}
