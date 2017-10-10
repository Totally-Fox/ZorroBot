using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Zorro_Bot.Extensions.Handler;
using Zorro_Bot.Services.AudioService;

namespace Zorro_Bot
{
    public class Zorro
    {
        private DiscordSocketClient Client;
        private CommandService CommandService;

        internal static void Main(string[] args)
            => new Zorro().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            Console.WriteLine($"- Starting up Zorro Bot v[{Assembly.GetEntryAssembly().GetName().Version}] -\n");

            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                DefaultRetryMode = RetryMode.AlwaysRetry
            });

            CommandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            });

            var services = ConfigureServices();
            await services.GetRequiredService<CommandHandlers>().InitAsync(services);


            Client.Log += Log;
            CommandService.Log += Log;

            await Client.LoginAsync(TokenType.Bot, "");
            await Client.StartAsync().ConfigureAwait(false);
            await Client.SetGameAsync("Yap at the Moon");

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton<CommandHandlers>()
                .AddSingleton<AudioService>()
                .BuildServiceProvider();
        }

        private static Task Log(LogMessage m)
        {
            Console.WriteLine(string.Concat("[", DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss"), "] [", m.Severity, "] ", m.Message));

            using (StreamWriter file = new StreamWriter(File.Open(@"Data/Text/Log.txt", FileMode.Append)))
            {
                file.Write(string.Concat("[", DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss"), "] [", m.Severity, "] ", m.Message) + Environment.NewLine);
            }

            return Task.CompletedTask;
        }
    }
}
