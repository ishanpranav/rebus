// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Client.Windows.JsonConverters;

namespace Rebus.Client.Windows
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<LoginForm>()
                .AddTransient<MainForm>()
                .AddSingleton(x =>
                {
                    Credentials result = x
                        .GetRequiredService<ObjectSaver>()
                        .Load<Credentials>();

                    result.ApplyCulture();

                    return result;
                })
                .AddSingleton<JsonConverter, JsonCultureInfoConverter>()
                .AddSingleton<JsonConverter, JsonIPAddressConverter>()
                .AddSingleton(x =>
                {
                    JsonSerializerOptions result = new JsonSerializerOptions()
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    foreach (JsonConverter converter in x.GetServices<JsonConverter>())
                    {
                        result.Converters.Add(converter);
                    }

                    return result;
                })
                .AddSingleton<ObjectSaver>()
                .AddSingleton(typeof(RpcClient<>))
                .BuildServiceProvider();

            Application.ApplicationExit += onApplicationExit;

            async void onApplicationExit(object? sender, EventArgs e)
            {
                await serviceProvider.DisposeAsync();
            }

            Application.Run(serviceProvider.GetRequiredService<LoginForm>());
        }
    }
}
