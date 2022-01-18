// Ishan Pranav's REBUS: SocketCommandModule.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Globalization;

namespace Rebus.Server.Discord
{
    public class SocketCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly IEngine _engine;

        public SocketCommandModule(IEngine engine)
        {
            this._engine = engine;
        }

        [Alias("-", "I")]
        [Command("execute")]
        public async Task ExecuteAsync([Remainder] string value)
        {
            SocketUser user = this.Context.User;
            StringExpressionWriter writer = new StringExpressionWriter();

            await this._engine.InterpretAsync(user.Id.ToString(CultureInfo.InvariantCulture), value, writer);
            await user.SendMessageAsync(writer.ToString());
        }
    }
}
