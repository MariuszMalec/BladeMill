using BladeMill.BLL.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace BladeMill.ConsoleApp.HowToReadConfFromJson
{
    public class ReadJsonFileAsConfig
    {
        public static void ReaderJson(ILogger logger)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var selection = config.GetSection(nameof(AppSettings));
            var appSettings = selection.Get<AppSettings>();
            logger.Information($"{appSettings.DirOneDriveClever}");
        }
    }
}
