using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Discord;
using Discord.Audio;

using YoutubeExplode;
using YoutubeExplode.Models;

namespace Zorro_Bot.Services.AudioService
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels =
            new ConcurrentDictionary<ulong, IAudioClient>();

        private readonly ConcurrentDictionary<ulong, Process> ConnectedChannelProcesses =
            new ConcurrentDictionary<ulong, Process>();

        private readonly string BasePath = @"Data/Audio/";
        private string FilePath = @"";

        public async Task JoinAudio(IGuild g, IVoiceChannel target)
        {
            if (ConnectedChannels.TryGetValue(g.Id, out IAudioClient _client))
                return;

            if (target.Guild.Id != g.Id)
                return;

            var audioClient = await target.ConnectAsync();
            if (ConnectedChannels.TryAdd(g.Id, audioClient)) { }
        }

        public async Task LeaveAudio(IGuild g)
        {
            if (ConnectedChannels.TryRemove(g.Id, out IAudioClient _client))
            {
                await _client.StopAsync();

                if (ConnectedChannelProcesses.TryRemove(g.Id, out Process _proc))
                    _proc.Kill();
            }
        }

        public async Task SendAudio(IGuild g, IMessageChannel c, string input)
        {
            var yt = new YoutubeClient();

            if (input.ToLower().Contains("youtube.com"))
                input = YoutubeClient.ParseVideoId(input);
            else
            {
                var res = await yt.SearchAsync(input);
                input = res.FirstOrDefault();
            }

            var vInfo = await yt.GetVideoInfoAsync(input);
            var vStreams = vInfo.AudioStreams.OrderBy(x => x.Bitrate).Last();
            var vTitle = vInfo.Title;

            FilePath = $@"{BasePath}{g.Id}/{vTitle}.{vStreams.Container.GetFileExtension()}";
            if (!Directory.Exists(BasePath + g.Id))
                Directory.CreateDirectory(BasePath + g.Id);

            if (!File.Exists(FilePath))
            {
                using (var goIn = await yt.GetMediaStreamAsync(vStreams))
                using (var goOut = File.Create(FilePath))
                    await goIn.CopyToAsync(goOut);
            }

            if (ConnectedChannels.TryGetValue(g.Id, out IAudioClient _client))
            {
                var dStream = _client.CreatePCMStream(AudioApplication.Music);
                await CreateStream(FilePath, g.Id).StandardOutput.BaseStream.CopyToAsync(dStream);
                await dStream.FlushAsync();
            }
        }

        private Process CreateStream(string p, ulong id)
        {
            Process GuildAudio = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{p}\" -vol 80 -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });

            if (ConnectedChannelProcesses.TryAdd(id, GuildAudio))
                return GuildAudio;
            else
                return GuildAudio;
        }
    }
}
