// Ishan Pranav's REBUS: SetCredentialEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server.EngineStates
{
    internal sealed class SetCredentialEngineState : CredentialEngineState
    {
        public SetCredentialEngineState(Player player) : base(player) { }

        public override Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            Player.Credential = value;

            return IntroduceAsync(context, writer);
        }
    }
}
