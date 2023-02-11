using BladeMill.BLL.Models;
using System.Collections.Generic;

namespace BladeMill.BLL.Interfaces
{
    public interface IToolService
    {
        List<Tool> LoadToolsFromFile(string file);
        string GetMachineFromFile(string file);

        List<Tool> GetAllToolsFromCurrentXml();
    }
}
