using BladeMill.BLL.DatatBaseAcess;
using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Programy pomocnicze
    /// </summary>
    public class ProgramExeService : IProgramExeService
    {
        private PathDataBase _pathService;
        private ILogger _logger;
        private static IEnumerable<ProgramExe> _programs = new List<ProgramExe>();

        public ProgramExeService()
        {
            _logger = Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj} {Properties:j}{NewLine}{Exception}").MinimumLevel.Debug()
                .WriteTo.File(@"C:\temp\ProgramExeService.log")
                .CreateLogger();
            _pathService = new PathDataBase();
        }

        public IEnumerable<ProgramExe> GetAll()
        {
            _programs =  HelpProgramsStorage.programs;
            return _programs;
        }

        public IEnumerable<string> GetFullName()
        {
            var models = GetAll();
            if (models.Any())
            {
                var newList = new List<string>();
                foreach (var item in models)
                {
                    newList.Add(item.FullName);
                }
                return newList;
            }
            return new List<string>();
        }

        public string GetFullNameById(int id)
        {
            return _programs.Where(u => u.Id == id).Select(u => u.FullName).FirstOrDefault().ToString();
        }

        public ProgramExe GetProgramExeById(int id)
        {
            return _programs.Where(u => u.Id == id).Select(u => u).FirstOrDefault();
        }

        public void StartNewProcess(string programWithFullName)
        {
            CheckProcess(programWithFullName);
        }
        public void CheckProcess(string file)//static(return) //void (no return)
        {
            if (File.Exists(file))
            {
                if (!file.Contains(".xlsm") && !file.Contains(".html"))
                {
                    Process.Start(file);
                }
                else//open excel with core5.0
                {
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(file)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
            }
            else
            {
                _logger.Warning($"Brak programu {file} , ZGLOS SIE DO MARIUSZA!");
            }
        }
        public List<ExeEnum> GetListEnemExe()
        {
            var enumList = new List<ExeEnum>();
            var enumMemberCount = Enum.GetNames(typeof(ExeEnum)).Length;
            if (enumMemberCount > 0)
            {
                enumList = Enum.GetValues(typeof(ExeEnum)).OfType<ExeEnum>().ToList();
            }
            return enumList;
        }
    }
}
