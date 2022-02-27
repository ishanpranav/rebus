// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rebus.Server.Tcp
{
    internal static class Program
    {
        private static async Task Main()
        {
            ServiceCollection services = new ServiceCollection();

            await using (ServiceProvider serviceProvider = services
                 .AddRebus()
                 .AddSingleton<Startup>()
                 .AddSingleton(x => x
                    .GetRequiredService<IConfiguration>()
                    .GetRequiredSection(nameof(TcpOptions))
                    .Get<TcpOptions>())
                 .BuildServiceProvider())
            {
                await serviceProvider
                    .GetRequiredService<Startup>()
                    .StartAsync();
            }
        }
    }
}
