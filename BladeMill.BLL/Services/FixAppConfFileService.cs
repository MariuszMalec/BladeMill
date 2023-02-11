using BladeMill.BLL.SourceData;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Generowanie pliki konfiguracji BM dla roznych dyskow
    /// </summary>
    public class FixAppConfFileService
    {
        private PathDataBase pathDataBase = new PathDataBase();
        private string appXmlFile;
        private ILogger _logger;
        public FixAppConfFileService()
        {
            _logger = Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj} {Properties:j}{NewLine}{Exception}").MinimumLevel.Debug()
                .WriteTo.File(@"C:\temp\FixAppConfFileService.log")
                .CreateLogger();
            appXmlFile = pathDataBase.GetApplicationConfFile();
        }
        public void CreateNewAppConfFile(string mainDir, string drive)
        {
            CreateLocalRootEngDir();
            CreateLocalRootMfgDir();
            CreateLocalTempNC();
            DeleteAppXmlFile(mainDir);
            CreateAppStructureXmlFile(mainDir);
            SetAppXmlFile(mainDir, drive);
        }
        private void CreateLocalTempNC()
        {
            var localTempNc = @"C:\tempNC";
            if (!Directory.Exists(localTempNc))
            {
                _logger.Information($"Od teraz katalog NcDir jest tutaj {localTempNc}");
                Directory.CreateDirectory(localTempNc);
            }
        }
        private void CreateLocalRootEngDir()
        {
            var localRootEngDir = @"C:\Clever\V300\BladeMill\data\RootEngDir";
            if (!Directory.Exists(localRootEngDir))
            {
                _logger.Information($"Od teraz katalog RootEngDir jest tutaj {localRootEngDir}");
                Directory.CreateDirectory(localRootEngDir);
            }
        }
        private void CreateLocalRootMfgDir()
        {
            var localRootMfgDir = @"C:\Clever\V300\BladeMill\data\RootMfgDir";
            if (!Directory.Exists(localRootMfgDir))
            {
                _logger.Information($"Od teraz katalog RootMfgDir jest tutaj {localRootMfgDir}");
                Directory.CreateDirectory(localRootMfgDir);
            }
        }
        private void SetAppXmlFile(string mainDir, string drive)
        {
            if (File.Exists(appXmlFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(appXmlFile);
                if (Directory.Exists(Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\BladeMillScripts")))
                {
                    if (drive != "C")
                    {
                        doc.SelectSingleNode("/server/directories/ENGINEERING_ORDER_DIR").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\data\RootEngDir");
                    }
                    else
                    {
                        doc.SelectSingleNode("/server/directories/ENGINEERING_ORDER_DIR").InnerText = Path.Combine(@"C:\", @"Clever\V300\BladeMill\data\RootEngDir");
                    }
                    doc.SelectSingleNode("/server/directories/MFG_ORDER_DIR").InnerText = Path.Combine(@"C:\", @"Clever\V300\BladeMill\data\RootMfgDir");
                    doc.SelectSingleNode("/server/directories/TEMP").InnerText = Path.Combine(@"C:\Users", Environment.UserName, @"BladeMill", pathDataBase.GetBMVersion(), @"Temp");
                    doc.SelectSingleNode("/server/directories/INSTALL_DIR").InnerText = Path.Combine(System.IO.Path.Combine(@"C:\", @"Clever", pathDataBase.GetBMVersion(), @"BladeMill"));
                    doc.SelectSingleNode("/server/directories/NC_DIR").InnerText = @"C:\tempNC";
                    doc.SelectSingleNode("/server/directories/SCRIPTS_DIR").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\BladeMillScripts");
                    doc.SelectSingleNode("/server/directories/REPRESENTATION_DIR").InnerText = System.IO.Path.Combine(@"C:\Users", Environment.UserName, @"AppData\Roaming\BladeMill", pathDataBase.GetBMVersion(), @"Representation");
                    //
                    doc.SelectSingleNode("/server/files/TOOL_LIST").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\AppData", @"Tools_elb_V300.cle");
                    doc.SelectSingleNode("/server/files/MACHINE_LIST").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\AppData", @"Machines_elb_V330.cle");
                    doc.SelectSingleNode("/server/files/APP_RET_LIST").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\AppData", @"AppRets_elb_V300.cle");
                    doc.SelectSingleNode("/server/files/MFG_PROCESS_LIST").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\AppData", @"Default_MfgProcesses.cle");
                    doc.SelectSingleNode("/server/files/TECHNOLOGY_LIST").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\AppData", @"Technologies_elb_V300.cle");
                    doc.SelectSingleNode("/server/files/MEASURINGLAW_LIST").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\AppData", @"MeasuringLaws_elb.cle");
                    doc.SelectSingleNode("/server/files/AUXCOMMAND_LIST").InnerText = Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\AppData", @"AuxiliaryCommands_elb.xml");
                    //
                    doc.SelectSingleNode("/server/com/APP_SERVER_IP").InnerText = "127.0.0.1";
                    doc.SelectSingleNode("/server/com/APP_SERVER_PORT").InnerText = "60000";
                    doc.SelectSingleNode("/server/com/CAD_PLUGIN_IP").InnerText = "127.0.0.1";
                    doc.SelectSingleNode("/server/com/CAD_PLUGIN_PORT").InnerText = "60007";
                    doc.SelectSingleNode("/server/com/LOG_ACCURACY").InnerText = "2";
                    doc.SelectSingleNode("/server/com/BACKUP_ORDER").InnerText = "0";
                    doc.SelectSingleNode("/server/com/DATAUNIT").InnerText = "Metric";

                    doc.Save(appXmlFile);
                    _logger.Debug($"Wypelnienie pliku {appXmlFile} danymi");

                    FixEncodingAppXmlFile();
                }
                else
                {
                    _logger.Warning($"Nie poprawiono pliku ! {appXmlFile}");
                    Console.WriteLine("Press any key to finish");
                    Console.ReadKey();
                }
            }
        }

        private void FixEncodingAppXmlFile()
        {
            //naprawa Applicationconffile brak konca lini to nizej poprawia
            string line;
            var lines = new List<string>(new string[] { });
            lines.Clear();
            StreamReader file = new StreamReader(appXmlFile);
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }
            file.Close();
            File.WriteAllLines(appXmlFile, lines);
        }

        private void DeleteAppXmlFile(string mainDir)
        {
            if (Directory.Exists(Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\BladeMillScripts")))
            {
                if (File.Exists(appXmlFile))
                {
                    File.Delete(appXmlFile);
                    _logger.Debug($"Wykasowano plik {appXmlFile}");
                }
            }
        }
        private void CreateAppStructureXmlFile(string mainDir)
        {
            if (Directory.Exists(Path.Combine(mainDir, @"Clever\V300\BladeMill\BladeMillServer\BladeMillScripts")))
            {
                XElement server =
                new XElement("server",
                    new XElement("directories",
                        new XElement("ENGINEERING_ORDER_DIR", ""),
                        new XElement("MFG_ORDER_DIR", ""),
                        new XElement("TEMP", ""),
                        new XElement("INSTALL_DIR", ""),
                        new XElement("NC_DIR", ""),
                        new XElement("SCRIPTS_DIR", ""),
                        new XElement("REPRESENTATION_DIR", "")
                    ),

                    new XElement("files",
                        new XElement("TOOL_LIST", ""),
                        new XElement("MACHINE_LIST", ""),
                        new XElement("APP_RET_LIST", ""),
                        new XElement("MFG_PROCESS_LIST", ""),
                        new XElement("TECHNOLOGY_LIST", ""),
                        new XElement("MEASURINGLAW_LIST", ""),
                        new XElement("AUXCOMMAND_LIST", ""),
                        new XElement("SUPPORT_SITE", "")
                    ),

                    new XElement("com",
                        new XElement("APP_SERVER_IP", ""),
                        new XElement("APP_SERVER_PORT", ""),
                        new XElement("CAD_PLUGIN_IP", ""),
                        new XElement("CAD_PLUGIN_PORT", ""),
                        new XElement("LOG_ACCURACY", ""),
                        new XElement("BACKUP_ORDER", ""),
                        new XElement("DATAUNIT", "")
                    )
                );
                server.Save(appXmlFile);
                _logger.Information($"Utworzenie pliku {appXmlFile}");
            }
        }
    }
}
