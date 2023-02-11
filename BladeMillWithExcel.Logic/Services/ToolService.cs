using BladeMillWithExcel.Logic.Models;
using System.Collections.Generic;

namespace BladeMillWithExcel.Logic.Services
{
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
    }
}
