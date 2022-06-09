// Ishan Pranav's REBUS: Startup.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Rebus.Server.Discord
{
    internal sealed class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly DiscordOptions _options;
        private readonly DiscordSocketClient _discordSocketClient;

        public Startup(ILogger<Startup> logger, DiscordOptions options, DiscordSocketClient discordSocketClient)
        {
            _logger = logger;
            _options = options;
            _discordSocketClient = discordSocketClient;
        }

        public async Task StartAsync()
        {
            _discordSocketClient.MessageReceived += (socketMessage) =>
            {
                if (socketMessage is SocketUserMessage socketUserMessage)
                {
                    SocketSelfUser currentUser = _discordSocketClient.CurrentUser;
                    string content = socketUserMessage.Content;

                    if (socketUserMessage.Author.Id != currentUser.Id && !socketUserMessage.Author.IsBot && (content.StartsWith(currentUser.Mention) || content.StartsWith('~')))
                    {
                        int start;

                        if (content.StartsWith(currentUser.Mention))
                        {
                            start = currentUser.Mention.Length;
                        }
                        else if (content.StartsWith('~'))
                        {
                            start = 1;
                        }
                        else
                        {
                            return Task.CompletedTask;
                        }

                        SocketCommandContext context = new SocketCommandContext(_discordSocketClient, socketUserMessage);
                    }
                }

                return Task.CompletedTask;
            };

            _discordSocketClient.Log += (logMessage) =>
                {
                    _logger.Log(getLogLevel(logMessage.Severity), "Discord: {Message}", logMessage.Message);

                    return Task.CompletedTask;

                    static LogLevel getLogLevel(LogSeverity logSeverity)
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
                            case LogSeverity.Verbose:
                                return LogLevel.Information;

                            case LogSeverity.Debug:
                                return LogLevel.Debug;

                            default:
                                return LogLevel.None;
                        }
                    }
                };

            await _discordSocketClient.LoginAsync(TokenType.Bot, _options.Token);
            await _discordSocketClient.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
}
