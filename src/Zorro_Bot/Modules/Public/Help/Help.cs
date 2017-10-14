using System.Threading.Tasks;

using Discord.Commands;


namespace Zorro_Bot.Modules.Public.Help
{
    public class Help : ModuleBase
    {
        [Command("help")]
        public async Task HelpMe()
        {
            await Context.Channel.SendMessageAsync("Listen, I need some help too sometimes :dog:");
        }

    }
}
