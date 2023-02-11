using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Tworznie VcProjecta z ...tool.xml
    /// </summary>
    public class XMLVCProjectService
    {
        private MachineServiceFactory _machineFactory = new MachineServiceFactory();
        private ToolNcService _toolNcService = new ToolNcService();
        private XmlDocument _documentIn = new XmlDocument();
        private XmlDocument _documentOut = new XmlDocument();
        private VcProjectData _vcProjectAllData;
        private ToolsXmlFile _toolsXmlFile = new ToolsXmlFile();
        private PathDataBase _data = new PathDataBase();
        private BMOrder _orderService;
        private string _order = "";
        private string _machine = "";
        private string _inputdata = @"SourceData\HSTM300_V0.VcProject";
        private string _outputdata = @"";
        private string _mainProgram = @"";
        private List<string> listsubprogramms = new List<string>(new string[] { }); //lista podprogramow z NC
        private ILogger _logger;
        public XMLVCProjectService(string toolsXmlFile)
        {
            _logger = Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj} {Properties:j}{NewLine}{Exception}").MinimumLevel.Information()
            .WriteTo.File(@"C:\temp\XMLVCProjectService.log")
            .CreateLogger();

            _toolsXmlFile = new ToolsXmlFile(toolsXmlFile);
            _mainProgram = _toolsXmlFile.GetMainProgramFileFromToolsXml(toolsXmlFile, _machine);
            _machine = _machineFactory.CreateMachine(TypeOfFile.toolsXmlFile).GetMachine(toolsXmlFile).MachineName;

            _inputdata = GetVcProjectTemplate();
            if (_inputdata != string.Empty)
            {
                if (File.Exists(_inputdata))
                    _documentIn.Load(_inputdata);
                _orderService = new BMOrder(_data.GetFileCurrentToolsXml());
                _order = _orderService.OrderNameDir;
                _outputdata = Path.Combine(_order, Path.GetFileName(_order) + ".VcProject");
                CopyVcProjectTemplate();
                if (File.Exists(_outputdata))
                {
                    _documentOut.Load(_outputdata);
                }
                else
                {
                    _logger.Warning($"Brak pliku w XMLVCProjectService {_outputdata}");
                }
            }
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
                        //firstElement.InnerText = "m_CLAMPING_GEOMETRY.stl";//wymiana detalu                                                
                        int m = 0;
                        foreach (XmlNode aNode in aNodes)
                        {
                            XmlAttribute idAttribute = aNode.Attributes[vector];
                            if (idAttribute != null)
                            {
                                //Console.WriteLine($"Vector {vector} {idAttribute.Value}");
                                if (m == 6)//stala wartosc
                                {
                                    string currentValue = idAttribute.Value;
                                    _logger.Information($"Obrot detalu {stlfile} o {vector} {value}");
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
        public bool ReplaceSubroutines()
        {
            if (_orderService != null)
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
        public bool ReplaceNcProgramFile()
        {
            if (_orderService != null)
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
        private void CopyVcProjectTemplate()
        {
            string nameTemplateVcProject = Path.GetFileName(_inputdata);
            string destTemplateVcProject = Path.Combine(_data.GetDirVericutProjectTemplate(), nameTemplateVcProject);
            if (File.Exists(destTemplateVcProject) && Directory.Exists(_order))
            {
                _logger.Debug($"Kopiowanie z {destTemplateVcProject} na {_outputdata}");
                File.Copy(destTemplateVcProject, _outputdata, true);
            }
            else
            {
                if (!File.Exists(destTemplateVcProject))
                    _logger.Error($"Uwaga! brak templata vc projecta {destTemplateVcProject}");
                if (!Directory.Exists(_order))
                    _logger.Error($"Uwaga! brak ordera {_order}");
            }
        }
        public string GetVcProjectTemplate()
        {
            _inputdata = _data.GetFileVericutProjectTemplate(_machine);
            return _inputdata;
        }
        public string GetVcProject()
        {
            _outputdata = Path.Combine(_order, Path.GetFileName(_order) + ".VcProject");
            return _outputdata;
        }
        public VcProjectData GetAllDataFromVcProject()
        {
            _vcProjectAllData = new VcProjectData()
            {
                ControlCtl = GetControlFile(),
                MachineMch = GetMachineFile(),
                ToolLibrary = GetToolLibrary(),
                NcProgram = GetNcProgramFile(),
                Subroutine = GetSubroutines(),
                StlGeometry = GetStls()
            }
            ;
            return _vcProjectAllData;
        }
        private List<string> GetElemByTagNameAndSelectNodes(string tageName, string selectNodes)
        {
            var tmp = new List<string>();
            XmlNodeList xmlNode = _documentIn.GetElementsByTagName(tageName);
            if (xmlNode.Count > 0)
            {
                for (int i = 0; i < xmlNode.Count; i++)
                {
                    var element = xmlNode[i].SelectNodes(selectNodes);
                    if (element == null || element.Count == 0) continue;
                    var firstElement = element.Item(0);
                    var docno = firstElement.InnerText;
                    if (docno != "end")
                        tmp.Add(docno);
                }
                return tmp;
            }
            return new List<string>() { "Brak" };
        }
        private List<string> GetStls()
        {
            return GetElemByTagNameAndSelectNodes("STL", "File");
        }
        private List<string> GetSubroutines()
        {
            return GetElemByTagNameAndSelectNodes("Subroutine", "File");
        }
        private string GetControlFile()
        {
            return GetElemByTagNameAndSelectNodes("Control", "File")[0].ToString();
        }
        private string GetMachineFile()
        {
            return GetElemByTagNameAndSelectNodes("Machine", "File")[0].ToString();
        }
        private string GetToolLibrary()
        {
            return GetElemByTagNameAndSelectNodes("ToolMan", "Library")[0].ToString();
        }
        private string GetNcProgramFile()
        {
            return GetElemByTagNameAndSelectNodes("NCProgram", "File")[0].ToString();
        }

        public void GetValue()//test nie dziala ze szkolenia! do wyrzucenia jak sie nie uda
        {
            var tmp = new List<string>();

            _inputdata = @"C:\Users\212517683\source\repos\BladeMill\BladeMill.BLL\SourceData\HSTM300_V0.VcProject";
            _logger.Information($"Czytam plik {_inputdata}");

            XElement root = XElement.Load(_inputdata);

            var sortedResults = root.Elements("VcProject")
                .Where(x => x.Attribute("Version").Value != null)
                .Select(x => new
                {
                    Version = x.Attribute("Version").Value,
                })
                ;

            foreach (var item in sortedResults)
            {
                Console.WriteLine($"{item.Version}");
            }

            Console.WriteLine($"{sortedResults.Select(x => x.Version).FirstOrDefault()}");
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

        public string GetVcProjectOutPutFile()
        {
            return _outputdata;
        }
    }
}
