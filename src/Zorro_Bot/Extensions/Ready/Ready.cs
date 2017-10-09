using System.Threading.Tasks;

namespace Zorro_Bot.Extensions.Ready
{
    public class Ready
    {
        public Ready()
        {
            Zorro.Client.Ready += HandleReady;
        }

        public async Task HandleReady()
            => await Zorro.Client.SetGameAsync("Yip Yip");
    }
}
