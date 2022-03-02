// Ishan Pranav's REBUS: Startup.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rebus.ExpressionWriters;
using SimpleTCP;

namespace Rebus.Server.Tcp
{
    internal sealed class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly TcpOptions _options;
        private readonly IEngine _engine;

        public Startup(ILogger<Startup> logger, TcpOptions options, IEngine engine)
        {
            _logger = logger;
            _options = options;
            _engine = engine;
        }

        public async Task StartAsync()
        {
            SimpleTcpServer simpleTcpServer = new SimpleTcpServer()
            {
                AutoTrimStrings = true,
                Delimiter = (byte)'\n',
                StringEncoder = Encoding.UTF8,
            };
            Dictionary<TcpClient, string> userIdsByClient = new Dictionary<TcpClient, string>();

            simpleTcpServer.ClientConnected += async (sender, e) =>
            {
                _logger.LogInformation("Client connected from {RemoteEndPoint}", e.Client.RemoteEndPoint);

                await sendAsync("ishan Pranav's REBUS", e);
                await sendAsync("copyright (c) 2021-2022 Ishan Pranav.all rights reserved.this software is licensed with the MIT license.", e);
                await sendAsync("version 1.0.0 | Connected via TCP/IP", e);
                await sendAsync("greetings,I am REBUS,your microcomputer assistant.with my instruments and your intellect,we will command vast fleets of spacecraft and explore the galaxy.i was created at Arnold O.beckman High School in Irvine,California,between 2021 and 2022 by Ishan Pranav.my design is inspired by early multi-user dungeon systems and the interactive fiction computer game genre.my software is written in C# for the Microsoft .NET 6.0 cross-platform runtime.", e);
                await sendAsync("please enter your username to log on.", e);
                await promptAsync(e);
            };

            simpleTcpServer.ClientDisconnected += (sender, e) =>
            {
                _logger.LogInformation("Client disconnected from {RemoteEndPoint}", e.Client.RemoteEndPoint);

                userIdsByClient.Remove(e);
            };

            simpleTcpServer.DataReceived += async (sender, e) =>
            {
                if (userIdsByClient.TryGetValue(e.TcpClient, out string? userId))
                {
                    _logger.LogInformation("Client sent \"{MessageString}\" from {RemoteEndPoint}", e.MessageString, e.TcpClient.Client.RemoteEndPoint);

                    WrappedExpressionWriter writer = new WrappedExpressionWriter();

                    await _engine.InterpretAsync(userId, e.MessageString, writer);

                    writer.WriteLine();
                    writer.Wrap();

                    e.ReplyLine(writer.ToString());

                    await promptAsync(e.TcpClient);

                    _logger.LogInformation("Replied to client ({UserId}): \"{MessageString}\" at {RemoteEndPoint}", userId, writer, e.TcpClient.Client.RemoteEndPoint);
                }
                else
                {
                    _logger.LogInformation("Client sent self-identifier \"{MessageString}\" from {RemoteEndPoint}", e.MessageString, e.TcpClient.Client.RemoteEndPoint);

                    if (!string.IsNullOrEmpty(e.MessageString))
                    {
                        userIdsByClient.Add(e.TcpClient, e.MessageString);

                        await sendAsync($"welcome,{e.MessageString}.I await your instructions.", e.TcpClient);
                    }

                    await promptAsync(e.TcpClient);
                }
            };

            simpleTcpServer.Start(_options.Port);

            await Task.Delay(Timeout.Infinite);

            static async Task sendAsync(string message, TcpClient tcpClient)
            {
                WrappedExpressionWriter writer = new WrappedExpressionWriter();

                writer.Write(message);
                writer.WriteLine();
                writer.WriteLine();
                writer.Wrap();

                await tcpClient
                    .GetStream()
                    .WriteAsync(Encoding.UTF8.GetBytes(writer.ToString()));
            }

            static async Task promptAsync(TcpClient tcpClient)
            {
                await tcpClient
                    .GetStream()
                    .WriteAsync(Encoding.UTF8.GetBytes(">>"));
            }
        }
    }
}
