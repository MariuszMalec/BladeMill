using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Models;
using System.Collections.Generic;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Serwis narzedzi z wstrzykiwaniem interfejsu
    /// </summary>
    public class ToolService
    {
        private readonly IToolService _toolService;

        public ToolService(IToolService toolService)
        {
            _toolService = toolService;
        }
        public List<Tool> LoadToolsFromFile(string file)
        {
            return _toolService.LoadToolsFromFile(file);
        }

        public string GetMachineFromFile(string file)
        {
            return _toolService.GetMachineFromFile(file);
        }

        public List<Tool> GetAllToolsFromCurrentXml()
        {
            return _toolService.GetAllToolsFromCurrentXml();
        }
    }
}
