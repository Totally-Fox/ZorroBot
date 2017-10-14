using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Discord.Commands;
using Discord.WebSocket;

using Zorro_Bot.Services.Configuration;
using Zorro_Bot.Extensions.ZorroEmbeding;

namespace Zorro_Bot.Extensions.Handler
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient ZorroClient;
        private readonly CommandService ZorroCommandService;
        private IServiceProvider ZorroServices;

        public CommandHandler(IServiceProvider services, DiscordSocketClient client, CommandService commands)
        {
            ZorroClient = client;
            ZorroCommandService = commands;
            ZorroServices = services;

            ZorroClient.MessageReceived += HandleCommand;
        }

        public async Task InitAsync(IServiceProvider services)
        {
            ZorroServices = services;
            await ZorroCommandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage m)
        {
            var msg = m as SocketUserMessage;
            if (msg == null)
                return;

            var Context = new CommandContext(ZorroClient, msg);
            int argPos = 0;

            if (!msg.HasStringPrefix(ZorroServices.GetRequiredService<BotConfiguration>().Load().Prefix, ref argPos) || msg.Author.IsBot) return;

            var result = await ZorroCommandService.ExecuteAsync(Context, argPos, ZorroServices);

            if (!result.IsSuccess)
            {
                if (result.Error == CommandError.UnknownCommand)
                    await Context.Channel.PrettifyMessage(":pig: oink oink, ya dun zoinked! Unknown Command!");
                else
                    await Context.Channel.PrettifyMessage(result.ErrorReason);
            }
        }
    }
}
