using BladeMill.BLL.Enums;
using BladeMill.BLL.SourceData;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Tworzenie VcProjecta z kodu NC
    /// </summary>
    public class NcVCProjectService
    {
        private ILogger _logger;
        private string _machine = "";
        private string _mainProgram = @"";
        private string _inputdata = @"SourceData\HSTM300_V0.VcProject";
        private string _outputdata = @"";
        private List<string> listsubprogramms = new List<string>(new string[] { }); //lista podprogramow z NC
        private PathDataBase _data = new PathDataBase();
        private ToolNcService _toolNcService = new ToolNcService();
        private XmlDocument _documentIn = new XmlDocument();
        private XmlDocument _documentOut = new XmlDocument();

        public NcVCProjectService(string mainProgram)
        {
            _logger = Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj} {Properties:j}{NewLine}{Exception}").MinimumLevel.Information()
            .WriteTo.File(@"C:\temp\NcVCProjectService.log")
            .CreateLogger();

            _mainProgram = mainProgram;

            NcMainProgramService _mainProgramService = new NcMainProgramService(_mainProgram);

            var machineFactory = new MachineServiceFactory();
            var ncFile = machineFactory.CreateMachine(TypeOfFile.ncFile);
            _machine = ncFile.GetMachine(mainProgram).MachineName;

            _inputdata = GetVcProjectTemplate();

            if (_inputdata != string.Empty)
            {
                if (File.Exists(_inputdata))
                    _documentIn.Load(_inputdata);
                _outputdata = Path.Combine(Path.GetDirectoryName(_mainProgram), Path.GetFileName(_mainProgram).Replace(".MPF","") + ".VcProject");

                CopyVcProjectTemplate();

                if (File.Exists(_outputdata))
                {
                    _documentOut.Load(_outputdata);
                }
                else
                {
                    _logger.Warning($"Brak pliku {_outputdata}");
                }
            }
        }
        public string GetVcProjectTemplate()
        {
            _inputdata = _data.GetFileVericutProjectTemplate(_machine);
            return _inputdata;
        }
        private void CopyVcProjectTemplate()
        {
            string nameTemplateVcProject = Path.GetFileName(_inputdata);
            string destTemplateVcProject = Path.Combine(_data.GetDirVericutProjectTemplate(), nameTemplateVcProject);
            if (File.Exists(destTemplateVcProject))
            {
                _logger.Debug($"Kopiowanie z {destTemplateVcProject} na {_outputdata}");
                File.Copy(destTemplateVcProject, _outputdata, true);
            }
            else
            {
                if (!File.Exists(destTemplateVcProject))
                    _logger.Error($"Uwaga! brak templata vc projecta {destTemplateVcProject}");
            }
        }
        public bool ReplaceNcProgramFile()
        {
            if (_mainProgram != null)
            {
                XmlNodeList ncProgramElement = _documentOut.GetElementsByTagName("NCProgram");
                for (int i = 0; i < ncProgramElement.Count; i++)
                {
                    var element = ncProgramElement[i].SelectNodes("File");
                    if (element == null || element.Count == 0) continue;
                    var firstElement = element.Item(0);
                    var docno = firstElement.InnerText;

                    if (_machine != "HURON")
                    {
                        int index = docno.IndexOf("922701.MPF");
                        if (index != -1)
                        {
                            firstElement.InnerText = Path.GetFileName(_mainProgram);
                            _logger.Debug($"Program glowny zostal podmieniony na {_mainProgram}");
                        }
                    }
                    else
                    {
                        int index = docno.IndexOf("huronconcateFile.NC");
                        if (index != -1)
                        {
                            firstElement.InnerText = Path.GetFileName(_mainProgram);
                            _logger.Debug($"Program glowny zostal podmieniony na {_mainProgram}");
                        }
                    }
                }
                if (_outputdata != "" && File.Exists(_outputdata))
                {
                    _documentOut.Save(_outputdata);
                    return true;
                }
            }
            return false;
        }
        public bool ReplaceSubroutines()
        {
            if (_mainProgram != null)
            {
                var toolsFromNc = _toolNcService.LoadToolsFromFile(_mainProgram);

                if (toolsFromNc.Count > 0)
                {
                    foreach (var item in toolsFromNc)
                    {
                        listsubprogramms.Add(item.BatchFile);
                    }
                }
                int l = 0;
                foreach (string item in listsubprogramms)
                {
                    l = l + 1;
                }
                int Count = _documentOut.GetElementsByTagName("Subroutine").Count;
                if (Count == l)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        _documentOut.GetElementsByTagName("Subroutine").Item(i).InnerText = Path.GetFileName(listsubprogramms[i]);
                        _logger.Debug($"Podmiana prodprogramu na {Path.GetFileName(listsubprogramms[i])}");
                    }
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                    {
                        int m = 0;
                        foreach (string item in listsubprogramms)
                        {
                            _documentOut.GetElementsByTagName("Subroutine").Item(m).InnerText = Path.GetFileName(listsubprogramms[m]);
                            _logger.Debug($"Podmiana prodprogramu na {Path.GetFileName(listsubprogramms[m])}");
                            m = m + 1;
                        }
                        _documentOut.GetElementsByTagName("Subroutine").Item(i).InnerText = "end";
                    }
                }
                //toolsFromNc.ForEach(t => Console.WriteLine($"Podmiana podprogramu {t.BatchFile}"));
                if (_outputdata != "" && File.Exists(_outputdata))
                {
                    _documentOut.Save(_outputdata);
                    return true;
                }                
            }
            return false;
        }
        public bool ReplaceControlFile()
        {
            int CountMachine = _documentOut.GetElementsByTagName("File").Count;
            for (int i = 0; i < CountMachine; i++)
            {
                if (_documentOut.GetElementsByTagName("File").Item(i).InnerText.Contains(".ctl"))
                {
                    _documentOut.GetElementsByTagName("File").Item(i).InnerText = _data.GetCtlFile(_machine);
                    _logger.Debug($"Control maszyny zostal podmieniony na {_data.GetCtlFile(_machine)}");
                }
            }
            if (_outputdata != "" && File.Exists(_outputdata))
            {
                _documentOut.Save(_outputdata);
                return true;
            }
            return false;
        }
        public bool ReplaceMachineFile()
        {
            int CountMachine = _documentOut.GetElementsByTagName("File").Count;
            for (int i = 0; i < CountMachine; i++)
            {
                if (_documentOut.GetElementsByTagName("File").Item(i).InnerText.Contains(".mch"))
                {
                    _documentOut.GetElementsByTagName("File").Item(i).InnerText = _data.GetMchFile(_machine);
                    _logger.Debug($"Control plik zostal podmieniony na {_data.GetMchFile(_machine)}");
                }
            }
            if (_outputdata != "" && File.Exists(_outputdata))
            {
                _documentOut.Save(_outputdata);
                return true;
            }
            return false;
        }
        public bool ReplaceToolLbrary()
        {
            int CountToolLibrary = _documentOut.GetElementsByTagName("Library").Count;
            for (int i = 0; i < CountToolLibrary; i++)
            {
                if (_documentOut.GetElementsByTagName("Library").Item(i).InnerText.Contains("BIBLIOTEKA"))
                {
                    _documentOut.GetElementsByTagName("Library").Item(i).InnerText = _data.GetFileVericutToolsLibrary();
                    _logger.Debug($"Tools library zostala podmieniona na {_data.GetFileVericutToolsLibrary()}");
                }
            }
            if (_outputdata != "" && File.Exists(_outputdata))
            {
                _documentOut.Save(_outputdata);
                return true;
            }                
            return false;
        }
        public bool RotationStl(string stlfile, string vector, int value)//TODO dodac Hurona i przerabiaczprogramu
        {
            XmlNodeList elemList = _documentOut.GetElementsByTagName("STL");
            XmlNodeList aNodes = _documentOut.GetElementsByTagName("Rotation");
            for (int i = 0; i < elemList.Count; i++)
            {
                var element = elemList[i].SelectNodes("File");
                if (element == null || element.Count == 0) continue;
                var firstElement = element.Item(0);
                var docno = firstElement.InnerText;
                if (_machine != "HURON")
                {
                    int index0 = docno.IndexOf(stlfile);
                    if (index0 != -1)
                    {
                        int m = 0;
                        foreach (XmlNode aNode in aNodes)
                        {
                            XmlAttribute idAttribute = aNode.Attributes[vector];
                            if (idAttribute != null)
                            {
                                if (m == 6)//stala wartosc
                                {
                                    string currentValue = idAttribute.Value;
                                    _logger.Debug($"Obrot detalu {stlfile} o {vector} {value}");
                                    idAttribute.Value = value.ToString();
                                }
                            }
                            m = m + 1;
                        }
                    }
                }
            }
            if (_outputdata != "" && File.Exists(_outputdata))
            {
                _documentOut.Save(_outputdata);
                return true;
            }
            return false;
        }
        public bool ReplaceGEOMETRY(string nameGeometry)
        {
            XmlNodeList ncProgramElement = _documentOut.GetElementsByTagName("STL");
            for (int i = 0; i < ncProgramElement.Count; i++)
            {
                var element = ncProgramElement[i].SelectNodes("File");
                if (element == null || element.Count == 0) continue;
                var firstElement = element.Item(0);
                var docno = firstElement.InnerText;

                if (_machine != "HURON")
                {
                    int index = docno.IndexOf(nameGeometry);
                    if (index != -1)
                    {
                        firstElement.InnerText = nameGeometry;
                        _logger.Debug($"Stl zostal podmieniony na {nameGeometry}");
                    }
                }
            }
            if (_outputdata != "" && File.Exists(_outputdata))
            {
                _documentOut.Save(_outputdata);
                return true;
            }
            return false;
        }
        /// <summary>
        /// replace stl file to new one
        /// </summary>
        /// <param name="nameGeometry"></param> searching name to replace
        /// <param name="newNameGeometry"></param> set new name
        public bool ReplaceGEOMETRY(string nameGeometry, string newNameGeometry)
        {
            XmlNodeList ncProgramElement = _documentOut.GetElementsByTagName("STL");
            for (int i = 0; i < ncProgramElement.Count; i++)
            {
                var element = ncProgramElement[i].SelectNodes("File");
                if (element == null || element.Count == 0) continue;
                var firstElement = element.Item(0);
                var docno = firstElement.InnerText;

                if (_machine != "HURON")
                {
                    int index = docno.IndexOf(nameGeometry);
                    if (index != -1)
                    {
                        firstElement.InnerText = newNameGeometry;
                        _logger.Debug($"Stl zostal podmieniony na {newNameGeometry}");
                    }
                }
            }
            if (_outputdata != "" && File.Exists(_outputdata))
            {
                _documentOut.Save(_outputdata);
                return true;
            }
            return false;
        }
        /// <summary>
        /// rotate stl file according machine type
        /// </summary>
        /// <param name="readvcproject"></param> 
        /// <param name="mainProgram"></param>
        /// <param name="adapter"></param> searching name to rotate
        public void RotateStl(NcVCProjectService readvcproject, string mainProgram, string adapter)
        {
            NcMainProgramService _mainProgramService = new NcMainProgramService(mainProgram);
            var machine = _mainProgramService.GetMachine();
            if (machine == MachineEnum.HSTM300.ToString() || machine == MachineEnum.HSTM500M.ToString())
            {
                readvcproject.RotationStl(adapter, "K", 180);
            }
            else if (machine == MachineEnum.HX151.ToString())
            {
                readvcproject.RotationStl(adapter, "I", 90);
            }
            else if (machine == MachineEnum.HSTM300HD.ToString())
            {
                readvcproject.RotationStl(adapter, "K", 180);
            }
            else
            {
                _logger.Debug("Rotate stl is not need for this machine");
            }
        }

        public void ReplacaAllClampingSystem()
        {
            var vcProjectTemplate = GetVcProjectTemplate();

            if (vcProjectTemplate.Contains("General"))//podmiana na oneDrive
            {
                var dirVericut = Path.Combine(@"C:\Users", Environment.UserName, @"General Electric International, Inc\Sekcja Technologiczna T1 - Clever\002_Vericut");
                var Grip_Claw1_a = "";
                var Grip_Claw2_a = "";
                var Grip_Lang_nr47085_a = "";
                var LANG_BASE_a = "";

                if (vcProjectTemplate.Contains(MachineEnum.HSTM300.ToString()) && !vcProjectTemplate.Contains("HD"))
                {
                    Grip_Claw1_a = Path.Combine(dirVericut, @"HSTM300\HSTM300_V7.2.3_GRIP\Grip_Claw1_a.stl");
                    Grip_Claw2_a = Path.Combine(dirVericut, @"HSTM300\HSTM300_V7.2.3_GRIP\Grip_Claw2_a.stl");
                    Grip_Lang_nr47085_a = Path.Combine(dirVericut, @"HSTM300\HSTM300_V7.2.3_GRIP\Grip_Lang_nr47085_a.stl");
                    LANG_BASE_a = Path.Combine(dirVericut, @"UCHWYTY\LANG\LANG_BASE_a.stl");
                }
                else if (vcProjectTemplate.Contains(MachineEnum.HSTM500.ToString()) && !vcProjectTemplate.Contains("HD"))
                {
                    Grip_Claw1_a = Path.Combine(dirVericut, @"HSTM500\HSTM500_V7.2.3\Grip_Claw1_a.stl");
                    Grip_Claw2_a = Path.Combine(dirVericut, @"HSTM500\HSTM500_V7.2.3\Grip_Claw2_a.stl");
                    Grip_Lang_nr47085_a = Path.Combine(dirVericut, @"HSTM500\HSTM500_V7.2.3\Grip_Lang_nr47085_a.stl");
                    LANG_BASE_a = Path.Combine(dirVericut, @"UCHWYTY\LANG\LANG_BASE_a.stl");
                }
                else if (vcProjectTemplate.Contains(MachineEnum.HX151.ToString()))
                {
                    Grip_Claw1_a = Path.Combine(dirVericut, @"HX151SI\HX_SIN_840D (Vericut 7.2.3) GRIP\Grip_Claw1_a.stl");
                    Grip_Claw2_a = Path.Combine(dirVericut, @"HX151SI\HX_SIN_840D (Vericut 7.2.3) GRIP\Grip_Claw2_a.stl");//TODO zmienic c male na C w templacie na S!!
                    Grip_Lang_nr47085_a = Path.Combine(dirVericut, @"HX151SI\HX_SIN_840D (Vericut 7.2.3) GRIP\Grip_Lang_nr47085_a.stl");
                    LANG_BASE_a = Path.Combine(dirVericut, @"UCHWYTY\LANG\LANG_BASE_a.stl");
                }
                else if (vcProjectTemplate.Contains(MachineEnum.HSTM1000.ToString()) || vcProjectTemplate.Contains(MachineEnum.HSTM1500.ToString()))
                {
                    Grip_Claw1_a = Path.Combine(dirVericut, @"HSTM1500_V8\Grip_Claw1_a.stl");
                    Grip_Claw2_a = Path.Combine(dirVericut, @"HSTM1500_V8\Grip_Claw2_a.stl");
                    Grip_Lang_nr47085_a = Path.Combine(dirVericut, @"HSTM1500_V8\Grip_Lang_nr47085_a.stl");
                    LANG_BASE_a = Path.Combine(dirVericut, @"UCHWYTY\LANG\LANG_BASE_a.stl");
                }
                else
                {
                    //nothing to fix
                }

                if (Grip_Claw1_a != "")
                {
                    ReplaceGEOMETRY("Grip_Claw1_a", Grip_Claw1_a);
                    ReplaceGEOMETRY("Grip_Claw2_a", Grip_Claw2_a);
                    ReplaceGEOMETRY("Grip_Lang_nr47085_a", Grip_Lang_nr47085_a);
                    ReplaceGEOMETRY("LANG_BASE_a", LANG_BASE_a);
                }
            }
        }

        public string GetVcProjectOutPutFile()
        {
            return _outputdata;
        }
    }
}
