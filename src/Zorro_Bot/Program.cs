
namespace Zorro_Bot
{
    public class Program
    {
        public static void Main(string[] args)
            => new Zorro().RunAsync().GetAwaiter().GetResult();
    }
}
