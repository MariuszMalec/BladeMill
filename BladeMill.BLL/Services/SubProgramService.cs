using BladeMill.BLL.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Serwis dla programow pomocniczych
    /// </summary>
    public class SubProgramService
    {
        private string _mainProgram;
        private int _count;
        private IEnumerable<SubProgram> _subPrograms = new List<SubProgram>() { };

        public SubProgramService(string mainProgram)
        {
            _mainProgram = mainProgram;
        }

        public IEnumerable<SubProgram> GetSubprogramsListFromNc()
        {
            var list = new List<SubProgram>();
            if (File.Exists(_mainProgram))
            {
                string[] lines = File.ReadAllLines(_mainProgram);
                foreach (string line in lines)
                {
                    if (line.Contains("EXTCALL"))//only HSTMs
                    {
                        list.Add(GetSubprogramAsExtcall(line));
                    }
                }
                return list;
            }
            return list;
        }
        private SubProgram GetSubprogramAsExtcall(string line)
        {
            char[] delimiterChars = { '(', ')' }; var id = 0; _count++;
            string[] NCProgram = line.Split(delimiterChars);
            NCProgram = NCProgram[1].Split('"');
            string ncFile = Path.Combine(Path.GetDirectoryName(_mainProgram), NCProgram[1] + ".SPF");
            return new SubProgram()
            {
                Id = _count,
                Created = DateTime.Now,
                SubProgramNameWithDir = ncFile,
            };
        }
    }
}
