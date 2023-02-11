using BladeMill.BLL.Enums;
using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Services;
using Serilog;
using System;
using System.IO;

namespace BladeMill.BLL.Validators
{
    /// <summary>
    /// Walidacja
    /// </summary>
    public class ValidateConvertMainProgram : IValidation
    {
        private readonly ILogger _logger;
        /// <summary>
        /// Uzywam tutaj seriloga!
        /// </summary>
        /// <param name="logger"></param>
        public ValidateConvertMainProgram(ILogger logger)
        {
            _logger = logger;
        }
        public bool ValidationMachine(string selectedMachine, string mainProgram)
        {
            if (string.IsNullOrEmpty(selectedMachine))
            {
                _logger.Error($"Nazwa maszyny nie moze byc pusta");
                return false;
            }
            if (string.IsNullOrEmpty(mainProgram))
            {
                _logger.Error($"Nazwa programu nie moze byc pusta");
                return false;
            }
            if (!File.Exists(mainProgram))
            {
                _logger.Error($"Brak programu {mainProgram}");
                return false;
            }
            var machineFactory = new MachineServiceFactory();
            var machine = machineFactory.CreateMachine(TypeOfFile.ncFile).GetMachine(mainProgram).MachineName;

            if (machine == null)
            {
                _logger.Error($"Maszyna nie moze by null");
                return false;
            }

            if ((machine == "HX151" && selectedMachine != "HX151") ||
                (machine != "HX151" && selectedMachine == "HX151"))
            {
                _logger.Error($"Wybrano bledny program NC dla maszyny {selectedMachine}");
                return false;
            }
            return true;
        }
        public bool ValidationNewNameProgram(string newNameProgram)
        {
            if (string.IsNullOrEmpty(newNameProgram))
            {
                _logger.Error($"Nazwa nowego programu nie moze byc pusta");
                return false;
            }
            if (newNameProgram.Length != 6 && newNameProgram.Length != 5)
            {
                _logger.Error($"Nieprawidlowa ilosc znakow w nazwie programu");
                return false;
            }
            return true;
        }
        public bool ValidationMainProgram(string mainProgram)
        {
            if (string.IsNullOrEmpty(mainProgram))
            {
                _logger.Error($"Nazwa programu nie moze byc pusta");
                return false;
            }
            if (!mainProgram.ToUpper().Contains(".MPF") && !mainProgram.ToUpper().Contains(".NC"))
            {
                _logger.Error($"Brak w nazwie programu ..MPF");
                return false;
            }
            if (!File.Exists(mainProgram))
            {
                _logger.Error($"Brak glownego programu {mainProgram}");
                return false;
            }
            return true;
        }
    }
}
