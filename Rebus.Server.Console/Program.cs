// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Rebus.Server.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            await using (ServiceProvider serviceProvider = new ServiceCollection()
                .AddRebus()
                .AddSingleton<ConsoleExpressionWriter>()
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