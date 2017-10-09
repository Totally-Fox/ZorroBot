using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Zorro_Bot.Extensions.Ready;
using Zorro_Bot.Extensions.Handler;

namespace Zorro_Bot
{
    public class Zorro
    {
        public static DiscordSocketClient Client { get; private set; }
        public static CommandService CommandService { get; private set; }

        #region Run Stuff
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

            Client.Log += Log;
            CommandService.Log += Log;
            #endregion

            #region Login Attempt
            try
            {
                await Client.LoginAsync(TokenType.Bot, "");
                await Client.StartAsync().ConfigureAwait(false);
            }
            catch
            {
                Console.Write("Login Failed :: Exiting in [3] seconds");
                await Task.Delay(3000);
                Environment.Exit(0);
            }
            #endregion

            await new CommandHandlers().StartAsync();
            new Ready();

            await Task.Delay(-1);
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
