using BladeMill.BLL.SourceData;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.ConsoleApp.AppConfiguraton
{
    static class PathsData
    {
        public static void Tests(ILogger logger)
        {
            var path = new PathDataBase();
            path.SetDirs().ToList().ForEach(p => Console.WriteLine($"{p}"));
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine($"AppConfFile   : {path.GetApplicationConfFile()}");
            Console.WriteLine($"GetCleverHome : {path.GetCleverHome()}");
            Console.WriteLine($"GetCurrentXml : {path.GetFileCurrentToolsXml()}");
            Console.WriteLine($"{path.GetGITAddfile()}");
            Console.WriteLine($"{path.GetGITCommitfile()}");
            Console.WriteLine($"{path.GetGITInitfile()}");
            var getAppConfDir = Path.GetDirectoryName(path.GetApplicationConfFile());
            Console.WriteLine("----------------------------------------------");
            if (!Directory.Exists(getAppConfDir))
            {
                logger.Error($"Brak katalogu {getAppConfDir}!!");
            }

            //var pathList = path.SetDirs();
            ////Console.WriteLine($"{pathList.ToList().Select(p=>p.DirOrders).FirstOrDefault()}");
            //pathList.ToList().ForEach(p => Console.WriteLine(p));
            ////var path = new PathDataBase();
            ////Console.WriteLine($"{path.GetDirTask()}");
        }
    }
}
