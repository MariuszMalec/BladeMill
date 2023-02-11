using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using BladeMill.ConsoleApp.VcProjectCreation;
using Serilog;
using System;
using System.IO;

namespace BladeMill.ConsoleApp.TransferOrder
{
    public class OrderTransfer
    {
        public static void Make(ILogger logger)
        {
            var pathData = new PathDataBase();
            var currentXmlFile = pathData.GetFileCurrentToolsXml();
            if (File.Exists(currentXmlFile))
            {
                logger.Information("Zrobienie VcProjecta ...");
                CreateVcproject.CreateFromXml(currentXmlFile, logger);

                logger.Information("Pakowanie ordera ...");
                var zipService = new ZipService();
                zipService.PackFile(currentXmlFile);

                logger.Information("Kopiowanie plikow ...");
                var transferService = new OrderTransferService();
                transferService.GetAllFileFromNcDir();
                transferService.GetAllFileFromOrder();

                transferService.GetAllFiles().ForEach(t => Console.WriteLine($"{t}"));

                transferService.CheckFilesIfExist(transferService.GetAllFiles());

                transferService.CopyAllFiles();

                logger.Information("Tworzenie Gita jesli nie istnieje...");
                var gitService = new GitService();
                gitService.CopyGITCommitfile();
                gitService.CopyGITInitfile();
                gitService.StartGitInitAsBat();
            }
            else
            {
                logger.Warning($"Brak pliku {currentXmlFile}. Program przerwany!");
                logger.Error($"Brak pliku {currentXmlFile}. Program przerwany!");
            }
        }
    }
}
