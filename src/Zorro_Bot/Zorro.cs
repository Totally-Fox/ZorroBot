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
using Zorro_Bot.Services.Configuration;

namespace Zorro_Bot
{
    public class Zorro
    {
        private DiscordSocketClient ZorroClient;
        private CommandService ZorroCommandService;

        internal static void Main(string[] args)
            => new Zorro().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            Console.WriteLine($"- Starting up Zorro Bot v[{Assembly.GetEntryAssembly().GetName().Version}] -\n");

            ZorroClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                DefaultRetryMode = RetryMode.AlwaysRetry
            });
            //
            ZorroCommandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            });

            var services = ConfigureServices();
            await services.GetRequiredService<CommandHandler>().InitAsync(services);
            //
            var creds = services.GetRequiredService<BotConfiguration>().Load();


            ZorroClient.Log += Log;
            ZorroCommandService.Log += Log;

            await ZorroClient.LoginAsync(TokenType.Bot, creds.Token);
            await ZorroClient.StartAsync();
            //
            await ZorroClient.SetGameAsync(creds.Game);

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base Services
                .AddSingleton(ZorroClient)
                .AddSingleton(ZorroCommandService)
                .AddSingleton<CommandHandler>()
                // Additional Services
                .AddSingleton<AudioService>()
                .AddSingleton<BotConfiguration>()
                .BuildServiceProvider();
        }

        private Task Log(LogMessage m)
        {
            string logM = string.Concat("[", DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss"), "] [", m.Severity, "] ", m.Message, m.Exception);
            //
            Console.WriteLine(logM);
            //
            using (StreamWriter file = new StreamWriter(File.Open(@"Data/Logging/Log.txt", FileMode.Append)))
            {
                file.Write(logM + Environment.NewLine);
            }

            return Task.CompletedTask;
        }
    }
}
