// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rebus.Server.Tcp
{
    internal static class Program
    {
        private static void Main()
        {
            using (ServiceProvider serviceProvider = new ServiceCollection()
                 .AddRebus()
                 .AddSingleton<Startup>()
                 .AddSingleton<IWrapper, Wrapper>()
                 .AddSingleton(x => x
                    .GetRequiredService<IConfiguration>()
                    .GetRequiredSection(nameof(TcpOptions))
                    .Get<TcpOptions>())
                 .BuildServiceProvider())
            {
                 serviceProvider
                    .GetRequiredService<Startup>()
                    .Start();
            }
        }
    }
}
