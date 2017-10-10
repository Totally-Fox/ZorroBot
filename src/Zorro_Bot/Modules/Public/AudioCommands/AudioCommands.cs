using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using YoutubeExplode;

using Zorro_Bot.Services.AudioService;

namespace Zorro_Bot.Modules.Public.AudioCommands
{
    public class AudioCommands : ModuleBase
    {
        private static Dictionary<ulong, List<string>> SongQueue =
            new Dictionary<ulong, List<string>>();

        private string NextSong, LeftInQueue;
        private readonly YoutubeClient _yt = new YoutubeClient();

        private readonly AudioService _service;

        public AudioCommands(AudioService service)
            => _service = service;

        [Command("bap", RunMode = RunMode.Async)]
        [Alias("connect")]
        [Summary("Joins Voice")]
        [Remarks("ro;bap")]
        public async Task JoinAsync()
        {
            if ((Context.Message.Author as IVoiceState).VoiceChannel == null)
                await Context.Channel.SendMessageAsync("Join voice first");
            else
            {
                await Context.Channel.SendMessageAsync("Here comes Zorro Boi :blush:");

                await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
                await (await Context.Guild.GetCurrentUserAsync() as SocketGuildUser).ModifyAsync(x => x.Deaf = true);
            }
        }

        [Command("yap", RunMode = RunMode.Async)]
        [Alias("disconnect")]
        [Summary("Leaves Voice")]
        [Remarks("ro;yap")]
        public async Task LeaveAsync()
        {
            await Context.Channel.SendMessageAsync("Doin a leave :point_right:");
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("add", RunMode = RunMode.Async)]
        [Summary("Add music to your queue")]
        [Remarks("ro;add <song name or youtube url>")]
        public async Task AddQueueSong([Remainder] string LinkOrName)
        {
            if (string.IsNullOrWhiteSpace(LinkOrName))
                await Context.Channel.SendMessageAsync("Add something to the command, like the song name or url!");

            var list = new List<string>();

            if (SongQueue.ContainsKey(Context.Guild.Id))
                SongQueue.TryGetValue(Context.Guild.Id, out list);

            if (LinkOrName.ToLower().Contains("youtube.com"))
                LinkOrName = YoutubeClient.ParseVideoId(LinkOrName);
            else
                LinkOrName = (await _yt.SearchAsync(LinkOrName)).FirstOrDefault();

            var vTitle = (await _yt.GetVideoInfoAsync(LinkOrName)).Title;

            list.Add(vTitle);
            SongQueue.Remove(Context.Guild.Id);
            SongQueue.Add(Context.Guild.Id, list);

            await Context.Channel.SendMessageAsync($"`{vTitle}` was placed in the queue :blush:"); 
        }

        [Command("play", RunMode = RunMode.Async)]
        [Summary("Begins playing music in the queue")]
        [Remarks("ro;play")]
        public async Task PlayAsync()
        {
            List<string> list;

            if (SongQueue.ContainsKey(Context.Guild.Id))
                SongQueue.TryGetValue(Context.Guild.Id, out list);
            else
            {
                await Context.Channel.SendMessageAsync("Add some spicy songs to your queue first :hot_pepper::fire:");
                return;
            }

            if ((await Context.Guild.GetCurrentUserAsync()).GuildPermissions.DeafenMembers != true)
                await Context.Channel.SendMessageAsync("Let me deafen myself first please!");
            else if ((Context.Message.Author as IVoiceState).VoiceChannel == null)
                await Context.Channel.SendMessageAsync("How about you join voice first :scream_cat:");
            else
                while (list.Count > 0)
                {
                    await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
                    await (await Context.Guild.GetCurrentUserAsync() as SocketGuildUser).ModifyAsync(x => x.Deaf = true);

                    NextSong = list.Count != 1 ? $"**Following Song:** `{list.ElementAt(1)}`" : "";
                    LeftInQueue = list.Count == 1 ? "Only one song remaining!" : $"Currently {list.Count} songs in queue";

                    await Context.Channel.SendMessageAsync($"**Now Playing:** `{list.First()}`/n/n{NextSong}/n{LeftInQueue}");

                    await _service.SendAudio(Context.Guild, Context.Channel, list.First());

                    list.RemoveAt(0);
                    SongQueue.Remove(Context.Guild.Id);
                    SongQueue.Add(Context.Guild.Id, list);
                    SongQueue.TryGetValue(Context.Guild.Id, out list);
                }

            if (list.Count == 0)
                await Context.Channel.SendMessageAsync("No more music! Add more or kick me out :heart:");
        }
    }
}
