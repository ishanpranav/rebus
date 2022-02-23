// Ishan Pranav's REBUS: Startup.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Globalization;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Rebus.ExpressionWriters;

namespace Rebus.Server.Discord
{
    internal sealed class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly StartupOptions _options;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IPlayerRepository _playerRepository;
        private readonly IEngineFactory _engineFactory;

        public Startup(ILogger<Startup> logger, StartupOptions options, DiscordSocketClient discordSocketClient, IPlayerRepository playerRepository, IEngineFactory engineFactory)
        {
            _logger = logger;
            _options = options;
            _discordSocketClient = discordSocketClient;
            _playerRepository = playerRepository;
            _engineFactory = engineFactory;
        }

        public async Task StartAsync()
        {
            IEngine engine = await _engineFactory.CreateEngineAsync();

            _discordSocketClient.MessageReceived += (socketMessage) =>
            {
                if (socketMessage is SocketUserMessage socketUserMessage)
                {
                    Task.Run(async () =>
                    {
                        await OnMessageReceivedAsync(engine, socketUserMessage).ConfigureAwait(false);

                        async Task OnMessageReceivedAsync(IEngine engine, SocketUserMessage socketUserMessage)
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
                                    return;
                                }

                                SocketCommandContext context = new SocketCommandContext(_discordSocketClient, socketUserMessage);
                                SocketUser user = context.User;
                                MarkdownExpressionWriter writer = new MarkdownExpressionWriter();

                                IPlayer player = await _playerRepository.GetPlayerAsync(user.Username, user.Id.ToString(CultureInfo.InvariantCulture));

                                await engine.InterpretAsync(player, content.Substring(start), writer);
                                await user.SendMessageAsync(writer.ToString());
                            }
                        }

                    });
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
