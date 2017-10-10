using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Discord.Commands;
using Discord.WebSocket;

namespace Zorro_Bot.Extensions.Handler
{
    public class CommandHandlers
    {
        private readonly DiscordSocketClient Client;
        private readonly CommandService CommandService;
        private IServiceProvider Services;

        public CommandHandlers(IServiceProvider services, DiscordSocketClient client, CommandService commands)
        {
            Client = client;
            CommandService = commands;
            Services = services;

            Client.MessageReceived += HandleCommand;
        }

        public async Task InitAsync(IServiceProvider services)
        {
            Services = services;
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage m)
        {
            var msg = m as SocketUserMessage;
            if (msg == null)
                return;

            var Context = new CommandContext(Client, msg);
            int argPos = 0;

            if (!msg.HasStringPrefix("ro;", ref argPos) || msg.Author.IsBot) return;

            var result = await CommandService.ExecuteAsync(Context, argPos, Services);

            if (!result.IsSuccess)
            {
                if (result.Error == CommandError.UnknownCommand)
                    await Context.Channel.SendMessageAsync(":pig: oink oink, ya dun zoinked! Unknown Command!");
            }
        }
    }
}
