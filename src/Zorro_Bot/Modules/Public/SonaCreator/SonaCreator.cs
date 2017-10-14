using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using Discord.Commands;

namespace Zorro_Bot.Modules.Public.SonaCreator
{
    public class SonaCreator : ModuleBase
    {
        // Global Base Image Paths
        static readonly string BasePath = @"Data/Img/";
        static readonly string OutputPath = @"Data/Img/Output/";

        // Global Image Var Defalts
        Image _Base = Image.FromFile(BasePath + "Bases/def.png");
        Image _Species = null;
        Image _Eyes = null;
        Image _Ears = null;
        Image _Hair = null;
        Image _Mouth = null;
        Image _Tail = null;
        Image _Accesory = null;

        [Command("sonahelp")]
        public async Task SonaHelpAsync()
        {
            await Context.Channel.SendMessageAsync("", false, new Discord.EmbedBuilder()
                .WithTitle("Sona Builder")
                .WithDescription("To create your sona, type `ro;sona 1 1 2 1 3 1 2` but with any number 1-3 \n\n In order, numbers are: \n\n -Species \n -Eyes \n -Ears \n -Hair \n -Mouth \n -Tail \n -Accesory")
                .WithColor(new Discord.Color(239, 199, 55))
                .Build());
        }

        [Command("sona")]
        public async Task SonaAsync(int species, int eyes, int ears, int hair, int mouth, int tail, int accesory)
        {
            #region Species
            switch (species)
            {
                case 1:
                    _Species = Image.FromFile(BasePath + "Species/1.png");
                    break;
                case 2:
                    _Species = Image.FromFile(BasePath + "Species/2.png");
                    break;
                case 3:
                    _Species = Image.FromFile(BasePath + "Species/3.png");
                    break;
            }
            #endregion
            #region Eyes
            switch (eyes)
            {
                case 1:
                    _Eyes = Image.FromFile(BasePath + "Eyes/1.png");
                    break;
                case 2:
                    _Eyes = Image.FromFile(BasePath + "Eyes/2.png");
                    break;
                case 3:
                    _Eyes = Image.FromFile(BasePath + "Eyes/3.png");
                    break;
            }
            #endregion
            #region Ears
            switch (ears)
            {
                case 1:
                    _Ears = Image.FromFile(BasePath + "Ears/1.png");
                    break;
                case 2:
                    _Ears = Image.FromFile(BasePath + "Ears/2.png");
                    break;
                case 3:
                    _Ears = Image.FromFile(BasePath + "Ears/3.png");
                    break;
            }
            #endregion
            #region Hair
            switch (hair)
            {
                case 1:
                    _Hair = Image.FromFile(BasePath + "Hair/1.png");
                    break;
                case 2:
                    _Hair = Image.FromFile(BasePath + "Hair/2.png");
                    break;
                case 3:
                    _Hair = Image.FromFile(BasePath + "Hair/3.png");
                    break;
            }
            #endregion
            #region Mouth
            switch (mouth)
            {
                case 1:
                    _Mouth = Image.FromFile(BasePath + "Mouth/1.png");
                    break;
                case 2:
                    _Mouth = Image.FromFile(BasePath + "Mouth/2.png");
                    break;
                case 3:
                    _Mouth = Image.FromFile(BasePath + "Mouth/3.png");
                    break;
            }
            #endregion
            #region Tail
            switch (tail)
            {
                case 1:
                    _Tail = Image.FromFile(BasePath + "Tail/1.png");
                    break;
                case 2:
                    _Tail = Image.FromFile(BasePath + "Tail/2.png");
                    break;
                case 3:
                    _Tail = Image.FromFile(BasePath + "Tail/3.png");
                    break;
            }
            #endregion
            #region Accesory
            switch (accesory)
            {
                case 1:
                    _Accesory = Image.FromFile(BasePath + "Accesory/1.png");
                    break;
                case 2:
                    _Accesory = Image.FromFile(BasePath + "Accesory/2.png");
                    break;
                case 3:
                    _Accesory = Image.FromFile(BasePath + "Accesory/3.png");
                    break;
            }
            #endregion

            Image newImage = new Bitmap(_Base.Width, _Base.Height);

            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.DrawImage(_Species, new Point(-80, -80));
                gr.DrawImage(_Eyes, new Point(-80, -80));
                gr.DrawImage(_Ears, new Point(-80, -80));
                gr.DrawImage(_Hair, new Point(-80, -80));
                gr.DrawImage(_Mouth, new Point(-80, -80));
                gr.DrawImage(_Tail, new Point(-80, -80));
                gr.DrawImage(_Accesory, new Point(-80, -80));
            }

            newImage.Save(OutputPath + $"{Context.Message.Author.Id}.png", ImageFormat.Png);

            await Context.Channel.SendFileAsync(OutputPath + $"{Context.Message.Author.Id}.png");
        }
    }
}
