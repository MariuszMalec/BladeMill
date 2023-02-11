using BladeMill.BLL.Services;
using Serilog;
using System.Linq;

namespace BladeMill.ConsoleApp.HelpProgramms
{
    public static class CheckAllProgamExe
    {
        public static void Test(ILogger logger)
        {
            logger.Information("View all programms");
            var programExeService = new ProgramExeService();
            programExeService.GetAll().ToList().ForEach(x => logger.Information($"{x.Id} {x.FullName}"));

            logger.Information("Checking if all exe files exist ... ");
            programExeService.GetAll().Where(x=>x.IsExist != true).ToList().ForEach(x=>logger.Error(x.FullName));
        }
    }
}
