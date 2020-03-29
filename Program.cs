using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable All

namespace Senate
{
    class Program
    {
        static void Main(string[] args) => new Program()
            .RunBotAsync()
            .GetAwaiter()
            .GetResult();

        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        private async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();
            var token = Hidden.token;
            client.Log += Log;
            await RegisterCommandsAsync();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task RegisterCommandsAsync()
        {
            client.MessageReceived += HandleMessages;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleMessages(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new CommandContext(client, message);
            int argpos = 0;

            if (message.Author.IsBot==false)
            {
                var result = await commands.ExecuteAsync(context, argpos, services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }
    }
}