// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Rebus.Server.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            await using (ServiceProvider serviceProvider = new ServiceCollection()
                .AddRebus()
                .AddSingleton(System.Console.In)
                .AddSingleton(System.Console.Out)
                .AddSingleton<Startup>()
                .BuildServiceProvider())
            {
                await serviceProvider
                    .GetRequiredService<Startup>()
                    .StartAsync();
            }
        }
    }
}