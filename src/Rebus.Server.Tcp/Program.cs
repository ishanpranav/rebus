// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rebus.Server.Tcp
{
    internal static class Program
    {
        private static async Task Main()
        {
            using (ServiceProvider serviceProvider = new ServiceCollection()
                .AddDbContextFactory<RebusDbContext>((serviceProvider, optionsBuilder) => optionsBuilder.UseSqlite(serviceProvider
                    .GetRequiredService<IConfiguration>()
                    .GetConnectionString(nameof(RebusDbContext))))
                .AddLogging(x => x.AddConsole())
                .AddSingleton<FisherYatesShuffle>()
                .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
                    .AddJsonFile(path: "appsettings.json")
                    .AddUserSecrets(typeof(Program).Assembly)
                    .Build())
                .AddSingleton(x => x
                    .GetRequiredService<IConfiguration>()
                    .GetSection("Rebus"))
                .AddSingleton(x =>
                {
                    string dataDirectory = x
                        .GetRequiredService<IConfigurationSection>()
                        .GetValue<string>(key: "dataDirectory");

                    List<string> getNames(Depth depth)
                    {
                        List<string> results = new List<string>();

                        using (StreamReader streamReader = new StreamReader(Path.ChangeExtension(Path.Combine(dataDirectory, $"{depth}Dictionary"), "txt")))
                        {
                            string? line;

                            while ((line = streamReader.ReadLine()) is not null)
                            {
                                results.Add(line);
                            }
                        }

                        return results;
                    }

                    return new Namer(x.GetRequiredService<FisherYatesShuffle>(), getNames(Depth.Constellation), getNames(Depth.Star), getNames(Depth.Planet));
                })
                .AddSingleton(x =>
                {
                    IConfigurationSection section = x.GetRequiredService<IConfigurationSection>();

                    return new Map(new JuliaSet(new Complex(section.GetValue<double>(key: "real"), section.GetValue<double>(key: "imaginary")), section.GetValue<double>(key: "r")), HexPoint.Empty, section.GetValue<int>(key: "radius"), section.GetValue<double>(key: "zoom"));
                })
                .AddSingleton(x => new Random(x
                    .GetRequiredService<IConfigurationSection>()
                    .GetValue<int>(key: "seed")))
                .AddSingleton(x =>
                {
                    IConfigurationSection section = x.GetRequiredService<IConfigurationSection>();

                    return new RpcListener(IPAddress.Parse(section.GetValue<string>(key: "ipAddress")), section.GetValue<int>(key: "port"), x.GetRequiredService<ILogger<RpcListener>>(), x.GetRequiredService<RpcService>());
                })
                .AddSingleton<RpcService>()
                .BuildServiceProvider())
            {
                serviceProvider
                    .GetRequiredService<RpcListener>()
                    .Start();

                await Task.Delay(Timeout.Infinite);
            }
        }
    }
}
