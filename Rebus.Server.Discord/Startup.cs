// Ishan Pranav's REBUS: Startup.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Rebus.Server.Discord
{
    internal class Startup
    {
        private readonly StartupOptions _options;
        private readonly ILogger<Startup> _logger;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        public Startup(IOptions<StartupOptions> options, ILogger<Startup> logger, DiscordSocketClient discordSocketClient, CommandService commandService, IServiceProvider serviceProvider)
        {
            this._options = options.Value;
            this._logger = logger;
            this._discordSocketClient = discordSocketClient;
            this._commandService = commandService;
            this._serviceProvider = serviceProvider;
        }

        private static LogLevel GetLogLevel(LogSeverity logSeverity)
        {
            switch (logSeverity)
            {
                case LogSeverity.Critical:
                    return LogLevel.Critical;

                case LogSeverity.Error:
                    return LogLevel.Error;

                case LogSeverity.Warning:
                    return LogLevel.Warning;

                case LogSeverity.Info:
                    return LogLevel.Information;

                case LogSeverity.Verbose:
                    return LogLevel.Trace;

                case LogSeverity.Debug:
                    return LogLevel.Debug;

                default:
                    return LogLevel.None;
            }
        }

        public async Task StartAsync()
        {
            this._discordSocketClient.MessageReceived += async (socketMessage) =>
            {
                if (socketMessage is SocketUserMessage socketUserMessage && socketUserMessage.Author.Id != this._discordSocketClient.CurrentUser.Id && !socketUserMessage.Author.IsBot)
                {
                    SocketCommandContext context = new SocketCommandContext(this._discordSocketClient, socketUserMessage);

                    IResult result = await this._commandService.ExecuteAsync(context, argPos: 0, this._serviceProvider);

                    if (!result.IsSuccess)
                    {
                        await context.Channel.SendMessageAsync(result.ToString());
                    }
                }
            };

            this._discordSocketClient.Log += (logMessage) =>
            {
                this._logger.Log(GetLogLevel(logMessage.Severity), "Discord: {Message}", logMessage.Message);

                return Task.CompletedTask;
            };

            await this._discordSocketClient.LoginAsync(TokenType.Bot, this._options.Token);
            await this._discordSocketClient.StartAsync();

            await this._commandService.AddModuleAsync<SocketCommandModule>(this._serviceProvider);

            await Task.Delay(Timeout.Infinite);
        }
    }
}