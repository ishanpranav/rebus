// Ishan Pranav's REBUS: ConsoleEngineContext.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading.Tasks;

namespace Rebus.Server.Console
{
    internal class Startup
    {
        private readonly TextReader _reader;
        private readonly IEngine _engine;
        private readonly ConsoleExpressionWriter _writer = new ConsoleExpressionWriter();

        public Startup(TextReader reader, IEngine engine)
        {
            this._reader = reader;
            this._engine = engine;
        }

        public async Task StartAsync()
        {
            await this._engine.InitializeAsync();

            string? playerTag = await this._reader.ReadLineAsync();

            if (playerTag is not null)
            {
                string? line;

                while (!String.IsNullOrWhiteSpace(line = await this._reader.ReadLineAsync()))
                {
                    await this._engine.InterpretAsync(playerTag, line, this._writer);
                }
            }
        }
    }
}