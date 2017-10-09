using System.Reflection;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

namespace Zorro_Bot.Extensions.Handler
{
    public class CommandHandlers
    {
        public async Task StartAsync()
        {
            await Zorro.CommandService.AddModulesAsync(Assembly.GetEntryAssembly());
            Zorro.Client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage m)
        {
            var msg = m as SocketUserMessage;
            if (msg == null)
                return;

            var Context = new CommandContext(Zorro.Client, msg);
            int argPos = 0;

            if (!msg.HasStringPrefix("ro;", ref argPos) || msg.Author.IsBot) return;

            var result = await Zorro.CommandService.ExecuteAsync(Context, argPos);

            if (!result.IsSuccess)
            {
                if (result.Error == CommandError.UnknownCommand)
                    await Context.Channel.SendMessageAsync(":pig: oink oink, ya dun zoinked! Unknown Command!");
            }
        }
    }
}
