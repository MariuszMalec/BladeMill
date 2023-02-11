using BladeMill.BLL.Entities;
using BladeMill.BLL.Enums;
using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Models;
using BladeMill.BLL.Validators;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Do przerobki kodu Nc
    /// </summary>
    public class ConvertSubProgramsService : IConvertSubProgramsService
    {
        private string _setNewProgram;
        private string[] _content;
        private ConvertMainProgram _model;
        private string _newDirectoryName = @"";
        private AppXmlConfDirectories _appXmlConfDirectories;

        public ConvertSubProgramsService(ConvertMainProgram model)
        {
            _model = model;
            _appXmlConfDirectories = new AppXmlConfDirectories();
            _newDirectoryName = Path.Combine(_appXmlConfDirectories.NC_DIR, _model.NewProgramName);
        }

        public void FixSubPrograms()
        {
            var subProgramService = new SubProgramService(_model.ProgramName);
            var files = subProgramService.GetSubprogramsListFromNc().ToList();
            //dodac 78 i 85 do przerobki
            var mainProgram = new NcMainProgramService(_model.ProgramName);
            var dir = Path.GetDirectoryName(_model.ProgramName);
            var subPrg75 = Path.Combine(dir, mainProgram.GetMainProgram().MainProgramName + "75.SPF");
            var subPrg85 = Path.Combine(dir, mainProgram.GetMainProgram().MainProgramName + "85.SPF");
            if (File.Exists(subPrg75))
                files.Add(new SubProgram() { Id = files.Count()+1, SubProgramNameWithDir = subPrg75});
            if (File.Exists(subPrg85))
                files.Add(new SubProgram() { Id = files.Count()+1, SubProgramNameWithDir = subPrg85 });

            bool isMachineCorrect = ValidationMachine(_model.OrgMachine, _model.ProgramName);
            bool isMainProgramCorrect = ValidationMainProgram(_model.ProgramName);
            bool checkFileExist = ValidationsSubPrograms(files);

            if (_model.NewProgramName != null && files.Count > 0 &&
                checkFileExist && isMainProgramCorrect && isMachineCorrect)
            {
                if (!Directory.Exists(_newDirectoryName))
                {
                    Directory.CreateDirectory(_newDirectoryName);
                }

                var ncService = new ToolNcService();
                var toolsFromNc = ncService.LoadToolsFromFile(_model.ProgramName, true);

                foreach (var file in files)
                {
                    if (file.SubProgramNameWithDir.Contains("spf") || file.SubProgramNameWithDir.Contains("SPF"))
                    {
                        if (File.Exists(file.SubProgramNameWithDir))
                        {
                            _setNewProgram = GetNewSubprogramName(file);
                            _content = File.ReadAllLines(file.SubProgramNameWithDir);

                            ReplaceToolCycleCycleTool();

                            DeleteRaport();

                            if (_model.MachineType.ToString() == MachineEnum.HSTM500.ToString() ||
                                _model.MachineType.ToString() == MachineEnum.HSTM300HD.ToString())
                            {
                                if (_model.OrgMachine != MachineEnum.HSTM300HD.ToString() &&
                                    _model.OrgMachine != MachineEnum.HSTM500.ToString())
                                {
                                    AddPreload(toolsFromNc, file.SubProgramNameWithDir);
                                }
                            }

                            if (_model.MachineType.ToString() == MachineEnum.HSTM300.ToString() ||
                                _model.MachineType.ToString() == MachineEnum.HSTM500M.ToString() ||
                                _model.MachineType.ToString() == MachineEnum.HSTM1000.ToString())
                            {
                                RemovePreload();
                                if (_model.MachineType.ToString() == MachineEnum.HSTM500M.ToString())
                                {
                                    ReplaceM348ToM346();
                                }
                            }

                            ReplaceNameMachine();

                            Log.Information($"{_setNewProgram} -> SAVED");
                            File.WriteAllLines(_setNewProgram, _content);
                        }
                    }
                }
            }
        }

        private bool ValidationMachine(string selectedMachine, string mainProgram)
        {
            if (string.IsNullOrEmpty(selectedMachine))
            {
                Log.Error($"Nazwa maszyny nie moze byc pusta");
                return false;
            }
            if (string.IsNullOrEmpty(mainProgram))
            {
                Log.Error($"Nazwa programu nie moze byc pusta");
                return false;
            }
            var machineFactory = new MachineServiceFactory();
            var machine = machineFactory.CreateMachine(TypeOfFile.ncFile).GetMachine(mainProgram).MachineName;

            if ((machine == "HX151" && selectedMachine != "HX151") ||
                (machine != "HX151" && selectedMachine == "HX151"))
            {
                Log.Error($"Wybrano bledny program NC dla maszyny {selectedMachine}");
                return false;
            }
            return true;
        }

        private void ReplaceNameMachine()
        {
            Log.Debug($"podmiana nazwy maszyny w programie {_setNewProgram}");
            for (int i = 0; i < _content.Length; i++)
            {
                if (_model.MachineType.ToString() == MachineEnum.HSTM500M.ToString())
                {
                    _content[i] = _content[i].Replace("HSTM_500_SIM840D_Py", "HSTM_500M_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_300_SIM840D_Py", "HSTM_500M_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_300HD_SIM840D_Py", "HSTM_500M_SIM840D_Py");
                }
                if (_model.MachineType.ToString() == MachineEnum.HSTM500.ToString())
                {
                    _content[i] = _content[i].Replace("HSTM_500M_SIM840D_Py", "HSTM_500_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_300_SIM840D_Py", "HSTM_500_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_300HD_SIM840D_Py", "HSTM_500_SIM840D_Py");
                }
                if (_model.MachineType.ToString() == MachineEnum.HSTM300HD.ToString())
                {
                    _content[i] = _content[i].Replace("HSTM_500M_SIM840D_Py", "HSTM_300HD_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_500_SIM840D_Py", "HSTM_300HD_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_300_SIM840D_Py", "HSTM_300HD_SIM840D_Py");
                }
                if (_model.MachineType.ToString() == MachineEnum.HSTM300.ToString())
                {
                    _content[i] = _content[i].Replace("HSTM_500M_SIM840D_Py", "HSTM_300_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_500_SIM840D_Py", "HSTM_300_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_300HD_SIM840D_Py", "HSTM_300_SIM840D_Py");
                }
                if (_model.MachineType.ToString() == MachineEnum.HSTM1000.ToString())
                {
                    _content[i] = _content[i].Replace("HSTM_500M_SIM840D_Py", "HSTM_1000_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_500_SIM840D_Py", "HSTM_1000_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_300HD_SIM840D_Py", "HSTM_1000_SIM840D_Py");
                    _content[i] = _content[i].Replace("HSTM_300_SIM840D_Py", "HSTM_1000_SIM840D_Py");
                }
            }
        }

        private bool ValidationMainProgram(string mainProgram)
        {
            if (string.IsNullOrEmpty(mainProgram))
            {
                Log.Error($"Nazwa programu nie moze byc pusta");
                return false;
            }
            if (!mainProgram.ToUpper().Contains(".MPF"))
            {
                Log.Error($"Brak w nazwie programu ..MPF");
                return false;
            }
            if (!File.Exists(mainProgram))
            {
                Log.Error($"Brak glownego programu {mainProgram}");
                return false;
            }
            return true;
        }

        private static bool ValidationsSubPrograms(List<SubProgram> files)
        {
            var checkFileExist = files.ToList().All(f => f.IsSubProgram == true);
            if (!checkFileExist)
            {
                files.ToList().Where(f => f.IsSubProgram == false).DoAction(f => Log.Error($"{f.SubProgramNameWithDir}"));
                Log.Error($"Subprograms files Not exist!");
            }
            return checkFileExist;
        }

        private void ReplaceM348ToM346()
        {
            for (int i = 0; i < _content.Length; i++)
            {
                if (_content[i].Contains(@"M348"))
                {
                    Log.Debug($"podmiana M348 na M346 w programie {_setNewProgram}");
                    _content[i] = _content[i].Replace("M348 ; chlodziwo mgla", "M346 ; chlodziwo poprawiono");
                }
            }
        }

        public void CreateInfoFile(string mainProgram, string newProgramName)
        {
            if (string.IsNullOrEmpty(mainProgram))
            {
                Log.Warning($"Glowny program nie moze byc pusty {mainProgram}");
                return;
            }
            if (string.IsNullOrEmpty(newProgramName))
            {
                Log.Warning($"Nowy program nie moze byc pusty {newProgramName}");
                return;
            }
            string mypath = Path.GetDirectoryName(mainProgram);
            string myfile = Path.GetFileNameWithoutExtension(mainProgram);
            string newProgram = newProgramName.Replace(".MPF", "").Replace(".mpf", "");
            string destFile = Path.Combine(mypath, newProgram, "PRZEROBIONY Z " + myfile);

            if (!Directory.Exists(Path.GetDirectoryName(destFile)))
            {
                Log.Warning($"Nie ma katalogu {destFile}");
                return;
            }
            if (!File.Exists(destFile))
            {
                Log.Information($"zapis pliku {destFile}");
                File.WriteAllText(destFile, "");
            }
        }

        private void RemovePreload()
        {
            if (_model.DeletePreload == true)
            {
                for (int i = 0; i < _content.Length; i++)
                {
                    if (_content[i].Contains(@"Preloading tool"))
                    {
                        Log.Debug($"usunecie wstawienie nastepnego narzedzia w programie {_setNewProgram}");
                        string currentLine = _content[i + 1];
                        _content[i + 1] = _content[i].Replace(_content[i], $";{currentLine}");
                        break;
                    }
                }
            }
        }

        private void AddPreload(List<Tool> tools, string file)
        {
            if (_model.AddPreload == true && (!file.Contains("75.SPF") && !file.Contains("85.SPF")))
            {
                for (int i = 0; i < _content.Length; i++)
                {
                    if (_content[i].Contains(@"M03"))
                    {
                        var preloadTool = tools.Where(x => x.BatchFile == file).Select(x => x.ToolIDPreLoad).First();
                        preloadTool = $"N19 T=\"{preloadTool}\"";
                        Log.Debug($"wstawienie nastepnego narzedzia w programie {_setNewProgram}");
                        string currentLine = _content[i];
                        _content[i] = _content[i].Replace(_content[i], "; (nastepne narzedzie)\n" + preloadTool + "\n" + _content[i]);
                        break;
                    }
                    if (_content[i].Contains(@";Parametry") && _setNewProgram.Contains("61.SPF"))
                    {
                        var preloadTool = tools.Where(x => x.BatchFile == file).Select(x => x.ToolIDPreLoad).First();
                        preloadTool = $"N19 T=\"{preloadTool}\"";
                        Log.Debug($"wstawienie nastepnego narzedzia w programie {_setNewProgram}");
                        string currentLine = _content[i];
                        _content[i] = _content[i].Replace(_content[i], "; (nastepne narzedzie)\n" + preloadTool + "\n" + _content[i] + "\n" + ";");
                        break;
                    }
                }
            }
        }
        private void DeleteRaport()
        {
            if (_model.DeleteRaport == true)
            {
                for (int i = 0; i < _content.Length; i++)
                {
                    if (_content[i].Contains(@"RAPORT"))
                    {
                        Log.Debug($"usuniecie RAPORT w programie {_setNewProgram}");
                        _content[i] = _content[i].Replace("RAPORT", ";RAPORT");
                    }
                }
            }
        }
        private void ReplaceToolCycleCycleTool()
        {
            if (_model.ReplaceToolCycle == true)
            {
                Log.Debug($"Zamiania Tool na Cycle w programie {_setNewProgram}");
                for (int i = 0; i < _content.Length; i++)
                {
                    if (_content[i].Contains(@"TOL(0.1,1,1)"))
                    {
                        _content[i] = _content[i].Replace("TOL(0.1,1,1)", "CYCLE832(0.1,1,1)");
                    }
                    if (_content[i].Contains(@"TOL(0.001,1,1)"))
                    {
                        _content[i] = _content[i].Replace("TOL(0.001,1,1)", "CYCLE832(0.01,1,1)");
                    }
                    if (_content[i].Contains(@"TOL(0.01,1,1)"))
                    {
                        _content[i] = _content[i].Replace("TOL(0.01,1,1)", "CYCLE832(0.01,1,1)");
                    }
                }
            }
            if (_model.ReplaceCycleTool == true)
            {
                Serilog.Log.Debug($"Zamiania Cycle na Tool w programie {_setNewProgram}");
                for (int i = 0; i < _content.Length; i++)
                {
                    if (_content[i].Contains(@"CYCLE832(0.1,3,1)"))
                    {
                        _content[i] = _content[i].Replace("CYCLE832(0.1,3,1)", "TOL(0.1,1,1)");
                    }
                    if (_content[i].Contains(@"CYCLE832(0.01,1,1)"))
                    {
                        _content[i] = _content[i].Replace("CYCLE832(0.01,1,1)", "TOL(0.01,1,1)");
                    }
                    if (_content[i].Contains(@"CYCLE832(0.1,1,1)"))
                    {
                        _content[i] = _content[i].Replace("CYCLE832(0.1,1,1)", "TOL(0.1,1,1)");
                    }
                    if (_content[i].Contains(@"CYCLE832(0.001,1,1)"))
                    {
                        _content[i] = _content[i].Replace("CYCLE832(0.001,1,1)", "TOL(0.01,1,1)");
                    }
                }
            }
        }
        public string GetNewSubprogramName(SubProgram file)
        {
            if (_model.ProgramName == null)
            {
                Log.Error($"Nie mozna stworzyc nowego programu!");
                return string.Empty;
            }
            var getProgramName = Path.GetFileNameWithoutExtension(_model.ProgramName);
            getProgramName = getProgramName.Remove(getProgramName.Length - 2);
            var setNewProgram = Path.GetFileName(file.SubProgramNameWithDir);
            setNewProgram = Path.Combine(_newDirectoryName, setNewProgram.Replace(getProgramName, _model.NewProgramName));
            return setNewProgram;
        }
    }
}
