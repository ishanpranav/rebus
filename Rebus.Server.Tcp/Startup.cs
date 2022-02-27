// Ishan Pranav's REBUS: Startup.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
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
        private readonly IPlayerRepository _playerRepository;
        private readonly IEngine _engine;
        private readonly MessageFactory _messageFactory;

        public Startup(ILogger<Startup> logger, TcpOptions options, IPlayerRepository playerRepository, IEngine engine, MessageFactory messageFactory)
        {
            _logger = logger;
            _options = options;
            _playerRepository = playerRepository;
            _engine = engine;
            _messageFactory = messageFactory;
        }

        public async Task StartAsync()
        {
            SimpleTcpServer simpleTcpServer = new SimpleTcpServer()
            {
                AutoTrimStrings = true,
                Delimiter = (byte)'\n',
                StringEncoder = Encoding.UTF8,
            };
            Dictionary<TcpClient, int> playersByClient = new Dictionary<TcpClient, int>();

            simpleTcpServer.ClientConnected += async (sender, e) =>
            {
                _logger.LogInformation("Client connected from {RemoteEndPoint}", e.Client.RemoteEndPoint);

                await sendMessageAsync(await _messageFactory.CreateMessageAsync(11), e);
                await sendMessageAsync(await _messageFactory.CreateMessageAsync(12), e);
                await sendMessageAsync(await _messageFactory.CreateMessageAsync(13, "TCP/IP"), e);
                await sendMessageAsync(await _messageFactory.CreateMessageAsync(14), e);
                await sendMessageAsync(await _messageFactory.CreateMessageAsync(15), e);
                await promptAsync(e);
            };

            simpleTcpServer.ClientDisconnected += (sender, e) =>
            {
                _logger.LogInformation("Client disconnected from {RemoteEndPoint}", e.Client.RemoteEndPoint);
            };

            simpleTcpServer.DelimiterDataReceived += async (sender, e) =>
            {
                if (playersByClient.TryGetValue(e.TcpClient, out int player))
                {
                    _logger.LogInformation("Client sent \"{MessageString}\" from {RemoteEndPoint}", e.MessageString, e.TcpClient.Client.RemoteEndPoint);

                    WrappedExpressionWriter writer = new WrappedExpressionWriter();

                    bool terminated = !await _engine.InterpretAsync(player, e.MessageString, writer);

                    writer.WriteLine();
                    writer.Wrap();

                    e.ReplyLine(writer.ToString());

                    await promptAsync(e.TcpClient);

                    _logger.LogInformation("Replied to client ({ConceptId}): \"{MessageString}\" at {RemoteEndPoint}", player, writer, e.TcpClient.Client.RemoteEndPoint);

                    if (terminated)
                    {
                        _logger.LogInformation("Terminated client ({ConceptId}) at {RemoteEndPoint}", player, e.TcpClient.Client.RemoteEndPoint);

                        e.TcpClient.Close();
                    }
                }
                else
                {
                    _logger.LogInformation("Client sent self-identifier \"{MessageString}\" from {RemoteEndPoint}", e.MessageString, e.TcpClient.Client.RemoteEndPoint);

                    if (!string.IsNullOrEmpty(e.MessageString))
                    {
                        playersByClient.Add(e.TcpClient, await _playerRepository.GetPlayerAsync(e.MessageString));

                        await sendMessageAsync(await _messageFactory.CreateMessageAsync(16, e.MessageString), e.TcpClient);
                    }

                    await promptAsync(e.TcpClient);
                }
            };

            simpleTcpServer.Start(_options.Port);

            await Task.Delay(Timeout.Infinite);

            static async Task sendMessageAsync(IWritable message, TcpClient tcpClient)
            {
                WrappedExpressionWriter writer = new WrappedExpressionWriter();

                message.Write(writer);

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
