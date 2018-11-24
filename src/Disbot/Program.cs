﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace Disbot
{
    public class Program
    {
        private const string DISCORD_BOT_TOKEN_VARIABLE = "DiscordBotToken";
        private const char COMMAND_PREFIX = '!';

        private static DiscordSocketClient _discordClient;
        private static CommandService _discordCommandService;
        private static IServiceCollection _serviceCollection;
        private static IServiceProvider _provider;

        private static async Task Main(string[] args)
        {
            Log.Logger = CreateLogger();

            _serviceCollection = new ServiceCollection();

            _discordClient = CreateDiscordClient();

            _discordCommandService = CreateDiscordCommandService();

            _discordClient.Log += HandleDiscordLog;
            _discordCommandService.Log += HandleDiscordLog;
            _discordClient.MessageReceived += HandleMessageReceived;

            await _discordCommandService.AddModulesAsync(Assembly.GetEntryAssembly());

            _provider = _serviceCollection.BuildServiceProvider();

            var discordToken = GetDiscordToken();

            await _discordClient.LoginAsync(TokenType.Bot, discordToken);

            await _discordClient.StartAsync();

            // Start an infinite delay to wait for, so we don't shut down
            await Task.Delay(-1);
        }

        private static async Task HandleMessageReceived(SocketMessage messageReceived)
        {
            if (!(messageReceived is SocketUserMessage message))
                return;

            var commandPrefixPosition = 0;

            if (!message.HasCharPrefix(COMMAND_PREFIX, ref commandPrefixPosition))
                return;

            var context = new CommandContext(_discordClient, message);

            using (var scope = _provider.CreateScope())
            {
                var result = await _discordCommandService.ExecuteAsync(context, commandPrefixPosition, scope.ServiceProvider);

                if (!result.IsSuccess)
                {
                    Log.Error("Something went wrong while running the command: {returnedMessage}", result);
                }
            }
        }

        private static Task HandleDiscordLog(LogMessage arg)
        {
            switch(arg.Severity)
            {
                case LogSeverity.Critical:
                    Log.Fatal(arg.Exception, arg.Message);
                    break;
                case LogSeverity.Error:
                    Log.Error(arg.Exception, arg.Message);
                    break;
                case LogSeverity.Warning:
                    Log.Warning(arg.Exception, arg.Message);
                    break;
                case LogSeverity.Info:
                    Log.Information(arg.Exception, arg.Message);
                    break;
                case LogSeverity.Verbose:
                    Log.Verbose(arg.Exception, arg.Message);
                    break;
                case LogSeverity.Debug:
                    Log.Debug(arg.Exception, arg.Message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.CompletedTask;
        }

        private static Logger CreateLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static DiscordSocketClient CreateDiscordClient()
        {
            return new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });
        }

        private static CommandService CreateDiscordCommandService()
        {
            return new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug,
            });
        }

        private static string GetDiscordToken()
        {
            return Environment.GetEnvironmentVariable(DISCORD_BOT_TOKEN_VARIABLE);
        }
    }
}
