using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

using Discord;
using Discord.Commands;
using Discord.Audio;

using Zorro_Bot.Extensions.Embed;
using Zorro_Bot.Extensions.Json;

namespace Zorro_Bot.Modules.Public.Zorro
{
    public class Commands : ModuleBase<ICommandContext>
    {
            #region Help
            [Command("help")]
            public async Task Help()
            {
                await Context.Channel.SendMessageAsync("Listen, I need some help too sometimes :dog:");
            }
            #endregion

            #region Spring Cleaning
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

                await Context.Channel.DeleteMessagesAsync(_m).ConfigureAwait(false);
                await Context.Channel.SendMessageAsync($":panda_face: Spring Cleaning Oso has deleted `{_m.Count()}` messages! :leaves:");
            }
            #endregion

            #region Dog
            [Command("dog")]
            public async Task Dog()
            {
                using (var _http = new HttpClient())
                {
                    await Context.Channel.SendMessageAsync($"http://random.dog/" + await _http.GetStringAsync("http://random.dog/woof").ConfigureAwait(false));
                }
            }
            #endregion

            //Edit GIFS to work with embeds
            #region Dance Battle
            [Command("dancebattle")]
            public async Task DanceBattle()
            {
                await Context.Channel.SendMessageAsync("http://i.imgur.com/cebGrXw.gifv \n http://i.imgur.com/Yy07lvG.gifv");
            }
            #endregion

            #region Rodin
            [Command("din")]
            public async Task Din()
            {
                await Context.Channel.Embed("Rodin", ":sparkles: ***Beautiful*** :sparkles:", new Discord.Color(178, 35, 16), "http://i.imgur.com/zngMzUs.gifv");
            }
            #endregion
        
            #region Key
            [Command("key")]
            public async Task KeyAsync(string key)
            {
                await Context.Channel.ReplyJson(key);
            }
            #endregion

            //Try to have connect for, like, longer than a second maybe?
            #region Beautiful
            [Command("beautiful", RunMode = RunMode.Async)]
            public async Task Beautiful(IVoiceChannel channel = null)
            {
                channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;

                if (channel == null)
                {
                    await Context.Message.Channel.SendMessageAsync("Can't hear me if you're not in a voice channel, how about joining one? :sparkling_heart:"); return;
                }

                //await Context.Channel.Embed("", ":sparkles: ***Beautiful*** :sparkles:", new Color(160, 72, 219), "");
                var IAudioClient = await channel.ConnectAsync();
            }
        #endregion
    }
    }



