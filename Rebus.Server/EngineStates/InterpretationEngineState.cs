// Ishan Pranav's REBUS: InterpretationEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server.EngineStates
{
    internal sealed class InterpretationEngineState : IEngineState
    {
        private readonly Player _player;

        public InterpretationEngineState(Player player)
        {
            _player = player;
        }

        public Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            return context.Engine.InterpretAsync(_player.Id, context.Executor, value, writer);
        }
    }
}
