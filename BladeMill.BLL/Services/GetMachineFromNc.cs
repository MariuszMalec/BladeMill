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
    /// Maszyna jako wzorzec fabryka
    /// </summary>
    public class GetMachineFromNc : MachineSettings
    {
        public override Machine GetMachine(string file)
        {
            var machine = string.Empty;
            if (File.Exists(file) && (file.Contains(".MPF") || file.Contains(".NC") || file.Contains(".nc") ||
                    file.Contains(".spf") || file.Contains(".SPF") || file.Contains(".mpf")))
            {
                var nc = GetNcLinesFromNC(file);
                //check if not null
                var validMACHINE = nc.Where(n => n.Line.Contains("MACHINE"))
                    .Select(n => n.Line).FirstOrDefault();
                if (validMACHINE != null)
                {
                    machine = validMACHINE.ToString().Split(':')[1].Replace(" ", "");
                    return new Machine() { Id = 1, Created = DateTime.Now, MachineName = GetShortName(machine), MachineControl = GetMachineControl(machine), MachineVericutTemplate = "" };
                }
                var validOBRABIARKA = nc.Where(n => n.Line.Contains("OBRABIARKA"))
                                     .Select(n => n.Line).FirstOrDefault();
                if (validOBRABIARKA != null)
                {
                    machine = validOBRABIARKA.ToString().Split(':')[1].Replace(" ", "");
                    return new Machine() { Id = 1, Created = DateTime.Now, MachineName = GetShortName(machine), MachineControl = GetMachineControl(machine), MachineVericutTemplate = "" };
                }
                //Hec
                var validHec = nc.Where(n => n.Line.Contains("L300"))
                    .Select(n => n.Line).FirstOrDefault();
                if (validHec != null)
                {
                    machine = MachineEnum.HEC.ToString();
                    return new Machine() { Id = 1, Created = DateTime.Now, MachineName = machine, MachineControl = GetMachineControl(machine), MachineVericutTemplate = "" };
                }
                //Avia
                var validAvia = nc.Where(n => n.Line.Contains("CYCLE800") || n.Line.Contains("PODPROGRAMIK"))
                    .Select(n => n.Line).FirstOrDefault();
                if (validAvia != null)
                {
                    machine = MachineEnum.AVIA.ToString();
                    return new Machine() { Id = 1, Created = DateTime.Now, MachineName = machine, MachineControl = GetMachineControl(machine), MachineVericutTemplate = "" };
                }
                //special if subprogram 61
                var validCycle60 = nc.Where(n => n.Line.Contains("CYCLE60"))
                    .Select(n => n.Line).FirstOrDefault();
                if (validCycle60 != null)
                {
                    machine = MachineEnum.HSTM300.ToString();
                    return new Machine() { Id = 1, Created = DateTime.Now, MachineName = machine, MachineControl = GetMachineControl(machine), MachineVericutTemplate = "" };
                }
            }
            if (machine == string.Empty)
                Log.Error($"The machine is null! Brak naglowka z nazwa maszyny! {file}");
            return new Machine() { Id=99, Created=DateTime.Now, MachineName="Brak", MachineControl="Brak", MachineVericutTemplate="Brak"};
        }

        public List<string> GetAllMachines()
        {
            var list = new List<string>();
            GetListMachineEnum().ForEach(machine => list.Add(machine.ToString()));
            return list;
        }

        private List<MachineEnum> GetListMachineEnum()
        {
            var enumList = new List<MachineEnum>();
            var enumMemberCount = Enum.GetNames(typeof(MachineEnum)).Length;
            if (enumMemberCount > 0)
            {
                enumList = Enum.GetValues(typeof(MachineEnum)).OfType<MachineEnum>().ToList();
            }
            return enumList;
        }
        private List<LineFromFile> GetNcLinesFromNC(string file)
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
        private string GetMachineControl(string machine)
        {
            var control = "unknow";
            if (!string.IsNullOrEmpty(machine))
            {
                if (machine.Contains("SIM840D"))
                    return "SIM840D";
            }
            return control;
        }

        private string GetShortName(string machine)
        {
            switch (machine.ToUpper())
            {
                case ("HSTM_300_SIM840D_PY"):
                    machine = MachineEnum.HSTM300.ToString();
                    break;
                case ("SH_HX151_24_SIM840D"):
                    machine = MachineEnum.HX151.ToString();
                    break;
                case ("HSTM_500M_SIM840D_PY"):
                    machine = MachineEnum.HSTM500M.ToString();
                    break;
                case ("HURON_EX20_SIM840D"):
                    machine = MachineEnum.HURON.ToString();
                    break;
                case ("HSTM_1000_SIM840D_PY"):
                    machine = MachineEnum.HSTM1000.ToString();
                    break;
                case ("HSTM_300HD_SIM840D_PY"):
                    machine = MachineEnum.HSTM300HD.ToString();
                    break;
                case ("HSTM_500_SIM840D_PY"):
                    machine = MachineEnum.HSTM500.ToString();
                    break;
                default:
                    machine = "unkown";
                    break;
            }
            return machine;
        }
    }
}
