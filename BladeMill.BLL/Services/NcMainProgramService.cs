using BladeMill.BLL.Entities;
using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Serwis dla programu glownego
    /// </summary>
    public class NcMainProgramService
    {
        private static List<NcMainProgram> _ncMainProgramFromDir = new List<NcMainProgram>();
        public static int IdMainProgram = 0;
        private string _mainProgram;

        public NcMainProgramService()
        {

        }
        public NcMainProgramService(string mainProgram)
        {
            _mainProgram = mainProgram;
        }
        public NcMainProgram GetMainProgram()
        {
            var model = new NcMainProgram() { Id = 1, MainProgram = _mainProgram };
            if (model == null)
                throw new System.Exception($"Uwaga sprawdz model NcMainProgram, funkcje GetName!");
            return model;
        }
        public string GetMainProgramWithDir()
        {
            if (!File.Exists(_mainProgram))
            {
                Serilog.Log.Warning($"Brak programu glownego {_mainProgram}");
            }
            return _mainProgram;
        }

        public string GetMachine()
        {
            var machine = string.Empty;
            var nc = GetNcLinesFromNC(_mainProgram);
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
            if (machine == string.Empty)
            {
                Serilog.Log.Warning("not get machine from main program!");
            }
            return machine;
        }

        private List<NcLine> GetNcLinesFromNC(string file)
        {
            var ncinfo = new List<NcLine>();
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
                        ncinfo.Add(new NcLine() { NmbLine = lineNb, Line = line });
                        lineNb++;
                    }
                }
            }
            return ncinfo;
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

        public List<NcMainProgram> GetListMainPrograms(string dir, string extention)
        {
            if (Directory.Exists(dir) == false)
            {
                return new List<NcMainProgram>();
            }
            var mainProgramList = new List<NcMainProgram>();
            string[] files = Directory.GetFiles(dir);
            int count = 0;

            var machineFactory = new MachineServiceFactory();
            var machine = machineFactory.CreateMachine(TypeOfFile.ncFile);

            foreach (var file in files)
            {
                if (file.Contains(extention))
                {
                    count++;
                    mainProgramList.Add(new NcMainProgram() 
                    { 
                        Id = count, 
                        MainProgram = file, 
                        Machine = machine.GetMachine(file).MachineName,
                        Clamping = GetClamping(file),                        
                    });
                }
            }
            return mainProgramList;
        }

        private string GetClamping(string file)
        {
            var lines = GetNcLinesFromNC(file);
            if (lines.Any(l=>l.Line.Contains("TYP MOCOWANIA")))
            {
                return lines.Where(l => l.Line.Contains("TYP MOCOWANIA"))
                            .Select(l => l.Line.Split(':')[1].Replace(" ",""))
                            .FirstOrDefault();
            }
            return "Brak";
        }

        public int GetIdMainProgram()
        {
            return _ncMainProgramFromDir.Where(n => n.Id == IdMainProgram).Select(m => m.Id).FirstOrDefault();
        }

        //Dla Web
        public string GetMainProgramMvc()
        {
            return _ncMainProgramFromDir.Where(n => n.Id == IdMainProgram).Select(m => m.MainProgram).FirstOrDefault();
        }
        public List<NcMainProgram> GetAll()
        {
            _ncMainProgramFromDir = GetListMainPrograms(@"C:\tempNC", "01.MPF");
            return _ncMainProgramFromDir;
        }
        public NcMainProgram GetById(int id)
        {
            IdMainProgram = id;//tutaj przypisuje Id ktore czyta ToolNcControler
            return _ncMainProgramFromDir.SingleOrDefault(m => m.Id == id);
        }
    }
}
