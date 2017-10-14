using System;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace Zorro_Bot.Modules.Public.SpringCleaning
{
    public class SpringCleaning : ModuleBase
    {
        [Command("springclean")]
        public async Task SpringCleanAsync(int _a)
        {
            if (_a < 1) { await Context.Channel.SendMessageAsync(":panda_face: Can't clean what isn't there mi oso!"); return; }

            await Context.Message.DeleteAsync().ConfigureAwait(false);

            int limit = (_a < 100) ? _a + 1 : 100;

            var _m = (await Context.Channel.GetMessagesAsync(limit: limit).Flatten().ConfigureAwait(false)).Where(x => DateTime.Now - x.CreatedAt < TimeSpan.FromDays(14));

            if (_m.FirstOrDefault()?.Id == Context.Message.Id)
                _m = _m.Skip(1).ToArray();
            else
                _m = _m.Take(_a);

            await (Context.Channel as ITextChannel).DeleteMessagesAsync(_m);
            await Context.Channel.SendMessageAsync($":panda_face: Spring Cleaning Oso has deleted `{_m.Count()}` messages! :leaves:");
        }
    }
}
