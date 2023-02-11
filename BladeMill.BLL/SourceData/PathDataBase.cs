using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMill.BLL.SourceData
{
    /// <summary>
    /// Podstawowe pliki i katalogi 
    /// i ustawienia domyslne
    /// </summary>
    public class PathDataBase
    {
        string DirOneDriveClever = Path.Combine(@"C:\Users", Environment.UserName, @"General Electric International, Inc\Sekcja Technologiczna T1 - Clever");
        string DirOrders = Path.Combine(@"T1-PROGRAMOWANIE NC\03_PROGRAMY NC\ORDERY");//S and C
        string DirVericutProjectTemplate = @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\VericutMachines_Templates";
        string FileVericutToolsLibrary = @"Vericut\BIBLIOTEKA NARZEDZI\GE_V9.0.tls";
        string DirProgramExe = @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\task";
        string DirBladeMillScripts = @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts";
        string DirDrive = @"S:\SHARES\CLEVER";
        string DirCmm = @"T1-PROGRAMOWANIE NC\04_POMIAROWKA\B&S";
        string DirTask = (@"Process\task");
        string DirHtml = @"Process\HTML";
        string DirIcon = @"Process\ICO";
        string FileExcelTemplate = @"Process\EXCEL_Templates\Nowa Lista Narzedziowa.xlsm";
        string GITInitfile = (@"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIT\GitInitCmd.bat");
        string GITCommitfile = (@"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIT\GitCommitCmd.bat");
        string GITAddfile = (@"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIT\add.py");
        string FileCurrentToolsXml = @"C:\tempNC\current.xml";
        string[] CtlDirs;
        string[] MchDirs;
        string FileVericutProjectTemplate = @"";
        private ILogger _logger;
        private string DirMainProgramTemplate = @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\NC_Templates";
        string FileMainProgramTemplate = @"";

        public PathDataBase()
        {
            _logger = Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj} {Properties:j}{NewLine}{Exception}").MinimumLevel.Debug()
            .WriteTo.File(@"C:\temp\PathDataBase.log")
            .CreateLogger();
        }

        public string GetFileMainProgramTemplate(string machine)
        {
            if (machine == MachineEnum.HSTM300.ToString()) { FileMainProgramTemplate = Path.Combine(GetDirMainProgramTemplate(), "PR_HSTM30001.MPF"); }
            else if (machine == MachineEnum.HSTM300HD.ToString()) { FileMainProgramTemplate = Path.Combine(GetDirMainProgramTemplate(), "PR_HSTM300HD.MPF"); }
            else if (machine == MachineEnum.HSTM500.ToString()) { FileMainProgramTemplate = Path.Combine(GetDirMainProgramTemplate(), "PR_HSTM500HD02.MPF"); }
            else if (machine == MachineEnum.HSTM1000.ToString()) { FileMainProgramTemplate = Path.Combine(GetDirMainProgramTemplate(), "PR_HSTM1000.MPF"); }
            else if (machine == MachineEnum.HSTM1500.ToString()) { FileMainProgramTemplate = Path.Combine(GetDirMainProgramTemplate(), "PR_HSTM1000.MPF"); }
            else if (machine == MachineEnum.HX151.ToString()) { FileMainProgramTemplate = Path.Combine(GetDirMainProgramTemplate(), "PR_HX151.MPF"); }
            else if (machine == MachineEnum.HSTM500M.ToString()) { FileMainProgramTemplate = Path.Combine(GetDirMainProgramTemplate(), "PR_HSTM500M.MPF"); }
            else { FileMainProgramTemplate = string.Empty; }
            return FileMainProgramTemplate;
        }

        public string GetDirMainProgramTemplate()
        {
            if (!Directory.Exists(Path.Combine(DirDrive, DirMainProgramTemplate)))
            {
                var folder = Path.Combine(DirDrive, DirMainProgramTemplate);
                _logger.Warning($"Uwaga! brak katalogu {folder}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), DirMainProgramTemplate);
            }
            return Path.Combine(DirDrive, DirMainProgramTemplate);
        }

        public IEnumerable<string> GetAllVcProjectTemplateFiles()
        {
            var allVcProjectNames = GetAllVcProjectTemplateAsEnum();
            var allVcProjectTemplatesFiles = new List<string>();
            var dirTemplates = GetDirVericutProjectTemplate();
            foreach (var item in allVcProjectNames)
            {
                var file = Path.Combine(Path.Combine(dirTemplates, item + ".VcProject"));
                allVcProjectTemplatesFiles.Add(file);
            }
            return allVcProjectTemplatesFiles;
        }

        private IEnumerable<VcProjectTemplateEnum> GetAllVcProjectTemplateAsEnum()
        {

            var enumList = new List<VcProjectTemplateEnum>();
            var enumMemberCount = Enum.GetNames(typeof(VcProjectTemplateEnum)).Length;
            if (enumMemberCount > 0)
            {
                enumList = Enum.GetValues(typeof(VcProjectTemplateEnum)).OfType<VcProjectTemplateEnum>().ToList();
                return enumList;
            }
            return new List<VcProjectTemplateEnum>();
        }

        public string GetDirAutomationScriptLauncher()
        {
            return Path.Combine(@"C:\Clever", GetBMVersion(), @"AutomationScriptLauncher");
        }
        public string GetDirOrdersLocalDisk()//only to test
        {
            return Path.Combine(@"C:", @"T1-PROGRAMOWANIE NC\03_PROGRAMY NC\ORDERY");
        }
        public string GetCleverHome()
        {
            return GetEnvironmentVariable("CLEVERHOME");
        }
        public List<string> GetListMachineEnem()
        {
            var machineList = new List<string>() { };
            var enumList = new List<MachineEnum>();
            var enumMemberCount = Enum.GetNames(typeof(MachineEnum)).Length;
            if (enumMemberCount > 0)
            {
                enumList = Enum.GetValues(typeof(MachineEnum)).OfType<MachineEnum>().ToList();
            }
            foreach (var item in enumList)
            {
                machineList.Add(item.ToString());
            }
            return machineList;
        }
        public string GetFileVericutProjectTemplate(string machine)
        {
            if (machine == MachineEnum.HSTM300.ToString()) { FileVericutProjectTemplate = Path.Combine(GetDirVericutProjectTemplate(), "HSTM300_V0.VcProject"); }
            else if (machine == MachineEnum.HSTM300HD.ToString()) { FileVericutProjectTemplate = Path.Combine(GetDirVericutProjectTemplate(), "HSTM300_V0.VcProject"); }
            else if (machine == MachineEnum.HSTM500.ToString()) { FileVericutProjectTemplate = Path.Combine(GetDirVericutProjectTemplate(), "HSTM500_V1.VcProject"); }
            else if (machine == MachineEnum.HSTM1000.ToString()) { FileVericutProjectTemplate = Path.Combine(GetDirVericutProjectTemplate(), "HSTM1500_V0.VcProject"); }
            else if (machine == MachineEnum.HSTM1500.ToString()) { FileVericutProjectTemplate = Path.Combine(GetDirVericutProjectTemplate(), "HSTM1500_V0.VcProject"); }
            else if (machine == MachineEnum.HX151.ToString()) { FileVericutProjectTemplate = Path.Combine(GetDirVericutProjectTemplate(), "HX151_V1.VcProject"); }
            else if (machine == MachineEnum.HURON.ToString()) { FileVericutProjectTemplate = Path.Combine(GetDirVericutProjectTemplate(), "HURON_V1.VcProject"); }
            else if (machine == MachineEnum.HSTM500M.ToString()) { FileVericutProjectTemplate = Path.Combine(GetDirVericutProjectTemplate(), "HSTM300_V0.VcProject"); }
            else { FileVericutProjectTemplate = string.Empty; }
            return FileVericutProjectTemplate;
        }
        public string GetMchFile(string machine)
        {
            string mchFile = string.Empty;
            if (Directory.Exists(DirDrive))
            {
                if (machine == MachineEnum.HSTM300.ToString() || machine == MachineEnum.HSTM500M.ToString())
                {
                    MchDirs = new string[] { GetDirDrive(), "Vericut", "HSTM300_V9", "hstm300-2new2.mch" };
                }
                else if (machine == MachineEnum.HSTM500.ToString())
                {
                    MchDirs = new string[] { GetDirDrive(), "Vericut", "HSTM500", "HSTM500_V7.2.3 (C-Achs-hydraulic-Adapter)", "HSTM500-HD-C-Achs-Adapter.mch" };
                }
                else if (machine == MachineEnum.HX151.ToString())
                {
                    MchDirs = new string[] { GetDirDrive(), "Vericut", "HX151SI_V9", "hx-1.mch" };
                }
                else if (machine == MachineEnum.HURON.ToString())
                {
                    MchDirs = new string[] { GetDirDrive(), "Vericut", "HURON_V9", "HURON_20140710.mch" };
                }
                else if (machine == MachineEnum.HSTM1000.ToString())
                {
                    MchDirs = new string[] { GetDirDrive(), "Vericut", "HSTM1500_V8", "hstm1500_V0.mch" };
                }
                else if (machine == MachineEnum.HSTM1500.ToString())
                {
                    MchDirs = new string[] { GetDirDrive(), "Vericut", "HSTM1500_V8", "hstm1500_V0.mch" };
                }
                else if (machine == MachineEnum.HSTM300HD.ToString())
                {
                    MchDirs = new string[] { GetDirDrive(), "Vericut", "HSTM300_V9", "hstm300-2new2.mch" };
                }
                else
                {
                    _logger.Error($"Uwaga! brak maszyny dla vericuta");
                    //Console.WriteLine($"Uwaga! brak maszyny dla vericuta");
                    return string.Empty;
                }
            }
            else
            {
                if (machine == MachineEnum.HSTM300.ToString() || machine == MachineEnum.HSTM500M.ToString())
                {
                    MchDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM300_V9", "hstm300-2new2.mch" };
                }
                else if (machine == MachineEnum.HSTM500.ToString())
                {
                    MchDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM500", "HSTM500_V7.2.3 (C-Achs-hydraulic-Adapter)", "HSTM500-HD-C-Achs-Adapter.mch" };
                }
                else if (machine == MachineEnum.HX151.ToString())
                {
                    MchDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HX151SI_V9" , "hx-1.mch" };
                }
                else if (machine == MachineEnum.HURON.ToString())
                {
                    MchDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HURON_V9", "HURON_20140710.mch" };
                }
                else if (machine == MachineEnum.HSTM1000.ToString())
                {
                    MchDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM1500_V8", "hstm1500_V0.mch" };
                }
                else if (machine == MachineEnum.HSTM1500.ToString())
                {
                    MchDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM1500_V8", "hstm1500_V0.mch" };
                }
                else if (machine == MachineEnum.HSTM300HD.ToString())
                {
                    MchDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM300_V9", "hstm300-2new2.mch" };
                }
                else
                {
                    _logger.Error($"Uwaga! brak maszyny dla vericuta");
                    //Console.WriteLine($"Uwaga! brak maszyny dla vericuta");
                    return string.Empty;
                }
            }
            if (MchDirs != null)
            {
                mchFile = Path.Combine(MchDirs);
            }
            return mchFile;
        }
        public string GetCtlFile(string machine)
        {
            string ctlFile = string.Empty;
            CtlDirs = null;
            if (Directory.Exists(DirDrive))
            {
                if (machine == MachineEnum.HSTM300.ToString() || machine == MachineEnum.HSTM500M.ToString())
                {
                    CtlDirs = new string[] { GetDirDrive(), "Vericut", "HSTM300", "HSTM300_V7.2.3_GRIP", "sin840d_KO.ctl" };
                }
                else if (machine == MachineEnum.HSTM500.ToString())
                {
                    CtlDirs = new string[] { GetDirDrive(), "Vericut", "HSTM500", "HSTM500_V7.2.3 (C-Achs-hydraulic-Adapter)", "840D.ctl" };
                }
                else if (machine == MachineEnum.HX151.ToString())
                {
                    CtlDirs = new string[] { GetDirDrive(), "Vericut", "HX151SI", "HX_SIN_840D (Vericut 7.2.3) GRIP", "sin840d_KO.ctl" };
                }
                else if (machine == MachineEnum.HURON.ToString())
                {
                    CtlDirs = new string[] { GetDirDrive(), "Vericut", "HURON_V9", "840D_20140710.ctl" };
                }
                else if (machine == MachineEnum.HSTM1000.ToString())
                {
                    CtlDirs = new string[] { GetDirDrive(), "Vericut", "HSTM1500_V8", "840D_V0.ctl" };
                }
                else if (machine == MachineEnum.HSTM1500.ToString())
                {
                    CtlDirs = new string[] { GetDirDrive(), "Vericut", "HSTM1500_V8", "840D_V0.ctl" };
                }
                else if (machine == MachineEnum.HSTM300HD.ToString())
                {
                    CtlDirs = new string[] { GetDirDrive(), "Vericut", "HSTM300", "HSTM300_V7.2.3_GRIP", "sin840d_KO.ctl" };
                }
                else
                {
                    _logger.Error($"Uwaga! brak kontrolera maszyny dla vericuta");
                    //Console.WriteLine($"Uwaga! brak kontrolera maszyny dla vericuta");
                    return string.Empty;
                }
            }
            else
            {
                if (machine == MachineEnum.HSTM300.ToString() || machine == MachineEnum.HSTM500M.ToString())
                {
                    CtlDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM300", "HSTM300_V7.2.3_GRIP", "sin840d_KO.ctl" };
                }
                else if (machine == MachineEnum.HSTM500.ToString())
                {
                    CtlDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM500", "HSTM500_V7.2.3 (C-Achs-hydraulic-Adapter)", "840D.ctl" };
                }
                else if (machine == MachineEnum.HX151.ToString())
                {
                    CtlDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HX151SI", "HX_SIN_840D (Vericut 7.2.3) GRIP", "sin840d_KO.ctl" };
                }
                else if (machine == MachineEnum.HURON.ToString())
                {
                    CtlDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HURON_V9", "840D_20140710.ctl" };
                }
                else if (machine == MachineEnum.HSTM1000.ToString())
                {
                    CtlDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM1500_V8", "840D_V0.ctl" };
                }
                else if (machine == MachineEnum.HSTM1500.ToString())
                {
                    CtlDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM1500_V8", "840D_V0.ctl" };
                }
                else if (machine == MachineEnum.HSTM300HD.ToString())
                {
                    CtlDirs = new string[] { GetDirOneDriveClever(), "002_Vericut", "HSTM300", "HSTM300_V7.2.3_GRIP", "sin840d_KO.ctl" };
                }
                else
                {
                    _logger.Error($"Uwaga! brak kontrolera maszyny dla vericuta");
                    //Console.WriteLine($"Uwaga! brak kontrolera maszyny dla vericuta");
                    return string.Empty;
                }
            }
            if (CtlDirs != null)
            {
                ctlFile = Path.Combine(CtlDirs);
            }
            return ctlFile;
        }
        public string GetFileCurrentToolsXml()
        {
            if (!File.Exists(FileCurrentToolsXml))
            {
                _logger.Error($"Brak pliku {FileCurrentToolsXml}, niektore aplikacje nie beda dzialac prawidlowo");
                FileCurrentToolsXml = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "current.xml");
                return FileCurrentToolsXml;
            }
            return FileCurrentToolsXml;
        }
        public string GetGITInitfile()
        {
            if (!File.Exists(Path.Combine(DirDrive, GITInitfile)))
            {
                var file = Path.Combine(DirDrive, GITInitfile);
                _logger.Warning($"Uwaga! brak pliku {file}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), GITInitfile);
            }
            return Path.Combine(DirDrive, GITInitfile);
        }
        public string GetGITCommitfile()
        {
            if (!File.Exists(Path.Combine(DirDrive, GITCommitfile)))
            {
                var file = Path.Combine(DirDrive, GITCommitfile);
                _logger.Warning($"Uwaga! brak pliku {file}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), GITCommitfile);
            }
            return Path.Combine(DirDrive, GITCommitfile);
        }
        public string GetGITAddfile()
        {
            if (!File.Exists(Path.Combine(DirDrive, GITAddfile)))
            {
                var file = Path.Combine(DirDrive, GITAddfile);
                _logger.Warning($"Uwaga! brak pliku {file}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), GITAddfile);
            }
            return Path.Combine(DirDrive, GITAddfile);
        }
        public string GetDirCmm()
        {
            var folder = Path.Combine(DirDrive, DirCmm);
            if (!Directory.Exists(folder))
            {
                _logger.Error($"Uwaga! brak katalogu {folder}");
                return Path.Combine(GetDirOneDriveClever(), DirCmm);
            }
            return Path.Combine(DirDrive, DirCmm);
        }
        public string GetDirDrive()
        {
            if (!Directory.Exists(DirDrive))
            {
                _logger.Warning($"Uwaga! brak katalogu {DirDrive}, zmiana na onedrive");
                return GetDirOneDriveClever();
            }
            return DirDrive;
        }
        public string GetFileExcelTemplate()
        {
            var template = Path.Combine(GetDirBladeMillScripts(), FileExcelTemplate);
            if (!File.Exists(template))
            {
                _logger.Warning($"Uwaga! brak pliku {template}!");
            }
            _logger.Debug(template);
            return template;
        }
        public string GetDirOneDriveClever()
        {
            if (!Directory.Exists(DirOneDriveClever))
            {
                _logger.Error($"Uwaga! brak katalogu {DirOneDriveClever}, zglos sie do Mariusza");
            }
            return DirOneDriveClever;
        }
        public string GetDirProgramExe()
        {
            if (!Directory.Exists(Path.Combine(DirDrive, DirProgramExe)))
            {
                _logger.Warning($"Uwaga! brak katalogu {DirProgramExe}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), DirProgramExe);
            }
            return Path.Combine(DirDrive, DirProgramExe);
        }
        public string GetFileVericutToolsLibrary()
        {
            if (!File.Exists(Path.Combine(DirDrive, FileVericutToolsLibrary)))
            {
                _logger.Warning($"Uwaga! brak pliku {FileVericutToolsLibrary}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), FileVericutToolsLibrary.Replace("Vericut", "002_Vericut"));
            }
            return Path.Combine(DirDrive, FileVericutToolsLibrary);
        }
        public string GetDirVericutProjectTemplate()
        {
            if (!Directory.Exists(Path.Combine(DirDrive, DirVericutProjectTemplate)))
            {
                var folder = Path.Combine(DirDrive, DirVericutProjectTemplate);
                _logger.Warning($"Uwaga! brak katalogu {folder}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), DirVericutProjectTemplate);
            }
            return Path.Combine(DirDrive, DirVericutProjectTemplate);
        }
        public string GetDirOrders()
        {
            if (!Directory.Exists(Path.Combine(DirDrive, DirOrders)))
            {
                _logger.Warning($"Uwaga! brak katalogu {DirOrders}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), @"003_ORDERY");
            }
            return Path.Combine(DirDrive, DirOrders);
        }
        public string GetDirBladeMillScripts()
        {
            if (!Directory.Exists(Path.Combine(DirDrive, DirBladeMillScripts)))
            {
                _logger.Warning($"Uwaga! brak katalogu {DirBladeMillScripts}, zmiana na onedrive");
                return Path.Combine(GetDirOneDriveClever(), DirBladeMillScripts);
            }
            return Path.Combine(DirDrive, DirBladeMillScripts);
        }
        public string GetDirTask()
        {
            return Path.Combine(GetDirBladeMillScripts(), DirTask);
        }
        public string GetDirHtml()
        {
            return Path.Combine(GetDirBladeMillScripts(), DirHtml);
        }
        public string GetDirIcon()
        {
            return Path.Combine(GetDirBladeMillScripts(), DirIcon);
        }
        public IEnumerable<BasePaths> SetDirs()
        {
            var _datas = new BasePaths
            {
                DirOneDriveClever = GetDirOneDriveClever(),
                DirOrders = GetDirOrders(),
                DirVericutProjectTemplate = GetDirVericutProjectTemplate(),
                FileVericutToolsLibrary = GetFileVericutToolsLibrary(),
                DirProgramExe = GetDirProgramExe(),
                DirBladeMillScripts = GetDirBladeMillScripts(),
                DirDrive = GetDirDrive(),
                DirCmm = GetDirCmm(),
                DirTask = GetDirTask(),
                DirHtml = GetDirHtml(),
                DirIcon = GetDirIcon(),
                FileExcelTemplate = GetFileExcelTemplate()
            };
            return new[] { _datas };
        }
        public string GetApplicationConfFile()
        {
            var appConfFile = Path.Combine(@"C:\Users", Environment.UserName, @"AppData\Roaming\BladeMill", GetBMVersion(), @"ConfigServer\Application.xml.conf");
            if (!File.Exists(appConfFile))
            {
                _logger.Error($"Uwaga! brak pliku {appConfFile}!!!");
            }
            return Path.Combine(Path.Combine(@"C:\Users", Environment.UserName, @"AppData\Roaming\BladeMill", GetBMVersion(), @"ConfigServer\Application.xml.conf"));
        }
        public string GetBMVersion()
        {
            string cleverhome = GetEnvironmentVariable("CLEVERHOME");
            if (cleverhome.Contains("V300"))
                return "3.20";
            string BMVersion = Path.GetFileName(cleverhome);
            return BMVersion;
        }
        private string GetEnvironmentVariable(string Variable)
        {
            Environment.CurrentDirectory = Environment.GetEnvironmentVariable(Variable);
            DirectoryInfo info = new DirectoryInfo(".");
            string envvariable = info.FullName;
            return envvariable;
        }

        public string GetNxDir()
        {
            return Path.Combine(@"C:\Clever", GetBMVersion(), @"NX19-0RemoteListener\environment");
        }
    }
}
