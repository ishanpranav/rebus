// Ishan Pranav's REBUS: RpcListener.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StreamJsonRpc;

namespace Rebus.Server
{
    public class RpcListener
    {
        private readonly ILogger<RpcListener> _logger;
        private readonly RpcService _service;
        private readonly TcpListener _tcpListener;

        private bool _active = true;

        public RpcListener(IPAddress ipAddress, int port, ILogger<RpcListener> logger, RpcService service)
        {
            _logger = logger;
            _service = service;
            _tcpListener = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            _tcpListener.Start();

            _ = Task.Run(async () =>
            {
                while (_active)
                {
                    try
                    {
                        if (_tcpListener.Pending())
                        {
                            TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();

                            await using (NetworkStream stream = tcpClient.GetStream())
                            {
                                using (MessagePackFormatter messageFormatter = new MessagePackFormatter())
                                using (LengthHeaderMessageHandler messageHandler = new LengthHeaderMessageHandler(stream, stream, messageFormatter))
                                using (JsonRpc jsonRpc = new JsonRpc(messageHandler, _service))
                                {
                                    jsonRpc.StartListening();

                                    await jsonRpc.Completion;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, message: "Exception");
                    }
                    finally
                    {
                        await Task.Delay(millisecondsDelay: 10);
                    }
                }

                _tcpListener.Stop();
            });
        }

        public void Stop()
        {
            _active = false;
        }
    }
}
