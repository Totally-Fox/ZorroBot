using System.Net.Http;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Zorro_Bot.Extensions.Json;

namespace Zorro_Bot.Modules.Public.General
{
    public class General : ModuleBase
    {
            [Command("dog")]
            public async Task Dog()
            {
                using (var _http = new HttpClient())
                {
                    await Context.Channel.SendMessageAsync($"http://random.dog/" + await _http.GetStringAsync("http://random.dog/woof").ConfigureAwait(false));
                }
            }

            // Edit GIFS to work with embeds
            [Command("dancebattle")]
            public async Task DanceBattle()
            {
                await Context.Channel.SendMessageAsync("http://i.imgur.com/cebGrXw.gifv \n http://i.imgur.com/Yy07lvG.gifv");
            }

            [Command("din")]
            public async Task Din()
            {
                await Context.Channel.SendMessageAsync("", false, new EmbedBuilder()
                    .WithTitle("Rodin")
                    .WithDescription(":sparkles: ***Beautiful*** :sparkles:")
                    .WithColor(new Color(178, 35, 16))
                    .WithImageUrl("http://i.imgur.com/zngMzUs.gifv").Build());
            }
        
            [Command("key")]
            public async Task KeyAsync(string key)
            {
                await Context.Channel.ReplyJson(key);
            }
        }
    }



