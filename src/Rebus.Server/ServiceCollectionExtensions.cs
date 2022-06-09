// Ishan Pranav's REBUS: ServiceCollectionExtensions.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rebus.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRebus(this IServiceCollection source)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddUserSecrets(typeof(ServiceCollectionExtensions).Assembly)
                .Build();

            return source
                .AddLogging(x => x
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information))
                .AddLocalization()
                .AddSingleton<IConfiguration>(configurationRoot);
        }
    }
}
