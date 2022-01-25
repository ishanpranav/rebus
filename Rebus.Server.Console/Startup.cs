// Ishan Pranav's REBUS: Startup.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server.Console
{
    internal sealed class Startup
    {
        private readonly ConsoleExpressionWriter _writer;
        private readonly IPlayerRepository _playerRepository;
        private readonly IEngineFactory _engineFactory;

        public Startup(ConsoleExpressionWriter writer, IPlayerRepository playerRepository, IEngineFactory engineFactory)
        {
            _writer = writer;
            _playerRepository = playerRepository;
            _engineFactory = engineFactory;
        }

        public async Task StartAsync()
        {
            IEngine engine = await _engineFactory.CreateEngineAsync();

            string? name = await System.Console.In.ReadLineAsync();

            if (name is not null)
            {
                IPlayer player = await _playerRepository.GetPlayerAsync(name, name);

                string? line;

                while (!string.IsNullOrWhiteSpace(line = await System.Console.In.ReadLineAsync()))
                {
                    await engine.InterpretAsync(player, line, _writer);
                }
            }
        }
    }
}