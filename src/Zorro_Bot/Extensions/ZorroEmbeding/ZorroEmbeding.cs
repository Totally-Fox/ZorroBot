using System;
using System.Threading.Tasks;

using Discord;

namespace Zorro_Bot.Extensions.ZorroEmbeding
{
    public static class ZorroEmbeding
    {
        private static Random rand = new Random();

        public static Task<IUserMessage> PrettifyMessage(this IMessageChannel chan, string msg)
            => chan.SendMessageAsync("", false, new EmbedBuilder().WithDescription(msg).WithColor(new Color(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255))).Build());
    }
}
