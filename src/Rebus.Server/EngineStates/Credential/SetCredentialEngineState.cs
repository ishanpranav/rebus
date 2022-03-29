// Ishan Pranav's REBUS: SetCredentialEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server.EngineStates.Credential
{
    internal sealed class SetCredentialEngineState : CredentialEngineState
    {
        public SetCredentialEngineState(Player player) : base(player) { }

        public override async Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            await context.Engine.Repository.SetCredentialAsync(Player, value);
            await IntroduceAsync(context, writer);
        }
    }
}
