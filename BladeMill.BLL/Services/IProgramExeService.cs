using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using System.Collections.Generic;

namespace BladeMill.BLL.Services
{
    public interface IProgramExeService
    {
        void CheckProcess(string file);
        IEnumerable<ProgramExe> GetAll();
        IEnumerable<string> GetFullName();
        string GetFullNameById(int id);
        List<ExeEnum> GetListEnemExe();
        void StartNewProcess(string programWithFullName);

        ProgramExe GetProgramExeById(int id);
    }
}