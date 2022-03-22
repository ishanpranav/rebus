// Ishan Pranav's REBUS: GetCredentialEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Rebus.Server.EngineStates
{
    internal sealed class GetCredentialEngineState : CredentialEngineState
    {
        public GetCredentialEngineState(Player player) : base(player) { }

        public override Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer)
        {
            if (Player.Credential.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return IntroduceAsync(context, writer);
            }
            else
            {
                writer.Write(context.Engine.Localizer["CredentialFailure"]);

                return Task.CompletedTask;
            }
        }
    }
}
