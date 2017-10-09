using System.Threading.Tasks;

using Discord;

namespace Zorro_Bot.Extensions.Embed
{
    public static class BuildEmbed
    {
        public static Task<IUserMessage> Embed(this IMessageChannel chan, string title, string desc, Color color, string embedImage = null, string embedThumbnail = null)
        {
            var eb = new EmbedBuilder();

            eb.Title = title;
            eb.Description = desc;
            eb.Color = color;
            eb.ImageUrl = embedImage ?? "";

            return chan.SendMessageAsync("", false, eb);
        }
    }
}
