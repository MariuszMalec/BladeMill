using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System;
using System.Linq;
using System.Net.Http.Headers;

namespace BladeMill.BLL.Entities
{
    public class ConvertMainProgram
    {
        public string OrgMachine { get; set; }
        public MachineEnum MachineType { get; set; }
        public string ProgramName { get; set; }
        public string NewProgramName { get; set; }
        public bool DeleteRaport { get; set; }
        public bool AddRaport { get; set; }
        public bool ReplaceToolCycle { get; set; }
        public bool ReplaceCycleTool { get; set; }
        public bool AddPreload { get; set; }
        public bool DeletePreload { get; set; }
        public bool ChangeNameProgram { get; set; }
        public string TemplateMainProgram { get; set; }
        public string Prefix
        {
            get
            {
                return GetPrefix(MachineType);
            }
            private set
            {
                Prefix =  GetPrefix(MachineType);
            }
        }

        //TODO dodac oryginal clamping here
        public string OrgClamping
        {
            get
            {
                return GetClamping(ProgramName);
            }
            private set
            {
                Prefix = GetClamping(ProgramName);
            }
        }

        private static string GetPrefix(MachineEnum machineEnum)
        {
            if (MachineEnum.HSTM300 == machineEnum)
            {
                return "A";
            }
            if (MachineEnum.HSTM300HD == machineEnum)
            {
                return "D";
            }
            if (MachineEnum.HSTM500 == machineEnum)
            {
                return "B";
            }
            if (MachineEnum.HSTM500M == machineEnum)
            {
                return "C";
            }
            return "";
        }

        private static string GetClamping(string file)
        {
            var fileService = new FileService();
            var lines = fileService.GetLinesFromFile(file);
            if (lines.Any(l => l.Line.Contains("TYP MOCOWANIA")))
            {
                return lines.Where(l => l.Line.Contains("TYP MOCOWANIA"))
                            .Select(l => l.Line.Split(':')[1].Replace(" ", ""))
                            .FirstOrDefault();
            }
            return "Brak";
        }
    }
}
