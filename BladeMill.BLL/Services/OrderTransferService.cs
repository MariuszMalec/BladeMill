using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Przeslanie plikow do archiwzacji kodu NC
    /// </summary>
    public class OrderTransferService
    {
        private PathDataBase _pathData = new PathDataBase();
        private AppXmlConfService appService = new AppXmlConfService();
        private BMOrder _bMOrder;
        private string _bmOrderDestination;
        private FileService _ncService = new FileService();
        private List<string> _transferFiles;
        private ToolsXmlFile _toolsXmlFile = new ToolsXmlFile();//TODO do zastapienia prze ToolsXml
        private ToolsXml _toolsXml;
        private ILogger _logger;

        public OrderTransferService()
        {
            _logger = Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj} {Properties:j}{NewLine}{Exception}").MinimumLevel.Debug()
                .WriteTo.File(@"C:\temp\OrderTransferService.log")
                .CreateLogger();
            var currentToolsXml = _pathData.GetFileCurrentToolsXml();
            _toolsXmlFile = new ToolsXmlFile(currentToolsXml);//TODO do wyrzucenia
            _bMOrder = new BMOrder(_pathData.GetFileCurrentToolsXml());
            _bmOrderDestination = _pathData.GetDirOrders();
            _transferFiles = new List<string>();
            _toolsXml = new ToolsXml(currentToolsXml);
        }
        public List<string> GetAllFiles()
        {
            return _transferFiles;
        }
        public List<string> GetAllFileFromNcDir()
        {
            var machine = _toolsXmlFile.MACHINE;
            var mainProgram = _toolsXmlFile.PRGNUMBER;
            //var ncDir = appService.GetNcDir();

            var ncDir = Path.GetDirectoryName(_pathData.GetFileCurrentToolsXml());

            if (machine != "HURON")
            {
                _transferFiles.Add(Path.Combine(ncDir, mainProgram + "02.MPF"));
                mainProgram = Path.Combine(ncDir, mainProgram + "01.MPF");                
            }
            else { mainProgram = Path.Combine(ncDir, mainProgram + "01.NC"); }
            _transferFiles.Add(mainProgram);
            if (machine != "HURON")
            {
                var spfPrograms = _ncService.GetSubprogramsListFromNc(mainProgram);
                spfPrograms.ToList().DoAction(f => _transferFiles.Add(f.SubProgramNameWithDir));
            }            
            _transferFiles.Add(Path.Combine(ncDir, _bMOrder.OrderName + ".tools.xml"));
            _transferFiles.Add(Path.Combine(ncDir, _bMOrder.OrderName + ".xlsm"));
            if (_toolsXml.BMDTYPE == "RTBFixedBlade")
            {
                _transferFiles.Add(Path.Combine(ncDir, _toolsXmlFile.PRGNUMBER + "75.SPF"));
                _transferFiles.Add(Path.Combine(ncDir, _toolsXmlFile.PRGNUMBER + "85.SPF"));
            }
            if (_transferFiles.Count > 0)
            { return _transferFiles; }
            return new List<string>();
        }
        public List<string> GetAllFileFromOrder()
        {
            var orderName = _bMOrder.OrderName;
            var listExtFromOrder = new List<string>() { ".stl", ".VcProject" };
            if (Directory.Exists(appService.GetRootMfgDir()))
            {
                foreach (var ext in listExtFromOrder)
                {
                    _transferFiles.AddRange(GetFilesFromOrder(Path.Combine(appService.GetRootMfgDir(), orderName), orderName, ext));
                }
                _transferFiles.AddRange(GetFilesFromOrder(appService.GetRootMfgDir(), orderName, ".zip"));
                if (!_transferFiles.Any(t=>t.Contains(".stl")))
                {
                    _transferFiles.Add(Path.Combine(Path.Combine(appService.GetRootMfgDir(), orderName), "m_CLAMP_ADAPTER.stl"));
                    _transferFiles.Add(Path.Combine(Path.Combine(appService.GetRootMfgDir(), orderName), "m_CLAMPING_GEOMETRY.stl"));
                    _transferFiles.Add(Path.Combine(Path.Combine(appService.GetRootMfgDir(), orderName), "m_BladeBody.stl"));
                }
            }
            if (_transferFiles.Count > 0)
            {
                CheckStlFileIfExist("m_CLAMP_ADAPTER");
                CheckStlFileIfExist("m_CLAMPING_GEOMETRY");
                CheckStlFileIfExist("m_BladeBody");
                return _transferFiles;
            }
            return new List<string>();
        }
        private void CheckStlFileIfExist(string StlName)
        {
            var checkIfStlIsNull = _transferFiles.Find(t => t.Contains(StlName));
            if (checkIfStlIsNull == null)
            {
                _logger.Warning($"Brak pliki {StlName}! Uzyj programu z pythona do wygenerowania plikow");
            }
        }
        private List<string> GetFilesFromOrder(string dir, string ordername, string extention)
        {
            if (Directory.Exists(dir))
            {
                var fileFromOrder = new List<string>();
                string[] files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    if (file.Contains(ordername) && file.Contains(extention))
                    {
                        fileFromOrder.Add(file);
                    }
                }
                return fileFromOrder;
            }
            return new List<string>();
        }
        public List<string> CheckFilesIfExist(List<string> transferFiles)
        {
            if (transferFiles.Count > 0)
            {
                var lackOfPrograms = new List<string>();
                foreach (var item in transferFiles)
                {
                    if (!File.Exists(item))
                    {
                        lackOfPrograms.Add(item);
                        _logger.Warning($"Uwaga! brak pliku {item}");
                    }
                }
                return lackOfPrograms;
            }
            return new List<string>();
        }
        public void CopyAllFiles()
        {
            if (_transferFiles.Count > 0)
            {
                string order = _bMOrder.OrderNameDir;
                string tmpordername = Path.GetFileName(order);
                string tmpOrdernasieci = Path.Combine(_bmOrderDestination, tmpordername);
                if (!Directory.Exists(tmpOrdernasieci))
                    Directory.CreateDirectory(tmpOrdernasieci);
                if (Directory.Exists(tmpOrdernasieci))
                {
                    string[] files = Directory.GetFiles(tmpOrdernasieci);
                    foreach (string file in files)
                    {
                        string name = Path.GetFileName(file);
                        string dest = Path.Combine(tmpOrdernasieci, name);
                        if (File.Exists(file))
                        {
                            if (!file.Contains("_V0.VcProject"))
                            {
                                File.Delete(dest);
                            }
                        }
                    }
                }
                if (Directory.Exists(tmpOrdernasieci))
                {
                    ConsoleUtility.WriteProgressBar(0);
                    for (int count = 0; count < _transferFiles.Count; count++)
                    {
                        string file = _transferFiles[count];
                        string name = Path.GetFileName(file);
                        string dest = Path.Combine(tmpOrdernasieci, name);
                        if (File.Exists(file))
                        {
                            File.Copy(file, dest, true);
                            _logger.Debug($"Copy: {file} => {dest}");
                        }
                        ConsoleUtility.WriteProgressBar((count + 1) * 100 / _transferFiles.Count, true);
                        Thread.Sleep(10);
                    }
                    Console.WriteLine();
                }
            }
        }
        public void CopyOrder()
        {
            string order = _bMOrder.OrderNameDir;
            string tmpordername = Path.GetFileName(order);
            string tmpOrdernasieci = Path.Combine(_bmOrderDestination, tmpordername, tmpordername);
            Console.WriteLine($"Order na sieci : {tmpOrdernasieci}");
            Console.WriteLine($"Order na lokalu: {order}");
            if (order.Contains("C:"))
            {
                if (Directory.Exists(order))
                {
                    if (Directory.Exists(tmpOrdernasieci))
                    {
                        RemoveFolder(tmpOrdernasieci);
                        _logger.Debug($"Remove folder : {tmpOrdernasieci}");
                    }
                    if (!Directory.Exists(tmpOrdernasieci))
                    {
                        CopyFolder(order, tmpOrdernasieci);
                        _logger.Debug($"Copy to {tmpOrdernasieci}");
                    }
                }
                else
                {
                    _logger.Error("Program nie wykonal przeslania ordera na siec, order nie istnieje!");
                }
            }
            else
            {
                _logger.Error("Program nie wykonal przeslania ordera na siec, sprawdz wpisane dane!");
            }
        }
        private void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }
        private void RemoveFolder(string removeFolder)
        {
            string[] files = Directory.GetFiles(removeFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(removeFolder, name);
                File.SetAttributes(file, FileAttributes.Normal);
                RemoveFolderWithAttributeReadOnly(dest);
            }
            string[] folders0 = Directory.GetDirectories(removeFolder);
            foreach (string folder in folders0)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(removeFolder, name);
                //dopisane tutaj nowe z powodu katalogu git 
                string gitfolder = dest;
                string[] gitfolders = Directory.GetDirectories(gitfolder);
                foreach (string objectsfolder in gitfolders)
                {
                    string[] objects = Directory.GetDirectories(objectsfolder);
                    foreach (string obj in objects)
                    {
                        string[] objets = Directory.GetFiles(obj);
                        foreach (string file4 in objets)
                        {
                            name = Path.GetFileName(file4);
                            dest = Path.Combine(obj, name);
                            RemoveFolderWithAttributeReadOnly(dest);
                        }
                        Directory.Delete(obj, true);
                    }
                    Directory.Delete(objectsfolder, true);
                }
            }
            Directory.Delete(removeFolder, true);
        }
        private void RemoveFolderWithAttributeReadOnly(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes &= ~FileAttributes.ReadOnly;
                File.SetAttributes(path, attributes);
                File.Delete(path);
            }
        }
    }
}
