// Ishan Pranav's REBUS: RebusDbContextFactory.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Rebus.Server
{
    internal sealed class RebusDbContextFactory : IDbContextFactory<RebusDbContext>, IDesignTimeDbContextFactory<RebusDbContext>
    {
        private readonly DbContextOptions<RebusDbContext> _options;

        public RebusDbContextFactory() : this(
            LoggerFactory.Create(x => x.AddConsole()),
            new ConfigurationBuilder()
                .AddUserSecrets(typeof(ServiceCollectionExtensions).Assembly)
                .Build())
        { }

        public RebusDbContextFactory(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _options = new DbContextOptionsBuilder<RebusDbContext>()
                .UseLoggerFactory(loggerFactory)
                .UseSqlite(configuration.GetConnectionString(nameof(RebusDbContext)), x => x.MigrationsHistoryTable("dotnet_migration")).Options;
        }

        public RebusDbContext CreateDbContext()
        {
            return new RebusDbContext(_options);
        }

        public RebusDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }
    }
}
