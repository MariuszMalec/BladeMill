using BladeMill.BLL.Entities;
using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Lista bledow dla danego programu NC
    /// </summary>
    public class NcCodeCheckService
    {
        private FileService fileService = new FileService();
        private ToolNcService ncService = new ToolNcService();
        public string Machine;
        public string MainProgram;
        private ILogger _logger;
        private List<NcCodeCheck> _errors = new List<NcCodeCheck>() { };
        private PathDataBase _pathService;
        private ToolsXmlFile _toolsXmlFile = new ToolsXmlFile();
        private AppXmlConfDirectories _appXmlConfDirectory = new AppXmlConfDirectories();
        private IEnumerable<SubProgram> _listsuprogrammsFromNc = new List<SubProgram>();
        private readonly MachineServiceFactory _machineServiceFactory;
        private MachineSettings _machineSettings;

        public NcCodeCheckService()
        {
            _logger = Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj} {Properties:j}{NewLine}{Exception}").MinimumLevel.Information()
                .WriteTo.File(@"C:\temp\NcCodeCheckService.log")
                .CreateLogger();
            _pathService = new PathDataBase();
            MainProgram = _toolsXmlFile.GetMainProgramFileFromCurrentToolsXml();            
            _machineServiceFactory = new MachineServiceFactory();
            _machineSettings = _machineServiceFactory.CreateMachine(TypeOfFile.ncFile);

        }

        public async Task<List<NcCodeCheck>> FindErrorsInNcCode(string mainProgram)
        {
            if (File.Exists(mainProgram))
            {
                if (IsMainProgram(mainProgram))
                {
                    var listsuprogramms = fileService.GetSubprogramsListFromNc(mainProgram).ToList();

                    var machineServiceFactory = new MachineServiceFactory();
                    Machine = machineServiceFactory.CreateMachine(TypeOfFile.ncFile).GetMachine(mainProgram).MachineName;

                    //Check subprogramms
                    await Task.Run(() =>
                    {
                        ConsoleUtility.WriteProgressBar(0);
                        for (int i = 0; i < listsuprogramms.Count; i++)
                        {
                            if (File.Exists(listsuprogramms[i].SubProgramNameWithDir))
                            {
                                GetErrorsForAll(listsuprogramms[i].SubProgramNameWithDir, i);
                                GetErrorsAccMachine(listsuprogramms[i].SubProgramNameWithDir, Machine, i);
                            }
                            else
                            {
                                _errors.Add(new NcCodeCheck(0, mainProgram, "Check file", new List<string> { $"brak podprogramu! {listsuprogramms[i].SubProgramNameWithDir}" }));
                                _logger.Warning($"Brak podprogramu! {listsuprogramms[i].SubProgramNameWithDir}");
                            }
                            ConsoleUtility.WriteProgressBar((i + 1) * 100 / listsuprogramms.Count, true);
                            Thread.Sleep(1);
                        }
                    });
                    //Check main program
                    if ((Machine.Contains(MachineEnum.HSTM500.ToString()) && !Machine.Contains(MachineEnum.HSTM500M.ToString())) ||
                        Machine.Contains(MachineEnum.HSTM300HD.ToString()) ||
                        Machine.Contains(MachineEnum.HX151.ToString()))
                    {
                        //trzeba sprawdzic czy wszystkie programy sa
                        var checkFileExist = listsuprogramms.All(p => p.IsSubProgram == true);
                        if (checkFileExist)
                        {
                            _errors.Add(new NcCodeCheck(1, mainProgram, CheckingNcOperationEnum.Check_preload.ToString(), CheckPreload(mainProgram)));
                        }
                        else
                        {
                            _logger.Warning($"Brak podprogramu. Nie sprawdzono przygotowania narzedzi!");
                        }
                    }
                    if (Machine.Contains(MachineEnum.AVIA.ToString()))
                    {
                        _errors.Add(new NcCodeCheck(1, mainProgram, CheckingNcOperationEnum.Check_M30.ToString(), HaveToBeInNcCode(mainProgram,"M30")));
                        _errors.Add(new NcCodeCheck(2, mainProgram, CheckingNcOperationEnum.Check_E_ZDARZ_1.ToString(), HaveToBeInNcCode(mainProgram, "E_ZDARZ=1")));
                        _errors.Add(new NcCodeCheck(3, mainProgram, CheckingNcOperationEnum.Check_E_ZDARZ_2.ToString(), HaveToBeInNcCode(mainProgram, "E_ZDARZ=2")));
                    }
                    if (Machine.Contains("HSTM"))
                    {
                        _errors.Add(new NcCodeCheck(4, mainProgram, CheckingNcOperationEnum.Check_GOTO.ToString(), CantBeInNcCode(mainProgram, ";GOTO PROG_")));
                    }
                }
            }
            else
            {

            }
            return new List<NcCodeCheck>() { };
        }

        public List<NcCodeCheck> FindErrorsLimitsNcCode(string file, double Zmax, double Ymin, bool checkLimits)//, bool zmienZ, bool zmienY, bool sprawdzprzekroczenia)
        {
            if (File.Exists(file) && checkLimits == true)
            {
                Machine = _machineSettings.GetMachine(file).MachineName;
                if (Machine.Contains("HURON"))
                {
                    _errors.Add(new NcCodeCheck(1, file, CheckingNcOperationEnum.Check_limit_axis_Z.ToString(), ChecklimitedPositionZ(file, Zmax)));
                    _errors.Add(new NcCodeCheck(2, file, CheckingNcOperationEnum.Check_limit_axis_Y.ToString(), ChecklimitedPositionY(file, Ymin)));
                    //TODO dodac podmiane y i z przy G0
                }
            }
            return new List<NcCodeCheck>() { };
        }

        public List<NcCodeCheck> FindErrorsInSubProgram(string subProgram, int i)//do usuniecie popraw Web
        {
            if (File.Exists(subProgram))
            {
                if (!IsMainProgram(subProgram))
                {
                    Machine = _machineSettings.GetMachine(subProgram).MachineName;
                    GetErrorsForAll(subProgram, i);
                    GetErrorsAccMachine(subProgram, Machine, i);
                }
            }
            else
            {
                _errors.Add(new NcCodeCheck(i, subProgram, "Check file", new List<string> { "brak programu!" }));
                _logger.Warning($"Brak programu {subProgram}");
            }
            return new List<NcCodeCheck>() { };
        }

        /// <summary>
        /// Bierze podprogramy z current.xml
        /// </summary>
        /// <returns></returns>
        public List<NcCodeCheck> SetWarningIfIsToManySubprograms()
        {
            var currentXml = _pathService.GetFileCurrentToolsXml();
            var mainProgram = _toolsXmlFile.PRGNUMBER;
            var ncDir = _appXmlConfDirectory.NC_DIR;
            _listsuprogrammsFromNc = fileService.GetSubprogramsListFromNc(MainProgram);
            if (File.Exists(currentXml))
            {
                var listsuprogramms = fileService.GetListFilesWithName(ncDir, "SPF", mainProgram);
                for (int i = 0; i < listsuprogramms.Count; i++)
                {
                    GetWarningIfExistNotExpectedSubprogram(listsuprogramms[i].BatchFile, i);
                }
            }
            return new List<NcCodeCheck>() { };
        }

        /// <summary>
        /// Bierze podprogramy z wybranego programu glownego
        /// </summary>
        /// <param name="mainPrg"></param>
        /// <returns></returns>
        public List<NcCodeCheck> SetWarningIfIsToManySubprograms(string mainPrg)
        {
            var ncMainProgramService = new NcMainProgramService(mainPrg);
            var mainProgramName = ncMainProgramService.GetMainProgram().MainProgramName;
            _listsuprogrammsFromNc = fileService.GetSubprogramsListFromNc(mainPrg);
            var ncDir = Path.GetDirectoryName(mainPrg);
            if (File.Exists(mainPrg))
            {
                var listsuprogramms = fileService.GetListFilesWithName(ncDir, "SPF", mainProgramName);
                for (int i = 0; i < listsuprogramms.Count; i++)
                {
                    GetWarningIfExistNotExpectedSubprogram(listsuprogramms[i].BatchFile, i);
                }
            }
            return new List<NcCodeCheck>() { };
        }

        private void GetWarningIfExistNotExpectedSubprogram(string file, int i)
        {
            _errors.Add(new NcCodeCheck(i, file, "Check if exist not expected subprogram", NotExpectedNcCode(file)));
        }
        private List<string> NotExpectedNcCode(string file)
        {
            var errors = new List<string>() { };
            if (File.Exists(file))
            {
                if (_listsuprogrammsFromNc.Count() > 0)
                {
                    if (_listsuprogrammsFromNc.Any(x => x.SubProgramNameWithDir.Contains(file)) == false)
                    {
                        errors.Add($"Jest nieoczekiwany podprogram, nie ma go w programie glownym, patrz program : {file}");
                    }
                }
            }
            return errors;
        }
        public List<string> GetWhatIsCheck()
        {
            return _errors.Select(e => e.Message).Distinct().ToList();
        }
        public void ShowPreoloadTools(string mainprogram)
        {
            Machine = _machineSettings.GetMachine(mainprogram).MachineName;
            if (Machine.Contains("HX151") || Machine.Contains("HSTM500") || Machine.Contains("HSTM300HD"))
            {
                var toolsFromNc = ncService.LoadToolsFromFile(mainprogram);
                toolsFromNc.ForEach(x => _logger.Information($"{x.BatchFile} {x.ToolID} {x.ToolIDPreLoad} {x.CheckProload}"));
            }
        }
        public void ShowAllErrors()
        {
            var listErrors = _errors.Where(e => e.ListErrors.Count > 0).ToList();
            if (listErrors.Count > 0)
            {
                foreach (var error in listErrors)
                {
                    foreach (var item in error.ListErrors)
                    {
                        _logger.Error($"{error.NcProgramCheck} => {error.Message} => {item}");
                    }
                }
            }
            else
            {
                _logger.Information("Nie znaleziono bledow");
            }
        }
        public List<NcCodeCheck> GetAllErrors()
        {
            return _errors;
        }
        public void GetErrorsForAll(string file, int i)
        {
            _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_Spindle.ToString(), CheckSpidle(file)));
            _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.CheckSyntaxError.ToString(), CheckSyntaxError(file)));
            _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.CheckSyntaxError_in_TRANS.ToString(), CheckSyntaxErrorInTRANS(file)));
            _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_4_or_5_axis_if_G41.ToString(), checkG41G40axisAB(file)));
            if (!IsMainProgram(file))
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_G41_G42_G40.ToString(), CheckG41G42G40(file)));
        }
        public void GetErrorsAccMachine(string file, string machine, int i)
        {
            if (!machine.Contains(MachineEnum.HURON.ToString()) && !machine.Contains(MachineEnum.HEC.ToString()))
            {
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_M17.ToString(), HaveToBeInNcCode(file, "M17")));
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_M6.ToString(), HaveToBeInNcCode(file, "M6")));
            }
            if (machine.Contains(MachineEnum.HEC.ToString()))
            {
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_M17.ToString(), HaveToBeInNcCode(file, "M17")));
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_L300.ToString(), HaveToBeInNcCode(file, "L300")));
            }
            if (machine.Contains("HSTM") || machine.Contains(MachineEnum.HX151.ToString()))
            {
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_TRAORI.ToString(), checkTraoriPosition(file, machine)));
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_TRANS_for_drilling.ToString(), checkTRANSForDrilling(file, machine)));
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_A360.ToString(), CantBeInNcCode(file, "A360")));
                if (file.Contains("35.") || file.Contains("49."))
                {
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_E_ZDARZ_3.ToString(),
                        HaveToBeInNcCode(file, "E_ZDARZ=3")));
                }
                if (file.Contains("33N"))
                {
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_TRANS_X_R9.ToString(), checkCorrectPosition(file, "TRANS", "X=R9", "Spiral Milling Platform")));
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_G54_D3.ToString(), checkCorrectPosition(file, "G54", "D3", "Spiral Milling Platform")));
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_TRANS_X_R3.ToString(), checkCorrectPosition(file, "TRANS", "X=R3", "Spiral Finish Transition")));                    
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_G54_D1.ToString(), checkCorrectPosition(file, "G54", "D1", "Spiral Finish Transition")));
                }
                if (file.Contains("33B"))
                {
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_TRANS_X_R8.ToString(), checkCorrectPosition(file, "TRANS", "X=R8", "Spiral Milling Platform")));
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_G54_D4.ToString(), checkCorrectPosition(file, "G54", "D4", "Spiral Milling Platform")));
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_TRANS_X_R4.ToString(), checkCorrectPosition(file, "TRANS", "X=R4", "Spiral Finish Transition")));
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_G54_D2.ToString(), checkCorrectPosition(file, "G54", "D2", "Spiral Finish Transition")));
                }
            }
            if (machine.Contains(MachineEnum.HX151.ToString()))
            {
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_Lenght_of_tool.ToString(), Checklenghtoftool(file)));
            }
            if (machine.Contains("HURON"))
            {
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_L9006.ToString(), HaveToBeInNcCode(file, "L9006")));
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_M30.ToString(), HaveToBeInNcCode(file, "M30")));
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_A_DC_360.ToString(), CantBeInNcCode(file, "A=DC(360.")));
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_TRANS_for_drilling.ToString(), checkTRANSForDrilling(file, machine)));
                if (IsMainProgram(file))
                {
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_EVENT_1.ToString(), HaveToBeInNcCode(file, "EVENT(1)")));
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_EVENT_2.ToString(), HaveToBeInNcCode(file, "EVENT(2)")));
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_EVENT_3.ToString(), HaveToBeInNcCode(file, "EVENT(3)")));
                    _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_duplicate_tools.ToString(), Checkduplicatekubkow(file)));
                    //TODO checkG1G2, checkG1G3
                }
            }
            if (machine.Contains(MachineEnum.HSTM500M.ToString()))
            {
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_M348.ToString(), CantBeInNcCode(file, "M348")));
            }
            if (machine.Contains(MachineEnum.AVIA.ToString()))
            {
                _errors.Add(new NcCodeCheck(i, file, CheckingNcOperationEnum.Check_G64.ToString(), HaveToBeInNcCode(file, "G64")));
            }

            //TODO nie uzywane narazie od 2022-10-27
            //_errors.Add(new NcCodeCheck(i, file, "Check tool for gravering", checkToolForSzczotka(file, machine)));
            //_errors.Add(new NcCodeCheck(i, file, "Check tool for marking", checkToolForCecha(file, machine)));
        }
        private List<string> CheckPreload(string file)
        {
            var errors = new List<string>();
            var toolInfo = new List<Tool>();
            toolInfo.Clear();
            var toolsFromNC = ncService.LoadToolsFromFile(file);
            //trzeba sprawdzic czy wszystkie programy sa
            var checkFileExist = toolsFromNC.All(p => File.Exists(p.BatchFile));
            if (checkFileExist)
            {
                int count = 0;
                foreach (var tool in toolsFromNC)
                {
                    count++;
                    toolInfo.Add(new Tool
                    (
                        count,
                        Path.GetFileName(tool.BatchFile),
                        tool.Description,
                        tool.ToolSet,
                        tool.ToolID,
                        tool.ToolIDPreLoad,
                        tool.Toollen,
                        tool.ToolDiam,
                        tool.ToolCrn,
                        tool.Spindle,
                        tool.Feedrate,
                        tool.MaxMillTime,
                        tool.Offsets,
                        tool.Machine,
                        CheckCorrectPreloadTool(count, toolsFromNC, tool.ToolID, tool.ToolIDPreLoad)
                    ));
                }
                var notPassPreload= toolInfo.Where(x => x.CheckProload == true);
                foreach(var err in notPassPreload)
                {
                    errors.Add($"SPR.PRG => {err.BatchFile} => blad preloading!");
                }
            }
            return errors;
        }
        private bool CheckCorrectPreloadTool(int count, List<Tool> tools, string toolId, string toolPreload)
        {
            if (toolPreload == "-")
            {
                return true;
            }

            for (int i = count; i < tools.Count; i++)
            {
                if (tools[count].ToolID != tools[count-1].ToolIDPreLoad)
                {
                    return true;
                }
            }
            return false;
        }
        public List<string> checkTRANSForDrilling(string fileName, string machine)
        {
            var errors = new List<string>();
            var lines = File.ReadAllLines(fileName);
            string checkline = "";
            string drill = "";
            string searchtext = "Y";
            string szukajwiercenia = "";
            if (machine.Contains("HSTM"))
            {
                searchtext = "Y";
                szukajwiercenia = "drill";
            }
            if (machine == "HURON")
            {
                searchtext = "Z";
                szukajwiercenia = "Drill";
            }
            foreach (var line in lines)
            {
                if (line.Contains(searchtext))
                {
                    drill = "drill";
                }
            }
            if (drill == "drill")
            {
                foreach (var line in lines)
                {
                    if (line.Contains("TRANS") & line.Contains(searchtext))
                    {
                        if (line.Contains("ATRANS"))
                        {
                            checkline = line.Replace(" ", "").Replace("ATRANS", "");
                        }
                        else
                        {
                            checkline = line.Replace(" ", "").Replace("TRANS", "");
                        }
                        if (searchtext == "Y")
                        {
                            errors.Add("SPR.PRG =>" + fileName + " => blad skladni TRANS nie prawidlowa os Y podmien na Z , PATRZ BLOK:" + line);
                        }
                        if (searchtext == "Z")
                        {
                            errors.Add("SPR.PRG =>" + fileName + " => blad skladni TRANS nie prawidlowa os Z podmien na Y , PATRZ BLOK:" + line);
                        }
                    }
                }
            }
            return errors;
        }
        private bool IsMainProgram(string file)
        {
            if (File.Exists(file))
            {
                var nc = ncService.GetNcLinesFromNC(file);
                var countL9006 = nc.Select(n => nc.Count(nc => nc.Line.Contains("L9006")));
                if (countL9006.FirstOrDefault() > 2)
                {
                    return true;
                }
                var countExtcall = nc.Select(n => nc.Count(nc => nc.Line.Contains("EXTCALL")));
                if (countExtcall.FirstOrDefault() > 0)
                {
                    return true;
                }
                var countAvia = nc.Select(n => nc.Count(nc => nc.Line.Contains("PODPROGRAMIK")));
                if (countAvia.FirstOrDefault() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public List<string> Checkduplicatekubkow(string fileName)
        {
            var errors = new List<string>();
            if (File.Exists(fileName))
            {
                IEnumerable<string> lines = File.ReadAllLines(fileName);
                IList<string> listOfLine = new List<string>();
                int count = 0;
                foreach (var line in lines) { count++; listOfLine.Add(line); }
                IList<HuronTools> listOfLineWithTools = new List<HuronTools>();
                for (int i = 0; i < listOfLine.Count; i++)
                {
                    if (listOfLine[i].Contains("L9006") && !listOfLine[i - 1].Contains("FNORM"))
                    {
                        var line = listOfLine[i - 1];//TODO blad gdy w NC jest opis Kubek - naprawic??
                        listOfLineWithTools.Add(new HuronTools() { SetTool = line.Split(" ")[1].Split(";")[0], IdTool = line.Split(";")[1] });
                    }
                }
                var listOfLineWithNoDuplicatTools = listOfLineWithTools.Select(t => new { kubek = t.SetTool, Tool = t.IdTool }).Distinct();
                if (listOfLineWithNoDuplicatTools.Count() != listOfLineWithTools.Count())
                {
                    var duplicates = listOfLineWithTools.GroupBy(x => x.SetTool)
                                                 .Where(g => g.Count() > 1)
                                                 .Select(g => g.Key)
                                                 .ToList();
                    //duplicates.ForEach(t => Console.WriteLine($"{t}"));
                    if (duplicates != null)
                    {
                        if (String.Join(", ", duplicates) != "")
                        {
                            errors.Add("SPRAWDZ KUBKI : " + String.Join(", ", duplicates) + " W PRG. " + fileName);//pokazuje tylko duplo a nie ze kubek inny dla tego samego narzedzia!
                            return errors;
                        }
                    }
                }
            }
            else { errors.Add($"BRAK.PRG => {fileName} nie sprawdzono duplikatow kubkow"); }
            return errors;
        }
        public List<bool> GetBoolListWarning(List<Tool> toolsFromNc)//TODO patrz lepsze rozwiazanie w zrobile usunac!
        {
            var listboolwarning = new List<bool> { };
            if (toolsFromNc.Count > 0)
            {
                for (int i = 0; i < toolsFromNc.Count; i++)
                {
                    string toolfromxml = "-";
                    string toolchange = toolsFromNc[i].ToolID;
                    string toolpreload = toolsFromNc[i].ToolIDPreLoad;
                    listboolwarning.Add(false);
                }
            }
            for (int i = 0; i < toolsFromNc.Count - 1; i++)
            {
                string toolchangenext = toolsFromNc[i + 1].ToolID;
                string toolpreloadcurrent = toolsFromNc[i].ToolIDPreLoad;
                if (toolpreloadcurrent != toolchangenext)
                {
                    _logger.Debug(toolsFromNc[i] + " " + toolchangenext + " " + toolpreloadcurrent);
                    //listwarning.Add($"SPR.PRG => {toolsFromNc[i].BatchFile} => jest blad typu narzedzie wstawione nie pasuje do narzedzia w nastepnej wymianie!");
                    listboolwarning.RemoveAt(i);
                    listboolwarning.Insert(i, true);
                }
            }
            return listboolwarning;
        }
        private List<string> checkTraoriPosition(string file, string machine)
        {
            var errors = new List<string>();
            if (File.Exists(file) && machine != "HURON" && machine != "HEC" && machine != "AVIA" && !file.Contains("61."))
            {
                var lines = File.ReadAllLines(file);
                string searchTRAFOOF = "TRAFOOF";
                string searchTRAORI = "TRAORI";
                int count = 0;
                List<int> countline = new List<int>(new int[] { });
                List<string> listline = new List<string>(new string[] { });
                List<string> traoriline = new List<string>(new string[] { });
                listline.Clear();
                foreach (var line in lines)//tworzy liste lini i pozycji dla TRAFOOF
                {
                    listline.Add(line);
                    if (line.Contains(searchTRAFOOF))
                    {
                        countline.Add(count);
                    }
                    count++;
                }
                int searchareaDown = 30;
                foreach (int nmb in countline)//szukanie lini z ruchem osi
                {
                    try
                    {
                        for (int i = 1; i <= searchareaDown; i++)
                        {
                            _logger.Debug(listline[nmb + i]);
                            if (listline[nmb + i].Contains(searchTRAORI))
                            { break; }
                            if (listline[nmb + i].Contains("X") & listline[nmb + i].Contains("Y") & listline[nmb + i].Contains("Z") & !listline[nmb + i].Contains("G53"))
                            {
                                errors.Add($"=> BRAK TRAORI, PATRZ BLOK: {listline[nmb + i]}");
                                //break;
                            }
                        }
                    }
                    catch// (Exception e)
                    {
                        //throw new Exception("ERROR cos z lista nie tak!!", e);
                        //MessageBox.Show("ERROR checkTraoriPosition: " + e,"UWAGA! ",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return errors;
        }
        private List<string> checkG41G40axisAB(string file)
        {
            var errors = new List<string>();
            if (File.Exists(file))
            {
                var lines = File.ReadAllLines(file);
                int count = 0;
                string searchtext1 = "G41";
                string searchtext2 = "G42";
                string checktext = "G40";
                List<string> listline = new List<string>(new string[] { });
                List<int> listG41 = new List<int>(new int[] { });
                List<int> listG42 = new List<int>(new int[] { });
                List<int> listG40 = new List<int>(new int[] { });
                foreach (var line in lines)//tworzy liste lini i pozycji dla G41 G42 i G40
                {
                    listline.Add(line);
                    if (line.Contains(searchtext1))
                    {
                        listG41.Add(count);
                    }
                    if (line.Contains(searchtext2))
                    {
                        listG41.Add(count);
                    }
                    if (line.Contains(checktext))
                    {
                        if (line.Contains("G17 G0 G40 G64 G90"))
                        {
                            //nic nie robi huron
                        }
                        else
                        {
                            listG40.Add(count);
                        }
                    }
                    count++;
                }
                try//JAK PONIZEJ ZLIKWIDOWAC BLAD LUB GO POMINAC?
                {
                    if (listG41.Count == listG40.Count)
                    {
                        for (int i = 0; i <= listG40.Count; i++)
                        {
                            int j = listG41[i];
                            int k = listG40[i];

                            for (int m = j; m <= k; m++)
                            {
                                if (listline[m].Contains("A") || listline[m].Contains("B"))
                                {
                                    errors.Add("=> PRZY KOREKCJI POJAWILA SIE OS A LUB B!" + " PATRZ BLOK: " + listline[m]);
                                }
                            }
                        }
                    }
                }
                catch //(Exception e)
                {
                    //
                    //throw new Exception("ERROR cos z lista nie tak!!", e);
                    //MessageBox.Show("ERROR checkG41G40axisAB: " + e,"UWAGA! ",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return errors;
        }
        private List<string> CheckG41G42G40(string file)
        {
            var errors = new List<string>();
            if (File.Exists(file))
            {
                var nc = ncService.GetNcLinesFromNC(file);
                var resultG41 = nc.Count(nc => nc.Line.Contains("G41"));
                var resultG42 = nc.Count(nc => nc.Line.Contains("G42"));
                var resultG40 = nc.Count(nc => nc.Line.Contains("G40"));
                if (resultG41 > 0 || resultG40 > 0)
                {
                    if (resultG41 + resultG42 != resultG40)
                    {
                        errors.Add($"Nie zgadza sie ilosc korekcji promieniowej G41={resultG41} i G40={resultG40}!");
                        return errors;
                    }
                    return errors;
                }
            }
            else
            {
                errors.Add($"Brak programu, nie sprawdzono G41/G42/G40!");
                return errors;
            }
            return errors;
        }
        public List<string> CheckSpidle(string file)
        {
            var errors = new List<string>();
            if (File.Exists(file))
            {
                var nc = ncService.GetNcLinesFromNC(file);
                var valid = nc.Any(n => (n.Line.Contains("M03") || n.Line.Contains("M3")) && n.Line.Contains("S"));
                if (valid == false)
                {
                    errors.Add($"brak obrotow wrzeciona!");
                    return errors;
                }
            }
            return errors;
        }
        private List<string> CantBeInNcCode(string file, string word)
        {
            var errors = new List<string>();
            if (File.Exists(file))
            {
                var nc = ncService.GetNcLinesFromNC(file);
                var count = nc.Select(n => nc.Count(nc => nc.Line.Contains(word)));
                if (count.FirstOrDefault() > 0)
                {
                    errors.Add($"Nie może byc {word}!");
                    return errors;
                }
            }
            return errors;
        }
        private List<string> HaveToBeInNcCode(string file, string word)
        {
            var errors = new List<string>();
            if (File.Exists(file))
            {
                var nc = ncService.GetNcLinesFromNC(file);
                var count = nc.Select(n => nc.Count(nc => nc.Line.Contains(word)));
                if (count.FirstOrDefault() == 0)
                {
                    errors.Add($"Brak {word}!");
                    return errors;
                }
            }
            return errors;
        }
        private List<string> CheckSyntaxError(string file)
        {
            var errors = new List<string>();
            if (File.Exists(file))
            {
                var words = new List<string>() { "X", "Y", "Z", "A", "B", "F" };
                foreach (string word in words)
                {
                    var nc = ncService.GetNcLinesFromNC(file);
                    var findXTostring = nc.Where(n => n.Line.Contains(word) &&
                                                      !n.Line.Contains("E_ZDARZ") &&
                                                      !n.Line.ToLower().Contains("sz") &&
                                                      !n.Line.ToLower().Contains("cz") &&
                                                      !n.Line.ToLower().Contains("bezp") &&
                                                      !n.Line.ToLower().Contains("delta") &&
                                                      !n.Line.ToLower().Contains("raport") &&
                                                      !n.Line.Contains("TRAILON") &&
                                                      !n.Line.ToLower().Contains("trans") &&
                                                      !n.Line.ToLower().Contains("trafoof") &&
                                                      !n.Line.ToLower().Contains("toolno") &&
                                                      !n.Line.ToLower().Contains(";") &&
                                                      !n.Line.Contains("FFWON") &&
                                                      !n.Line.Contains("BRAK ODJAZDU") &&
                                                      !n.Line.Contains("CYCLE800") &&
                                                      !n.Line.Contains("MSG") &&
                                                      !n.Line.Contains("OFFN") &&
                                                      !n.Line.Contains("GOTO") &&
                                                      !n.Line.ToLower().Contains("cycle60")).ToList();
                    foreach (LineFromFile line in findXTostring)
                    {
                        string text = line.Line;
                        var count = text.ToLowerInvariant().Split(new string[] { word.ToLowerInvariant() }, StringSplitOptions.None).Count() - 1;
                        if (count > 1)
                        {
                            _logger.Debug($"{file} => {count} => {text}");
                            errors.Add(text);
                        }
                    }
                }
            }
            return errors;
        }
        private List<string> CheckSyntaxErrorInTRANS(string file)
        {
            var errors = new List<string>();
            if (File.Exists(file))
            {
                var words = new List<string>() { "X", "Y", "Z", "A", "B" };
                foreach (string word in words)
                {
                    var nc = ncService.GetNcLinesFromNC(file);
                    var findXTostring = nc.Where(n => n.Line.Contains(word) &&
                                                      n.Line.Contains("TRANS") &&
                                                      !n.Line.Contains("E_ZDARZ") &&
                                                      !n.Line.ToLower().Contains("sz") &&
                                                      !n.Line.ToLower().Contains("cz") &&
                                                      !n.Line.ToLower().Contains("bezp") &&
                                                      !n.Line.ToLower().Contains("delta") &&
                                                      !n.Line.Contains("TRAILON") &&
                                                      !n.Line.ToLower().Contains("raport") &&
                                                      !n.Line.ToLower().Contains("trafoof") &&
                                                      !n.Line.ToLower().Contains("toolno") &&
                                                      !n.Line.ToLower().Contains(";") &&
                                                      !n.Line.ToLower().Contains("cycle60")).ToList();
                    foreach (LineFromFile line in findXTostring)
                    {
                        _logger.Debug($"{line}");
                        string text = line.Line;
                        var count = text.Replace("TRANS", "").ToLowerInvariant().Split(new string[] { word.ToLowerInvariant() }, StringSplitOptions.None).Count() - 1;
                        if (count > 1)
                        {
                            _logger.Debug($"{file} => {count} => {text}");
                            errors.Add(text);
                        }
                    }
                }
            }
            return errors;
        }
        private List<string> Checklenghtoftool(string file)//only HX151
        {
            var errors = new List<string>() { };
            if (File.Exists(file))
            {
                var toolNcService = new ToolNcService();
                var toolSetLength = toolNcService.GetNcToollenFromNC(file);
                double dlugoscnarzedzia = 0.0;
                try
                {
                    dlugoscnarzedzia = Convert.ToDouble(toolSetLength);
                }
                catch
                {
                    errors.Add($"Bledny format dlugosci narzedzia w MSG, patrz program : {file}");
                }
                if (dlugoscnarzedzia > 205.0)
                {
                    errors.Add("DLUGOSC NARZEDZIA NIE MOZE PRZEKRACZAC 205MM, SPRAWDZ PROGRAM!");
                }
            }
            return errors;
        }
        private List<string> checkToolForSzczotka(string file, string machine)
        {
            var errors = new List<string>() { };
            if (File.Exists(file) && file.Contains("81.") && (machine.Contains("HX151") || machine.Contains("HSTM")))
            {
                string szukanaszczotka = "";
                var toolNcService = new ToolNcService();
                var toolId = toolNcService.GetToolIdFromNc(file);
                if (machine.Contains("HX151"))
                {
                    szukanaszczotka = "7758677";
                    if (szukanaszczotka != toolId)
                    {
                        errors.Add($" wymien szczote na {szukanaszczotka}!");
                    }
                }
                if (machine.Contains("HSTM"))
                {
                    szukanaszczotka = "7758476";
                    if (szukanaszczotka != toolId)
                    {
                        errors.Add($" wymien szczote na {szukanaszczotka}!");
                    }
                }

            }
            return errors;
        }
        private List<string> checkToolForCecha(string file, string machine)
        {
            var errors = new List<string>() { };
            if (File.Exists(file) && file.Contains("61.") && (machine.Contains("HX151") || machine.Contains("HSTM")))
            {
                string szukanacecha = "";
                var toolNcService = new ToolNcService();
                var toolId = toolNcService.GetToolIdFromNc(file);
                if (machine.Contains("HX151"))
                {
                    szukanacecha = "7757807";
                    if (szukanacecha != toolId)
                    {
                        errors.Add($" wymien cechowarke na {szukanacecha}!");
                    }
                }
                if (machine.Contains("HSTM"))
                {
                    szukanacecha = "7756910";
                    if (szukanacecha != toolId)
                    {
                        errors.Add($" wymien cechowarke na {szukanacecha}!");
                    }
                }
            }
            return errors;
        }
        private List<string> ChecklimitedPositionZ(string fileName, double Zmax)
        {
            var errors = new List<string>() { };
            if (File.Exists(fileName))
            {
                var lines = File.ReadAllLines(fileName);
                float zMaxValye = Convert.ToSingle(Zmax);
                //ostrzezenia przed blednym wpisaniem wartosci
                var listG0WithZ = lines.Where(l => l.Contains("G0") || l.Contains("G00"))
                    .Where(l => !l.Contains(";"))
                    .Where(l => l.Contains("Z"))
                    .Select(l => l)
                    ;
                if (listG0WithZ.Count() > 0)
                {
                    var separators = " ";
                    foreach (var item in listG0WithZ)
                    {
                        string[] splitedLine = item.Split(separators);
                        var findTxt = "Z";
                        var valueZ = 0.0;
                        for (int i = 0; i < splitedLine.Length; i++)
                        {
                            if (splitedLine[i].Contains(findTxt))
                            {
                                if (findTxt.Contains(findTxt))
                                {
                                    findTxt = splitedLine[i].Replace("Z", "");
                                    valueZ = Convert.ToDouble(splitedLine[i].Replace("Z", ""));//TODO dodac validacje
                                }
                            }
                            if (valueZ > zMaxValye)
                            {
                                errors.Add(" => przekroczono Z" + Zmax + ", PATRZ BLOK:" + item);
                            }
                        }
                    }
                }
            }
            var delDuplitatesErrors = errors.Distinct();
            return delDuplitatesErrors.ToList();
        }
        private List<string> ChecklimitedPositionY(string fileName, double Ymin)
        {
            var errors = new List<string>() { };
            if (File.Exists(fileName))
            {
                var lines = File.ReadAllLines(fileName);
                float yMinValue = Convert.ToSingle(Ymin);
                //ostrzezenia przed blednym wpisaniem wartosci
                if (yMinValue < 0.0)
                {
                    errors.Add($"Yaxis has to be possitive!");
                    return errors;
                }
                var listG0WithY = lines.Where(l => l.Contains("G0") || l.Contains("G00"))
                    .Where(l => !l.Contains(";"))
                    .Where(l => l.Contains("Y-"))
                    .Select(l => l)
                    ;
                if (listG0WithY.Count() > 0)
                {
                    var separators = " ";
                    foreach (var item in listG0WithY)
                    {
                        string[] splitedLine = item.Split(separators);
                        var findTxt = "Y";
                        var valueY = 0.0;
                        for (int i = 0; i < splitedLine.Length; i++)
                        {
                            if (splitedLine[i].Contains(findTxt))
                            {
                                if (findTxt.Contains(findTxt))
                                {
                                    findTxt = splitedLine[i].Replace("Y", "");
                                    valueY = Convert.ToDouble(splitedLine[i].Replace("Y", ""));//TODO dodac validacje
                                }
                            }
                            if (Math.Abs(valueY) > yMinValue)
                            {
                                errors.Add(" => przekroczono Y-" + Ymin + ", PATRZ BLOK:" + item);
                            }
                        }
                    }
                }
            }
            var delDuplitatesErrors = errors.Distinct();
            return delDuplitatesErrors.ToList();
        }

        private List<string> checkCorrectPosition(string file, string searchText, string trans, string operation)
        {
            var errors = new List<string>();
            if (File.Exists(file))
            {
                var lines = File.ReadAllLines(file);
                string searchCurrentTrans = trans;
                int count = 0;
                List<int> countline = new List<int>(new int[] { });
                List<string> listline = new List<string>(new string[] { });
                List<string> traoriline = new List<string>(new string[] { });
                listline.Clear();

                var modelLine = new List<LineFromFile>();

                var checkOperation = lines.Any(l => l.Contains(operation));

                if (checkOperation)
                {

                    for (int i = 0; i < lines.Count(); i++)
                    {
                        modelLine.Add(new LineFromFile() { Nmb = i, Line = lines[i] });
                    }

                    var startFromOperation = modelLine.Where(l => l.Line.Contains(operation)).Select(l=>l.Nmb).FirstOrDefault();

                    for (int i = startFromOperation; i < lines.Count(); i++)
                    {
                        listline.Add(lines[i]);
                        modelLine.Add(new LineFromFile() { Nmb = i, Line = lines[i] });
                        if (lines[i].Contains("G01") && lines[i].Contains("X") && lines[i].Contains("Y") && lines[i].Contains("Z"))
                        {
                            break;
                        }
                        count++;
                    }
                }

                if (!listline.Any(l=>l.Contains(trans)))
                {
                    errors.Add($"=> BRAK TRANS {trans} w operacji {operation}");
                    return errors;
                }

                int searchareaDown = 30;
                for (int i = 0; i < listline.Count(); i++)
                {
                    for (int j = 0; j <= searchareaDown; j++)
                    {
                        _logger.Debug(listline[i + j]);
                        if (listline[i + j].Contains("G00") && listline[i + j].Contains("X") & listline[i + j].Contains("Y") & listline[i + j].Contains("Z") & !listline[i + j].Contains("G53"))
                        {
                            for (int k = 0; k < 6; k++)
                            {
                                if (listline[(i + j) - k].Contains(searchText))
                                {
                                    if (!listline[(i + j) - k].Contains(trans))
                                    {
                                        errors.Add($"=> BRAK TRANS {trans} w operacji {operation}, PATRZ BLOK: {listline[(i + j) - k]}");
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return errors;
        }
    }
}