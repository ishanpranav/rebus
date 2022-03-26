// Ishan Pranav's REBUS: CredentialEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server.EngineStates
{
    internal abstract class CredentialEngineState : IEngineState
    {
        protected Player Player { get; }

        protected CredentialEngineState(Player player)
        {
            Player = player;
        }

        public async Task IntroduceAsync(EngineContext context, ExpressionWriter writer)
        {
            foreach (Fleet fleet in context.Engine.Repository.GetFleets(Player.Id))
            {
                context.Engine.Controller.Report(writer, fleet);

                await context.Engine.Controller.ViewRegionAsync(writer, fleet.Region);
            }

            writer.Write(context.Engine.Localizer["EmptyParsingError"]);

            context.State = new InterpretationEngineState(Player);
        }

        public abstract Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer);
    }
}
