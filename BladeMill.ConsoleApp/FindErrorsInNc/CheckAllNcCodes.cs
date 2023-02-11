using BladeMill.BLL;
using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace BladeMill.ConsoleApp.FindErrorsInNc
{
    static class CheckAllNcCodes
    {
        public static void Test(ILogger logger)
        {
            //------------------------------------------------
            //wez wszystkie pliki do sprawdzenia *nc,*spf
            //------------------------------------------------
            logger.Information("Czekaj, sprawdzanie programow ... ");
            var listSubrogramms = new List<SelectedFile>();
            var fileService = new FileService();
            var appConfig = new AppXmlConfService();
            var dir = appConfig.GetNcDir();
            listSubrogramms.AddRange(fileService.GetListSelectedFiles(dir, ".SPF"));
            listSubrogramms.AddRange(fileService.GetListSelectedFiles(dir, ".NC"));
            listSubrogramms.AddRange(fileService.GetListSelectedFiles(dir, ".nc"));
            //listSubrogramms.ForEach(file =>Console.WriteLine($"{file.Id} {file.BatchFile}" ));
            var checkNC = new NcCodeCheckService();
            ConsoleUtility.WriteProgressBar(0);
            for (int i = 0; i < listSubrogramms.Count; i++)
            {
                checkNC.FindErrorsInSubProgram(listSubrogramms[i].BatchFile, i);
                ConsoleUtility.WriteProgressBar((i + 1) * 100 / listSubrogramms.Count, true);
                Thread.Sleep(1);
            }
            Console.WriteLine("-------------------------------------------------------------------------------");
            checkNC.ShowAllErrors();
        }
    }
}
