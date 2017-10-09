using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Newtonsoft.Json;

using Discord;

namespace Zorro_Bot.Extensions.Json
{
    public static class JsonResponse
    {
        private static Map jMap { get; set; }

        public static Task<IUserMessage> ReplyJson(this IMessageChannel chan, string _k)
        {
            var jFile = File.ReadAllText(@"Data/Json/Yap.json", new UTF8Encoding(false));
            var jDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jFile);
            jMap = new Map(jDict);

            if (_k == "list")
            {
                string list = "**List of Keys**\n\n";
                int count = 1;

                foreach (var k in jMap.rMap)
                {
                    list += $"**{count}:** *{k.Key}*\n";
                    count++;
                }

                return chan.SendMessageAsync(list);
            }
            else
                return chan.SendMessageAsync(jMap.rMap[_k]);
        }
    }

    public class Map
    {
        public IReadOnlyDictionary<string, string> rMap { get; private set; }

        public Map(IDictionary<string, string> map)
        {
            rMap = new ReadOnlyDictionary<string, string>(map);
        }
    }
}
