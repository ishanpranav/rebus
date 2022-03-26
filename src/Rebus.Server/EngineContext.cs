// Ishan Pranav's REBUS: EngineContext.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Rebus.Server.EngineStates;

namespace Rebus.Server
{
    internal sealed class EngineContext
    {
        public IEngine Engine { get; }
        public IEngineState State { get; set; } = new InitialEngineState();
        public Executor Executor { get; set; } = new Executor();

        public EngineContext(IEngine engine)
        {
            Engine = engine;
        }
    }
}
