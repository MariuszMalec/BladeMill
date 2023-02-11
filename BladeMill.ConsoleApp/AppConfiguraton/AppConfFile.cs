using BladeMill.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.ConsoleApp.AppConfiguraton
{
    static class AppConfFile
    {
        public static void Tests()
        {
            var appService = new AppXmlConfService();
            Console.WriteLine($"GetNcDir               : {appService.GetNcDir()}");
            Console.WriteLine($"GetRootEngDir          : {appService.GetRootEngDir()}");
            Console.WriteLine($"GetRootMfgDir          : {appService.GetRootMfgDir()}");
            Console.WriteLine($"GetBladeMillScriptsDir : {appService.GetBladeMillScriptsDir()}");
        }
    }
}
