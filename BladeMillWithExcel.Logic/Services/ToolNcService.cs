using BladeMillWithExcel.Logic.Enums;
using BladeMillWithExcel.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMillWithExcel.Logic.Services
{
    public class ToolNcService : IToolService
    {
        private static List<Tool> _toolsFromNc = new List<Tool>();
        public int _count = 0;
        public int count = 0;

        /// <summary>
        /// Czyta liste na podstawie M6 i preload
        /// </summary>
        /// <param name="mainProgramFile"></param>
        /// <returns></returns>
        public List<Tool> LoadToolsFromFile(string mainProgramFile)//only hamuel
        {
            var _toolsFromNc = new List<Tool>() { };
            if (File.Exists(mainProgramFile))
            {
                var listsuprogramms = GetSubprogramsListFromNc(mainProgramFile).ToList();
                for (int i = 0; i < listsuprogramms.Count; i++)
                {
                    if (i == (listsuprogramms.Count - 1))
                        _toolsFromNc.Add(GetToolNc(listsuprogramms[i].SubProgramNameWithDir, listsuprogramms[0].SubProgramNameWithDir));
                    if (i != (listsuprogramms.Count - 1))
                        _toolsFromNc.Add(GetToolNc(listsuprogramms[i].SubProgramNameWithDir, listsuprogramms[i + 1].SubProgramNameWithDir));
                }
                //korekcja preload
                if (_toolsFromNc.Count > 0)
                {
                    //trzeba sprawdzic czy wszystkie programy sa
                    var toolInfo = new List<Tool>();
                    toolInfo.Clear();
                    var checkFileExist = _toolsFromNc.All(p => File.Exists(p.BatchFile));
                    if (checkFileExist)
                    {
                        int count = 0;
                        foreach (var tool in _toolsFromNc)
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
                                tool.CheckProload = CheckCorrectPreloadTool(count, _toolsFromNc, tool.ToolID, tool.ToolIDPreLoad)
                            ));
                        }
                    }
                }
                return _toolsFromNc;
            }
            return _toolsFromNc;
        }
        /// <summary>
        /// Czyta liste narzedzi tylko na podstawie M6
        /// </summary>
        /// <param name="mainProgramFile"></param>
        /// <param name="convertNc"></param>
        /// <returns></returns>
        public List<Tool> LoadToolsFromFile(string mainProgramFile, bool convertNc)//only hamuel and convertNc
        {
            var _toolsFromNc = new List<Tool>() { };
            if (File.Exists(mainProgramFile))
            {
                var listsuprogramms = GetSubprogramsListFromNc(mainProgramFile).ToList();
                for (int i = 0; i < listsuprogramms.Count; i++)
                {
                    if (i == (listsuprogramms.Count - 1))
                        _toolsFromNc.Add(GetToolNcM6(listsuprogramms[i].SubProgramNameWithDir, listsuprogramms[0].SubProgramNameWithDir));
                    if (i != (listsuprogramms.Count - 1))
                        _toolsFromNc.Add(GetToolNcM6(listsuprogramms[i].SubProgramNameWithDir, listsuprogramms[i + 1].SubProgramNameWithDir));
                }
                return _toolsFromNc;
            }
            return _toolsFromNc;
        }

        private IEnumerable<SubProgram> GetSubprogramsListFromNc(string mainProgram)
        {
            var list = new List<SubProgram>() { };
            if (File.Exists(mainProgram))
            {
                string[] lines = File.ReadAllLines(mainProgram);
                //Avia
                var getProgramiki = lines.Where(n => n.Contains("PODPROGRAMIK")).Select(n => n);
                getProgramiki.ToList().ForEach(p => list.Add(GetSubprogramAsProgramik(mainProgram, p)));
                //hstms and hx
                foreach (string line in lines)
                {
                    if (line.Contains("EXTCALL"))//only HSTMs
                    {
                        list.Add(GetSubprogramAsExtcall(mainProgram, line));
                    }
                }
                //Huron
                if (lines.Contains("L9006"))
                {
                    list.Add(new SubProgram() { Id = 1, Created = DateTime.Now, SubProgramNameWithDir = mainProgram });
                }
            }
            return list;
        }
        private SubProgram GetSubprogramAsExtcall(string file, string line)
        {
            char[] delimiterChars = { '(', ')' }; _count++;
            string[] NCProgram = line.Split(delimiterChars);
            NCProgram = NCProgram[1].Split('"');
            string ncFile = Path.Combine(Path.GetDirectoryName(file), NCProgram[1] + ".SPF");
            return new SubProgram()
            {
                Id = _count,
                Created = DateTime.Now,
                SubProgramNameWithDir = ncFile,
            };
        }
        private SubProgram GetSubprogramAsProgramik(string file, string line)
        {
            char[] delimiterChars = { ' ', ';' }; _count++;
            string[] NCProgram = line.Split(delimiterChars);
            if (delimiterChars.Length > 0)
            {
                NCProgram = NCProgram[1].Split();
            }
            else
            {
                NCProgram = NCProgram[0].Split();
            }
            string ncFile = Path.Combine(Path.GetDirectoryName(file), NCProgram[0] + ".SPF");
            return new SubProgram()
            {
                Id = _count,
                Created = DateTime.Now,
                SubProgramNameWithDir = ncFile,
            };
        }

        public List<LineFromFile> GetNcLinesFromNC(string file)
        {
            var ncinfo = new List<LineFromFile>();
            if (File.Exists(file))
            {
                string[] linesReaded = File.ReadAllLines(file);
                int lineNb = 1;

                ncinfo.Clear();
                foreach (string line in linesReaded)
                {
                    if (line != "")
                    {
                        string[] temp = { lineNb.ToString(), line };
                        ncinfo.Add(new LineFromFile() { Nmb = lineNb, Line = line });
                        lineNb++;
                    }
                }
            }
            return ncinfo;
        }
        private bool CheckCorrectPreloadTool(int count, List<Tool> tools, string toolId, string toolPreload)
        {
            if (toolPreload == "-")
            {
                return true;
            }

            for (int i = count; i < tools.Count; i++)
            {
                if (tools[count].ToolID != tools[count - 1].ToolIDPreLoad)
                {
                    return true;
                }
            }
            return false;
        }
        public string GetToolIdPreloadFromNc(string file)//TODO poprawic w nc dodac unit test
        {
            var toolIDPreload = "-";
            var nc = GetNcLinesFromNC(file);
            //check if not null
            var valid = nc.Where(n => n.Line.Contains(" T="))
                .Select(n => n.Line).LastOrDefault();
            if (valid != null)
            {
                toolIDPreload = valid.ToString().Split('=')[1].Replace("\"", "");
            }
            return toolIDPreload;
        }
        public string GetNcSpindleFromNC(string file)
        {
            var spindle = "-";
            var nc = GetNcLinesFromNC(file);
            //check if not null
            var valid = nc.Where(n => n.Line.Contains("M03"))
                .Select(n => n.Line).FirstOrDefault();
            if (valid != null)
            {
                spindle = valid.ToString().Split(' ')[1]
                        .Replace(" ", "")
                        .Replace("M03", "")
                    ;
            }
            return spindle;
        }
        public string GetNcToolCrnFromNC(string file)
        {
            var toolCrn = "-";
            var nc = GetNcLinesFromNC(file);
            //check if not null
            var valid = nc.Where(n => n.Line.Contains("MSG") &&
                                      !n.Line.Contains("E_STR") &&
                                      !n.Line.Contains("KONIEC") &&
                                      !n.Line.Contains("GROUPED"))
                .Select(n => n.Line).FirstOrDefault();
            if (valid != null)
            {
                toolCrn = valid.ToString().Split('=')[3]
                        .Replace(" ", "")
                        .Replace("Ang", "")
                    ;
            }
            return toolCrn;
        }
        public string GetNcToolDiamFromNC(string file)
        {
            var toolDiam = "-";
            var nc = GetNcLinesFromNC(file);
            //check if not null
            var valid = nc.Where(n => n.Line.Contains("MSG") &&
                                      !n.Line.Contains("E_STR") &&
                                      !n.Line.Contains("KONIEC") &&
                                      !n.Line.Contains("GROUPED"))
                .Select(n => n.Line).FirstOrDefault();
            if (valid != null)
            {
                toolDiam = valid.ToString().Split('=')[2]
                    .Replace(" ", "")
                    .Replace("R", "")
                    ;
            }
            return toolDiam;
        }
        public string GetNcToollenFromNC(string file)
        {
            var toollen = "-";
            var nc = GetNcLinesFromNC(file);
            //check if not null
            var valid = nc.Where(n => n.Line.Contains("MSG") &&
                                      !n.Line.Contains("E_STR") &&
                                      !n.Line.Contains("KONIEC") &&
                                      !n.Line.Contains("GROUPED"))
                .Select(n => n.Line).FirstOrDefault();
            if (valid != null)
            {
                toollen = valid.ToString().Split('=')[5]
                    .Replace(" ", "")
                    .Replace(")", "")
                    .Replace("\"", "");
            }
            return toollen;
        }
        public string GetDescriptionFromNc(string file)
        {
            var descriptionOperation = "-";
            var nc = GetNcLinesFromNC(file);
            //check if not null
            var valid = nc.Where(n => n.Line.Contains("/"))
                .Select(n => n.Line).FirstOrDefault();
            if (valid != null)
            {
                descriptionOperation = valid.ToString().Split('/')[1];
            }
            return descriptionOperation;
        }
        public string GetToolIdFromNc(string file)
        {
            var toolID = "-";
            var nc = GetNcLinesFromNC(file);
            //check if not null
            var valid = nc.Where(n => n.Line.Contains("T="))
                .Select(n => n.Line).FirstOrDefault();
            if (valid != null)
            {
                toolID = valid.ToString().Split('=')[1].Replace("\"", "").Replace(" ", "");
            }
            return toolID;
        }
        public string GetMachineFromFile(string file)
        {
            var machine = string.Empty;
            var nc = GetNcLinesFromNC(file);
            //check if not null
            var validMACHINE = nc.Where(n => n.Line.Contains("MACHINE"))
                .Select(n => n.Line).FirstOrDefault();
            if (validMACHINE != null)
            {
                machine = validMACHINE.ToString().Split(':')[1].Replace(" ", "");
                return SimplyMachineName(machine);
            }
            var validOBRABIARKA = nc.Where(n => n.Line.Contains("OBRABIARKA"))
                                 .Select(n => n.Line).FirstOrDefault();
            if (validOBRABIARKA != null)
            {
                machine = validOBRABIARKA.ToString().Split(':')[1].Replace(" ", "");
                return SimplyMachineName(machine);
            }
            //Hec
            var validHec = nc.Where(n => n.Line.Contains("L300"))
                .Select(n => n.Line).FirstOrDefault();
            if (validHec != null)
            {
                machine = MachineEnum.HEC.ToString();
                return MachineEnum.HEC.ToString();
            }
            //Avia
            var validAvia = nc.Where(n => n.Line.Contains("CYCLE800") || n.Line.Contains("PODPROGRAMIK"))
                .Select(n => n.Line).FirstOrDefault();
            if (validAvia != null)
            {
                machine = MachineEnum.AVIA.ToString();
                return MachineEnum.AVIA.ToString();
            }
            //special if subprogram 61
            var validCycle60 = nc.Where(n => n.Line.Contains("CYCLE60"))
                .Select(n => n.Line).FirstOrDefault();
            if (validCycle60 != null)
            {
                machine = MachineEnum.HSTM300.ToString();
                return MachineEnum.HSTM300.ToString();
            }
            return machine;
        }
        public string GetNcMachineFromNC(string fileNC)
        {
            var machine = string.Empty;
            var nc = GetNcLinesFromNC(fileNC);
            //check if not null
            var validMACHINE = nc.Where(n => n.Line.Contains("MACHINE"))
                .Select(n => n.Line).FirstOrDefault();
            if (validMACHINE != null)
            {
                machine = validMACHINE.ToString().Split(':')[1].Replace(" ", "");
                return SimplyMachineName(machine);
            }
            var validOBRABIARKA = nc.Where(n => n.Line.Contains("OBRABIARKA"))
                                 .Select(n => n.Line).FirstOrDefault();
            if (validOBRABIARKA != null)
            {
                machine = validOBRABIARKA.ToString().Split(':')[1].Replace(" ", "");
                return SimplyMachineName(machine);
            }
            return machine;
        }
        private string SimplyMachineName(string machine)
        {
            switch (machine)
            {
                case ("HSTM_300_SIM840D_Py"):
                    machine = MachineEnum.HSTM300.ToString();
                    break;
                case ("SH_HX151_24_SIM840D"):
                    machine = MachineEnum.HX151.ToString();
                    break;
                case ("HSTM_500M_SIM840D_Py"):
                    machine = MachineEnum.HSTM500M.ToString();
                    break;
                case ("HURON_EX20_SIM840D"):
                    machine = MachineEnum.HURON.ToString();
                    break;
                case ("HSTM_1000_SIM840D_Py"):
                    machine = MachineEnum.HSTM1000.ToString();
                    break;
                case ("HSTM_300HD_SIM840D_Py"):
                    machine = MachineEnum.HSTM300HD.ToString();
                    break;
                case ("HSTM_500_SIM840D_Py"):
                    machine = MachineEnum.HSTM500.ToString();
                    break;
                default:
                    machine = string.Empty;
                    break;
            }
            return machine;
        }
        private Tool GetToolNc(string subfile, string nextSubfile)//TODO sprawdz createtoolnc!! checkNC!!
        {
            count++;
            Tool toolNc = new Tool(count,
                subfile,
                GetDescriptionFromNc(subfile),
                "",
                GetToolIdFromNc(subfile),
                GetToolIdPreloadFromNc(subfile),
                GetNcToollenFromNC(subfile),
                GetNcToolDiamFromNC(subfile),
                GetNcToolCrnFromNC(subfile),
                GetNcSpindleFromNC(subfile),
                "",
                "",
                "",
                GetNcMachineFromNC(subfile),
                false);
            return toolNc;
        }

        private Tool GetToolNcM6(string subfile, string nextSubfile)//TODO tylko dla convertNc
        {
            count++;
            Tool toolNc = new Tool(count,
                subfile,
                GetDescriptionFromNc(subfile),
                "",
                GetToolIdFromNc(subfile),
                GetToolIdFromNc(nextSubfile),
                GetNcToollenFromNC(subfile),
                GetNcToolDiamFromNC(subfile),
                GetNcToolCrnFromNC(subfile),
                GetNcSpindleFromNC(subfile),
                "",
                "",
                "",
                GetNcMachineFromNC(subfile),
                false);
            return toolNc;
        }

        //HURON only
        /// <summary>
        /// Wczytanie danych narzedziowych z programu glownego (tylko Huron!)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<Tool> LoadNCMultiple(string file)//only Huron
        {
            var count = 1;
            var machine = GetNcMachineFromNC(file);
            var toolNC = new List<Tool>();
            string tDiam = "-";
            string tDescription = "-";
            string tSpindle = "-";
            string tToolId = "-";
            string tToolCrn = "-";
            string tToolLen = "-";
            string tToolSet = "-";
            var toolSet = GetMsgLineWithToolSet(file);
            if (toolSet != null)
            {
                foreach (var item in toolSet)
                {
                    tToolId = item.Split('=')[1].ToString().Replace(" TLD", "");
                    tDiam = item.Split('=')[2].ToString().Replace(" TLR", "");
                    tToolCrn = item.Split('=')[3].ToString().Replace(" TLA", "");
                    tToolLen = item.Split('=')[5].ToString().Replace(")", "").Replace("\"", "");
                    tToolSet = item.Split('=')[6].ToString().Replace("\"", "");
                    tDescription = item.Split('=')[7].ToString().Replace("\"", "");
                    tSpindle = item.Split('=')[8].ToString().Replace("\"", "");
                    toolNC.Add(new Tool(count++, file, tDescription, tToolSet, tToolId, "-", tToolLen, tDiam, tToolCrn, tSpindle, "-", "-", "-", machine, false));
                }
                return toolNC;
            }
            return toolNC;
        }

        private List<string> GetMsgLineWithToolSet(string file)
        {
            var toolSet = new List<string>();
            var nc = GetNcLinesFromNC(file);
            var searchingL9006 = 10;
            var searchingDescription = 40;
            var searchingSpindle = 20;
            Console.WriteLine("------------------Szukam MSG-------------------");
            List<string> msglines = new List<string>() { };
            List<string> l9006lines = new List<string>() { };
            for (int i = 0; i < nc.Count; i++)
            {
                if (nc[i].Line.Contains("MSG") && nc[i].Line.Contains("TLID"))
                {
                    for (int j = 0; j < searchingL9006; j++)
                    {
                        if (nc[i + j].Line.Contains("T")
                                && nc[i + j].Line.Contains(";")
                                && !nc[i + j].Line.Contains(":")
                                && !nc[i + j].Line.Contains("CUTTER")
                                && !nc[i + j].Line.Contains("(")
                                && !nc[i + j].Line.Contains("wstawione")
                                && !nc[i + j].Line.Contains("ACS")
                                && !nc[i + j].Line.Contains("POMIAR")
                                && !nc[i + j].Line.Contains("NNT")
                                && !nc[i + j].Line.Contains("LIMIT"))
                        {
                            var l9006Line = GetSetTool(nc[i + j].Line);
                            l9006lines.Add(l9006Line);
                            var msgLine = nc[i].Line;
                            msglines.Add(msgLine);

                            //get description
                            var descriptionLine = "-";
                            for (int k = 0; k < searchingDescription; k--)
                            {
                                if (nc[i - k].Line.Contains("; OPER. DESCR. :"))
                                {
                                    descriptionLine = GetDescritptionFromLine(nc[i - k].Line);
                                    break;
                                }
                            }

                            //get spindle
                            var spindleLine = "-";
                            for (int k = 0; k < searchingSpindle; k++)
                            {
                                if (nc[i + k].Line.Contains("S") && nc[i + k].Line.Contains("M03"))
                                {
                                    spindleLine = GetSpindleFromLine(nc[i + k].Line);
                                    break;
                                }
                            }

                            toolSet.Add(msgLine + "=" + l9006Line + "=" + descriptionLine + "=" + spindleLine);
                            break;
                        }
                    }
                }
            }
            if (msglines.Count() != l9006lines.Count())
            {
                return new List<string>();
            }
            return toolSet;
        }

        private string GetSpindleFromLine(string line)
        {
            var spindle = "-";
            spindle = $"{line.Split('S')[1].Replace(" M03", "")}";
            return spindle;
        }

        private string GetDescritptionFromLine(string line)
        {
            var description = "-";
            try
            {
                description = $"{line.Split('/')[1]}";
            }
            catch { description = $"{line.Split('/')[0].Replace("; OPER. DESCR. : ", "")}"; }
            return description;
        }

        private string GetSetTool(string toolLine)
        {
            var toolSet = "-";
            toolSet = $"T{toolLine.Replace(" ", "").Split('T')[1].Split(';')[0]}";
            return toolSet;
        }

        private bool checkIfIsMainProgram(string file)
        {
            bool result = false;
            var nc = GetNcLinesFromNC(file);
            var valid = nc.Count(n => n.Line.Contains("L9006"));
            if (valid > 1)
            {
                result = true;
                return result;
            }
            return result;
        }
    }
}
