// Ishan Pranav's REBUS: Startup.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Globalization;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rebus.ExpressionWriters;

namespace Rebus.Server.Discord
{
    internal sealed class Startup
    {
        private readonly StartupOptions _options;
        private readonly ILogger<Startup> _logger;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IPlayerRepository _playerRepository;
        private readonly IEngineFactory _engineFactory;

        public Startup(IOptions<StartupOptions> options, ILogger<Startup> logger, DiscordSocketClient discordSocketClient, IPlayerRepository playerRepository, IEngineFactory engineFactory)
        {
            _options = options.Value;
            _logger = logger;
            _discordSocketClient = discordSocketClient;
            _playerRepository = playerRepository;
            _engineFactory = engineFactory;
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
                case LogSeverity.Verbose:
                    return LogLevel.Information;

                case LogSeverity.Debug:
                    return LogLevel.Debug;

                default:
                    return LogLevel.None;
            }
        }

        private async Task OnMessageReceivedAsync(IEngine engine, SocketUserMessage socketUserMessage)
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
                StringExpressionWriter writer = new StringExpressionWriter();

                IPlayer player = await _playerRepository.GetPlayerAsync(user.Username, user.Id.ToString(CultureInfo.InvariantCulture));

                await engine.InterpretAsync(player, content.Substring(start), writer);
                await user.SendMessageAsync(writer.ToString());
            }
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
                        await
                            OnMessageReceivedAsync(engine, socketUserMessage)
                            .ConfigureAwait(false);
                    });
                }

                return Task.CompletedTask;
            };

            _discordSocketClient.Log += (logMessage) =>
            {
                _logger.Log(GetLogLevel(logMessage.Severity), "Discord: {Message}", logMessage.Message);

                return Task.CompletedTask;
            };

            await _discordSocketClient.LoginAsync(TokenType.Bot, _options.Token);
            await _discordSocketClient.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
}