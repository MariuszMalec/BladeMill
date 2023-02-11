using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using BladeMill.BLL.Validators;
using Serilog;

namespace BladeMill.ConsoleApp.ConvertMainNc
{
    public static class ConvertMainProgram
    {
        public static void Tests(ILogger logger, string mainProgram, string newProgramName, MachineEnum enumMachine)
        {            
            var machineServiceFactory = new MachineServiceFactory();
            var orgMachine = machineServiceFactory.CreateMachine(TypeOfFile.ncFile).GetMachine(mainProgram).MachineName;
            var newMachine = enumMachine.ToString();

            logger.Debug($"OrgProgram = {mainProgram}");
            logger.Debug($"OrgMachine = {orgMachine}");
            logger.Debug($"newMachine = {newMachine}");

            //Validation
            var validation = new ValidateConvertMainProgram(logger);
            var isMainProgramCorrect = validation.ValidationMainProgram(mainProgram);
            var isMainNewNameProgramCorrect = validation.ValidationNewNameProgram(newProgramName);
            var isMachineCorrect = validation.ValidationMachine(newMachine, mainProgram);//TODO poprawic przechodzi gdy np.HST30!!!!
            var isOrgMachineCorrect = validation.ValidationMachine(orgMachine, mainProgram);

            if (isMainProgramCorrect == true &&
                isMachineCorrect == true &&
                isMainNewNameProgramCorrect == true &&
                isOrgMachineCorrect == true)
            {
                var convertSettings = new ConvertSettingsService();
                var settings = convertSettings.SetConvertParameters(newMachine, mainProgram, newProgramName);

                //add prefix to newProgram
                settings.NewProgramName = settings.Prefix + newProgramName;

                if (settings.NewProgramName != null)
                {
                    var convertSubService = new ConvertSubProgramsService(settings);
                    convertSubService.FixSubPrograms();

                    if (settings.OrgClamping.Contains("Zabierak") || settings.OrgClamping.Contains("ZABIERAK"))
                    {
                        var convertMainProgramService = new ConvertMainProgramService(settings);
                        convertMainProgramService.FixMainProgram();
                    }
                    else
                    {
                        logger.Warning($"Nie wykonano programu glownego, ze wzgledu na brak szablonu na mocowanie {settings.OrgClamping}");
                    }

                    convertSubService.CreateInfoFile(mainProgram, settings.NewProgramName);
                }
            }
            else
            {
                logger.Error($"Program nie przerobiono");
            }
        }
    }
}
