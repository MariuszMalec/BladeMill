using BladeMillWithExcel.Logic.Models;
using System.Collections.Generic;

namespace BladeMillWithExcel.Logic.Services
{
    public interface IToolService
    {
        List<Tool> LoadToolsFromFile(string mainProgramFile);
    }
}