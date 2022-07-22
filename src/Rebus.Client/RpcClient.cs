// Ishan Pranav's REBUS: RpcClient.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using StreamJsonRpc;

namespace Rebus.Client
{
    public class RpcClient<TService> : IAsyncDisposable where TService : class, IDisposable
    {
        private TcpClient? _tcpClient = new TcpClient();
        private NetworkStream? _networkStream;
        private MessagePackFormatter? _messageFormatter = new MessagePackFormatter();
        private LengthHeaderMessageHandler? _messageHandler;
        private JsonRpc? _jsonRpc;
        private TService? _service;

        public TService Service
        {
            get
            {
                return _service ?? throw new InvalidOperationException();
            }
        }

        public async Task ConnectAsync(IPAddress ipAddress, int port)
        {
            if (_tcpClient is null || _messageFormatter is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                await _tcpClient.ConnectAsync(ipAddress, port);

                _networkStream = _tcpClient.GetStream();
                _messageHandler = new LengthHeaderMessageHandler(_networkStream, _networkStream, _messageFormatter);
                _jsonRpc = new JsonRpc(_messageHandler);

                _jsonRpc.StartListening();

                _service = _jsonRpc.Attach<TService>();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(continueOnCapturedContext: false);

            GC.SuppressFinalize(this);
        }

        [SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "DisposeAsyncCore")]
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_networkStream is not null)
            {
                await _networkStream.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);

                _networkStream = null;
            }

            if (_messageHandler is not null)
            {
                await _messageHandler.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);

                _messageHandler = null;
            }

            _tcpClient?.Dispose();
            _tcpClient = null;

            _messageFormatter?.Dispose();
            _messageFormatter = null;

            _jsonRpc?.Dispose();
            _jsonRpc = null;

            _service?.Dispose();
            _service = null;
        }
    }
}
