using BladeMill.BLL.Entities;
using BladeMill.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Praca z plikami
    /// </summary>
    public class FileService
    {
        private static List<NcMainProgram> _ncMainPrograms = new List<NcMainProgram>();
        private string _testDir = @"C:\Users\212517683\source\repos\BladeMill\UnitTests\SourceData";
        private int _count;
        private IEnumerable<SubProgram> _subPrograms = new List<SubProgram>() { };
        public List<LineFromFile> GetLinesFromFile(string file)
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
        public IEnumerable<SubProgram> GetSubprogramsListFromNc(string mainProgram)
        {
            var list = new List<SubProgram>() { };
            if (File.Exists(mainProgram))
            {
                string[] lines = File.ReadAllLines(mainProgram);
                //Avia
                var getProgramiki = lines.Where(n => n.Contains("PODPROGRAMIK")).Select(n => n);
                getProgramiki.ToList().ForEach(p=> list.Add(GetSubprogramAsProgramik(mainProgram, p)));
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
                    list.Add(new SubProgram() { Id=1, Created =DateTime.Now, SubProgramNameWithDir = mainProgram});
                }
            }
            return list;
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
        public List<SelectedFile> GetListSelectedFiles(string dir, string extention)
        {
            if (Directory.Exists(dir) == false)
            {
                return new List<SelectedFile>();
            }
            var tmpList = new List<SelectedFile>();
            string[] files = Directory.GetFiles(dir);
            int count = 0;
            foreach (var file in files)
            {
                if (file.Contains(extention) && !file.Contains("_COPY") && !file.Contains("_01_"))
                {
                    count++;
                    tmpList.Add(new SelectedFile(count, file));
                }
            }
            return tmpList;
        }

        public List<SelectedFile> GetSubprogramsListFromNcAsIEnumerable(string mainProgram)
        {
            var list = new List<SelectedFile>() { };
            if (File.Exists(mainProgram))
            {
                string[] lines = File.ReadAllLines(mainProgram);
                foreach (string line in lines)
                {
                    if (line.Contains("EXTCALL"))//only HSTMs and HX
                    {
                        GetSubprogramAsExtcall(mainProgram, list, line);
                    }
                }
            }
            return list;
        }

        private void GetSubprogramAsExtcall(string file, List<SelectedFile> list, string line)
        {
            char[] delimiterChars = { '(', ')' };
            string[] NCProgram = line.Split(delimiterChars);
            NCProgram = NCProgram[1].Split('"');
            string ncFile = Path.Combine(Path.GetDirectoryName(file), NCProgram[1] + ".SPF");
            _count++;
            list.Add(new SelectedFile() { Id = _count, BatchFile = ncFile});
        }

        public List<SelectedFile> GetListFilesWithName(string dir, string extention, string name)
        {
            if (Directory.Exists(dir) == false)
            {
                return new List<SelectedFile>();
            }
            var tmpList = new List<SelectedFile>();
            string[] files = Directory.GetFiles(dir);
            int count = 0;
            foreach (var file in files)
            {
                if (file.Contains(extention) && file.Contains(name) && !file.Contains("_COPY") && !file.Contains("_01_"))
                {
                    count++;
                    tmpList.Add(new SelectedFile(count, file));
                }
            }
            return tmpList;
        }

        public List<NcMainProgram> GetMainProgromFromDir(string dir, string extention)
        {
            if (Directory.Exists(dir) == false)
            {
                return new List<NcMainProgram>();
            }
            var _ncMainPrograms = new List<NcMainProgram>();
            string[] files = Directory.GetFiles(dir);
            int count = 0;
            foreach (var file in files)
            {
                if (file.Contains(extention))
                {
                    count++;
                    _ncMainPrograms.Add(new NcMainProgram() { Id = count, MainProgram = file });
                }
            }
            return _ncMainPrograms;
        }
        /// <summary>
        /// Dla Web App
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NcMainProgram> GetAll()
        {
            var exe = "01.MPF";
            var dir = @"C:\tempNC";
            _ncMainPrograms = GetMainProgromFromDir(dir, exe);
            if (_ncMainPrograms.Count() == 0)
            {
                _ncMainPrograms = GetMainProgromFromDir(_testDir, exe);
            }
            return _ncMainPrograms;
        }
        public NcMainProgram GetById(int id)
        {
            return _ncMainPrograms.SingleOrDefault(m => m.Id == id);
        }

        public void Delete(string file)
        {
            var path = file;
            try
            {
                File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {
                FileAttributes attributes = File.GetAttributes(path);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    attributes &= ~FileAttributes.ReadOnly;
                    File.SetAttributes(path, attributes);
                    File.Delete(path);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}