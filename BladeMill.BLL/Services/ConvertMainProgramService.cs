using BladeMill.BLL.Entities;
using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Do przerobki kodu Nc
    /// </summary>
    public class ConvertMainProgramService : IConvertMainProgramService
    {
        private string _templateMainProgram = @"";
        private AppXmlConfDirectories _appXmlConfDirectories;
        private static List<string> orglines = new List<string>(new string[] { });
        private static List<string> headerLines = new List<string>(new string[] { });
        private static List<string> trailerLines = new List<string>(new string[] { });
        private static List<string> finiLines = new List<string>(new string[] { });
        private static List<string> subProgramLines = new List<string>(new string[] { });
        private double pos_U;
        private ConvertMainProgram _model;
        private string _newDirectoryName = @"";

        public ConvertMainProgramService(ConvertMainProgram model)
        {
            subProgramLines.Clear();
            headerLines.Clear();
            trailerLines.Clear();
            finiLines.Clear();
            _appXmlConfDirectories = new AppXmlConfDirectories();
            _model = model;
            //TODO zabezpiczenie aby moc wstrzykiwac do serwisow
            if (_model.NewProgramName == null)
                _model.NewProgramName = "123456";
            _newDirectoryName = Path.Combine(_appXmlConfDirectories.NC_DIR, _model.NewProgramName);
        }

        public void FixMainProgram()
        {
            var subProgramService = new SubProgramService(_model.ProgramName);
            var files = subProgramService.GetSubprogramsListFromNc();

            if (!Directory.Exists(_newDirectoryName))
            {
                Directory.CreateDirectory(_newDirectoryName);
            }

            string setNewProgram = GetNewMainProgram();

            CopyMainProgramTemplate();

            if (File.Exists(_model.ProgramName))
            {
                if (_model.OrgClamping.Contains("Zabierak") || _model.OrgClamping.Contains("ZABIERAK"))
                {
                    //przygotowanie headera
                    AddHeader();

                    //kalkulacja pozycjiU
                    GetPositionU();

                    //przygotowanie dojazdu
                    AddTrailer();

                    //przygotowanie nowych podprogramow do wklejenia
                    AddSubPrograms(files.ToList());

                    //koniec
                    AddFinish();
                }
                else
                {
                    Log.Error("Program glowny nie zostal prawidlowo przerobiony ze wzgledu na mocowanie");
                }
            }

            //Podmiana templeta
            if (File.Exists(_model.TemplateMainProgram))
            {
                //var content = File.ReadAllLines(_model.TemplateMainProgram);
                StreamReader file = new StreamReader(_model.TemplateMainProgram);
                string line;
                string pinmilling = "BRAK!";
                while ((line = file.ReadLine()) != null)
                {
                    if (!line.Contains("HSTM_300_SIM840D_Py") && !line.Contains(pinmilling) && !line.Contains("GOTO PROG_")//
                        && !line.Contains("G0 D0 G53 X-200 Y150 Z550") && !line.Contains("KONIKIEM")//
                        && !line.Contains("M342") && !line.Contains("PROGRAM NAME")//
                        && !line.Contains("HSTM_1000_SIM840D_Py") && !line.Contains("KOPP_OFF") && !line.Contains("ORDER")//
                        && !line.Contains("HSTM300") && !line.Contains("EXTCALL") && !line.Contains("PROG_") && !line.Contains("U_RAW_PART_LENGTH")
                        && !line.Contains("SUBPROGRAMS") && !line.Contains("HEADER") && !line.Contains("R61=")
                        && !line.Contains("TRAILER") && !line.Contains("FINI"))
                    {
                        orglines.Add(line);
                    }
                    else
                    {
                        if (line.Contains(";HEADER"))
                        {
                            for (int i = 0; i < headerLines.Count; i++)
                            {
                                Serilog.Log.Debug($"Wstawienie naglowka {headerLines[i]}");
                                orglines.Add(headerLines[i].ToUpper());
                            }
                        }
                        if (line.Contains(";TRAILER"))
                        {
                            for (int i = 0; i < trailerLines.Count; i++)
                            {
                                Serilog.Log.Debug($"Wstawienie dojazdu {trailerLines[i]}");
                                orglines.Add(trailerLines[i]);
                            }
                        }
                        if (line.Contains(";SUBPROGRAMS"))
                        {
                            for (int i = 0; i < subProgramLines.Count; i++)
                            {
                                Serilog.Log.Debug($"Wstawienie nowego podprogramu {subProgramLines[i]}");
                                orglines.Add(subProgramLines[i].ToUpper());
                            }
                        }
                        if (line.Contains(";FINI"))
                        {
                            for (int i = 0; i < finiLines.Count; i++)
                            {
                                Serilog.Log.Debug($"Wstawienie koncowki {finiLines[i]}");
                                orglines.Add(finiLines[i]);
                            }
                        }
                    }
                }
                file.Close();
                File.WriteAllLines(setNewProgram, orglines);
                Serilog.Log.Information($"{setNewProgram} -> SAVED");
            }
            else
            {
                Serilog.Log.Error($"Brak szablonu programu glownego!");
            }
        }

        private List<string> AddFinish()
        {
            Serilog.Log.Debug($"Dodanie koncowki w programie {_model.NewProgramName}");
            if (_model.OrgClamping.Contains("Zabierak") || _model.OrgClamping.Contains("ZABIERAK"))
            {
                finiLines.Add($"N56 G0 G53 Z550 D0");
                finiLines.Add($"N57 G0 X-200");
                finiLines.Add($"N58 M9");
                finiLines.Add($"N59 PRESS_OFF");
                finiLines.Add($"N60 G0 U499");
                finiLines.Add($"N61 M37");
                finiLines.Add($"N62 G04 F1");
                finiLines.Add($"N63 M38");
                finiLines.Add($"N64 G0 G53 X-200 A0 Y0 B0");
                finiLines.Add($"N65 STOPRE");
                finiLines.Add($"N66 E_ZDARZ=2 ; ACS - koniec obrobki");
                finiLines.Add($"N67 T_STOP(1)");
                finiLines.Add($"N68 R99=0");
                finiLines.Add($";N69 WP_CH");
                finiLines.Add($";N70 WP_PREP");
                finiLines.Add($";N71 GOTOB ASTART");
                finiLines.Add($"N72 M38");
                finiLines.Add($"N73 M30");
            }
            return finiLines;
        }

        private List<string> AddTrailer()
        {
            Serilog.Log.Debug($"Dodanie dojazdu w programie {_model.NewProgramName}");
            if (_model.OrgClamping.Contains("Zabierak") || _model.OrgClamping.Contains("ZABIERAK"))
            {
                if (_model.MachineType != MachineEnum.HSTM1000 &&
                    _model.MachineType != MachineEnum.HSTM500)
                {
                    trailerLines.Add($"N14 G0 D0 G53 X-200 Y150 Z550");
                    trailerLines.Add($"N16 ;---- PODJAZD KONIKIEM DO NITA-----");
                    trailerLines.Add($"N17 ASTART:");
                    trailerLines.Add($"N18 STOPRE");
                    trailerLines.Add($"N19 E_ZDARZ=1 ; ACS - start obrobki");
                    trailerLines.Add($"N20 T_INI");
                    trailerLines.Add($"N21 T_START(1)");
                    trailerLines.Add($";");
                    trailerLines.Add("M38");
                    trailerLines.Add($"R61={pos_U}");
                    trailerLines.Add($";");
                    trailerLines.Add($"N22 G0 U=R61+10; dojazd przed klocek");
                    trailerLines.Add($"N23 M38");
                    trailerLines.Add($"N24 PEAKFW(1000)");
                    trailerLines.Add($"R99=$AA_IW[U]");
                    trailerLines.Add($"N25 PEAKBW");
                    trailerLines.Add($"N26 G1 U=R99+1 F150");
                    trailerLines.Add($"N27 PRESS_ON(900)");
                }
                if (_model.MachineType == MachineEnum.HSTM500)
                {
                    trailerLines.Add($";");
                    trailerLines.Add($"STOPRE");
                    trailerLines.Add($"R61={pos_U}");
                    trailerLines.Add($";");
                    trailerLines.Add($"N22 G0 U=R61+10; dojazd przed klocek");
                    trailerLines.Add($"N23 M38");
                    trailerLines.Add($"N24 PEAKFW(1000)");
                    trailerLines.Add($"R99=$AA_IW[U]");
                    trailerLines.Add($"N25 PEAKBW");
                    trailerLines.Add($"N26 G1 U=R99+1 F150");
                    trailerLines.Add($"N27 PRESS_ON(900)");
                    trailerLines.Add($";");
                    trailerLines.Add($"N18 T_INI");
                    trailerLines.Add($"N19 T_START(1)");
                    trailerLines.Add($"N20 E_ZDARZ=1; ACS - start obrobki");
                }
            }
            return trailerLines;
        }

        private void GetPositionU()
        {
            StreamReader file = new StreamReader(_model.ProgramName);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (_model.MachineType.ToString() == MachineEnum.HSTM300.ToString() ||
                    _model.MachineType.ToString() == MachineEnum.HSTM300HD.ToString() ||
                    _model.MachineType.ToString() == MachineEnum.HSTM500M.ToString())
                {
                    if (line.Contains(";SUMA="))
                    {
                        var posU = line.Replace("N14 ;SUMA=", "");
                        pos_U = Validation(posU);
                    }
                    if (line.Contains("R61="))
                    {
                        var posU = line.Replace("R61=", "");
                        pos_U = Validation(posU);
                    }
                }
                if (_model.MachineType.ToString() == MachineEnum.HSTM500.ToString())
                {
                    if (line.Contains("R61="))
                    {
                        var posU = line.Replace("R61=", "");
                        pos_U = Validation(posU);
                    }
                }                
            }
            file.Close();

            CalculatePositionU();
        }

        private void CalculatePositionU()
        {
            if (_model.MachineType.ToString() == MachineEnum.HSTM300.ToString() ||
                _model.MachineType.ToString() == MachineEnum.HSTM300HD.ToString() ||
                _model.MachineType.ToString() == MachineEnum.HSTM500M.ToString())
            {
                if (pos_U != 0.0)
                {
                    if (_model.OrgMachine == MachineEnum.HSTM500.ToString())
                    {
                        pos_U = (pos_U - 74.5);//TODO zmiana wartosci U gdy HSTM500 => HSTM300!!??
                        Serilog.Log.Information($"Zmiana pozycji U -> {String.Format("{0:0.0}", pos_U)}");
                    }
                    else
                    {
                        Serilog.Log.Information($"Z kopiowanie pozycji U -> {String.Format("{0:0.0}", pos_U)}");
                    }
                }
            }
            else if (_model.MachineType.ToString() == MachineEnum.HSTM500.ToString())
            {
                if (pos_U != 0.0)
                {
                    pos_U = (pos_U + 275);
                    Serilog.Log.Information($"Zmiana pozycji U -> {String.Format("{0:0.0}", pos_U)}");
                }
            }
            else
            {
                Serilog.Log.Warning("Uwaga! nie ustawiono pozycji U");
            }
        }

        private double Validation(string posU)
        {
            if (posU.Contains("+"))
                return double.Parse(posU.Split('+')[0]) + double.Parse(posU.Split('+')[1]);

            Char delimiter = 'U';
            String[] substrings = posU.Split(delimiter);
            int substringslength = substrings.Length;
            posU = "";
            for (int i = 0; i <= (substringslength - 1); i++)
            {
                posU += substrings[i];
            }
            double pos_U = 0.0;
            if (IsDigitsOnly(posU.Replace(".", "")))
            {
                pos_U = double.Parse(posU);
            }
            else
            {
                Serilog.Log.Error("Bledna wartosc pozycji U z czytana z programu, wpisz U recznie!!");
            }
            return pos_U;
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        private void AddSubPrograms(List<SubProgram> files)
        {
            foreach (var item in files)
            {
                var newSubrogramName = GetNewSubprogramName(item);
                subProgramLines.Add($"PROG_{newSubrogramName}:");
                subProgramLines.Add($"EXTCALL(\"{newSubrogramName}\")");
                subProgramLines.Add("; ---------------------------------------------");
            }
        }

        private List<string> AddHeader()
        {
            Serilog.Log.Debug($"Dodanie naglowka w programie {_model.NewProgramName}");
            if (_model.MachineType.ToString() == MachineEnum.HSTM300.ToString() ||
                _model.MachineType.ToString() == MachineEnum.HSTM300HD.ToString() ||
                _model.MachineType.ToString() == MachineEnum.HSTM500M.ToString())
            {
                headerLines.Add("; ACS_EXIST_FINISH_UNDERCUT = 0");
                headerLines.Add("; ACS_EXIST_FINISH_SLOT_DGA = 0");
                headerLines.Add("; ACS_EXIST_PREFINISH_AIRFOIL = 0");
                headerLines.Add("; -PROGRAM GLOWNY 02 Z PODAJNIKIEM      -");
                headerLines.Add($"; PROGRAM NEW NAME: {_model.NewProgramName}01.MPF");
                headerLines.Add($"; OLD ORDER: {_model.ProgramName}");
                if (_model.MachineType.ToString() == MachineEnum.HSTM300HD.ToString())
                {
                    headerLines.Add("; MACHINE: HSTM_300HD_SIM840D_Py");
                }
                else if (_model.MachineType.ToString() == MachineEnum.HSTM500M.ToString())
                {
                    headerLines.Add("; MACHINE: HSTM_500M_SIM840D_Py");
                }
                else
                {
                    headerLines.Add("; MACHINE: HSTM_300_SIM840D_Py");
                }
                headerLines.Add(";             : HSTM300 / 500 - HAMUEL: 5 - axis milling machine-SIM840D Traori PP_Script");
                headerLines.Add("; CONTROL: SINUMERIK 840D");
                headerLines.Add("; ----------------------------------------");
                headerLines.Add($"; TYP MOCOWANIA : {_model.OrgClamping}");
                headerLines.Add($";N11 WP_CH");
                headerLines.Add($";N12 WP_PREP");
            }
            else if (_model.MachineType.ToString() == MachineEnum.HSTM500.ToString())
            {
                headerLines.Add("; -PROGRAM GLOWNY -");
                headerLines.Add($"; PROGRAM NEW NAME: {_model.NewProgramName}01.MPF");
                headerLines.Add($"; OLD ORDER: {_model.ProgramName}");
                headerLines.Add("; MACHINE: HSTM_500_SIM840D_Py");
                headerLines.Add(";             : HSTM300 / 500 - HAMUEL: 5 - axis milling machine-SIM840D Traori PP_Script");
                headerLines.Add("; CONTROL: SINUMERIK 840D");
                headerLines.Add("; ----------------------------------------");
                headerLines.Add($"; TYP MOCOWANIA : {_model.OrgClamping}");
                if (_model.OrgClamping.Contains("Zabierak"))
                {
                    headerLines.Add("N10 PRESS_ON(800)");
                    headerLines.Add("N11 G0 G53 Z540 D0");
                    headerLines.Add("N12 G0 G53 X-200 Y150 A0 B0");
                }
            }
            else if (_model.MachineType.ToString() == MachineEnum.HX151.ToString())
            {
                headerLines.Add("; -PROGRAM GLOWNY -");
                headerLines.Add($"; PROGRAM NEW NAME: {_model.NewProgramName}01.MPF");
                headerLines.Add($"; OLD ORDER: {_model.ProgramName}");
                headerLines.Add("; MACHINE: SH_HX151_24_SIM840D");
                headerLines.Add(";             : STARRAG_HECKERT_5axis_finish_milling_machine");
                headerLines.Add("; CONTROL: SINUMERIK 840D");
                headerLines.Add("; ----------------------------------------");
            }
            else
            {
                headerLines.Add("; BRAK NAGLOWKA");
                Serilog.Log.Warning($"Brak naglowka!");
            }
            return headerLines;
        }

        private void CopyMainProgramTemplate()
        {
            _templateMainProgram = _model.TemplateMainProgram;

            if (File.Exists(_templateMainProgram))
            {
                var setNewProgramWithDir = GetNewMainProgram();
                File.Copy(_templateMainProgram, setNewProgramWithDir, true);
            }
            else
            {
                Serilog.Log.Warning("Brak szablonu programu glownego! Program glowny nie zrobiony");
            }
        }

        private string GetNewSubprogramName(SubProgram file)
        {
            var getProgramName = Path.GetFileNameWithoutExtension(_model.ProgramName);
            if (_model.ProgramName.Contains("01.MPF"))
            { 
                getProgramName = getProgramName.Remove(getProgramName.Length - 2);
            }
            var setNewProgram = file.SubProgramNameWithoutExtension;
            setNewProgram = setNewProgram.Replace(getProgramName, _model.NewProgramName);
            return setNewProgram;
        }

        private string GetNewMainProgram()
        {
            if (_model.ProgramName == null)
            {
                Log.Error($"Nie mozna stworzyc nowego programu!");
                return string.Empty;
            }
            var getProgramName = Path.GetFileNameWithoutExtension(_model.ProgramName);
            getProgramName = getProgramName.Remove(getProgramName.Length - 2);
            var setNewProgram = Path.GetFileName(_model.NewProgramName);
            setNewProgram = Path.Combine(_newDirectoryName, setNewProgram.Replace(getProgramName, _model.NewProgramName) + "01.MPF");
            return setNewProgram;
        }
    }
}
