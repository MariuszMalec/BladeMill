using BladeMill.BLL.Entities;

namespace BladeMill.BLL.Services
{
    public interface IConvertSubProgramsService
    {
        void CreateInfoFile(string mainProgram, string newProgramName);
        void FixSubPrograms();
        string GetNewSubprogramName(SubProgram file);
    }
}