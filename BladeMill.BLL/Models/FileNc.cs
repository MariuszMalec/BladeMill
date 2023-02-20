using BladeMill.BLL.Entities;
using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMill.BLL.Models
{
    public class FileNc : Entity
    {
        public string? Name
        {
            get
            {
                return Path.GetFileName(NameWithDir);
            }
            private set
            {
                Name = Path.GetFileName(NameWithDir);
            }
        }

        public string? NameWithDirWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(NameWithDir);
            }
            private set
            {
                NameWithDirWithoutExtension = Path.GetFileNameWithoutExtension(NameWithDir);
            }
        }

        public string? NameWithDir { get; set; }

        public bool IsMainProgram
        {
            get
            {
                return IsItMainProgram(NameWithDir);
            }
            private set
            {
                IsMainProgram = IsItMainProgram(NameWithDir);
            }
        }

        public bool IsExist
        {
            get
            {
                return File.Exists(NameWithDir);
            }
            private set
            {
                IsExist = File.Exists(NameWithDir);
            }
        }

        public List<NcLine>? Lines
        {
            get
            {
                return GetNcLinesFromNC(NameWithDir);
            }
            private set
            {
                Lines = GetNcLinesFromNC(NameWithDir);
            }
        }

        public Machine Machine
        {
            get
            {
                return GetMachine(NameWithDir);
            }
            private set
            {
                Machine = GetMachine(NameWithDir);
            }
        }

        private Machine GetMachine(string nameWithDir)
        {
            var machineServiceFactory = new MachineServiceFactory();
            return machineServiceFactory.CreateMachine(TypeOfFile.ncFile).GetMachine(nameWithDir);
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
                        ncinfo.Add(new NcLine() { NmbLine = lineNb, Line = line });
                        lineNb++;
                    }
                }
            }
            return ncinfo;
        }

        private bool IsItMainProgram(string file)
        {
            if (File.Exists(file))
            {
                ToolNcService ncService = new ToolNcService();
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
    }
}
