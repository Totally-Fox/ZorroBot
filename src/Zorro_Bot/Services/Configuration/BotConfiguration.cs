using System.IO;

using Newtonsoft.Json;

namespace Zorro_Bot.Services.Configuration
{
    public class BotConfiguration
    {
        private readonly string ConfigPath = @"Data/Configuration/Bot/BotConfig.json";

        /// <summary>
        /// Load bot stuff from Config file
        /// </summary>
        /// <returns>Bot Config Items</returns>
        public BotConfigItems Load()
        {
            if (File.Exists(ConfigPath))
                return JsonConvert.DeserializeObject<BotConfigItems>(File.ReadAllText(ConfigPath));
            else
                throw new FileNotFoundException("Configuration File Not Found!");
        }
    }

    public class BotConfigItems
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }
    }
}
