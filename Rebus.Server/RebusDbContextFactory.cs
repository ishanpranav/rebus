// Ishan Pranav's REBUS: RebusDbContextFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Rebus.Server
{
    internal sealed class RebusDbContextFactory : IDbContextFactory<RebusDbContext>, IDesignTimeDbContextFactory<RebusDbContext>
    {
        private readonly IConfiguration _configuration;

        public RebusDbContextFactory()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(ServiceCollectionExtensions.AppSettingsJsonFile)
                .Build();
        }

        public RebusDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RebusDbContext CreateDbContext()
        {
            return new RebusDbContext(new DbContextOptionsBuilder<RebusDbContext>()
                .UseSqlite(_configuration.GetConnectionString(nameof(RebusDbContext)), x => x
                    .MigrationsHistoryTable("Migration"))
                .Options);
        }

        public RebusDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }
    }
}
