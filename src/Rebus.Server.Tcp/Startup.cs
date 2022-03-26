// Ishan Pranav's REBUS: Startup.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleTCP;

namespace Rebus.Server.Tcp
{
    internal sealed class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly TcpOptions _options;
        private readonly IEngine<TcpClient> _engine;
        private readonly IWrapper _wrapper;

        public Startup(ILogger<Startup> logger, TcpOptions options, IEngine<TcpClient> engine, IWrapper wrapper)
        {
            _logger = logger;
            _options = options;
            _engine = engine;
            _wrapper = wrapper;
        }

        public async Task StartAsync()
        {
            SimpleTcpServer simpleTcpServer = new SimpleTcpServer()
            {
                AutoTrimStrings = true,
                Delimiter = (byte)'\n',
                StringEncoder = Encoding.UTF8
            };

            simpleTcpServer.ClientConnected += (sender, e) =>
            {
                _logger.LogInformation("Client connected from {RemoteEndPoint}", e.Client.RemoteEndPoint);
            };

            simpleTcpServer.ClientDisconnected += (sender, e) =>
            {
                _logger.LogInformation("Client disconnected from {RemoteEndPoint}", e.Client.RemoteEndPoint);
            };

            simpleTcpServer.DataReceived += async (sender, e) =>
            {
                _logger.LogInformation("Client sent \"{MessageString}\" from {RemoteEndPoint}", e.MessageString, e.TcpClient.Client.RemoteEndPoint);

                ExpressionWriter writer = new ExpressionWriter();

                await _engine.InterpretAsync(e.TcpClient, e.MessageString, writer);

                writer.WriteLine();
                writer.Wrap(_wrapper);

                e.ReplyLine(writer.ToString());

                _logger.LogInformation("Replied to client \"{MessageString}\" at {RemoteEndPoint}", writer, e.TcpClient.Client.RemoteEndPoint);
            };

            simpleTcpServer.Start(IPAddress.Loopback, _options.Port);

            foreach (IPAddress listeningIP in simpleTcpServer.GetListeningIPs())
            {
                _logger.LogInformation("Listening on {ListeningIP} Port {Port}", listeningIP, _options.Port);
            }

            await Task.Delay(Timeout.Infinite);
        }
    }
}
