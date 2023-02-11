using BladeMill.BLL.Entities;
using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Do przerobki kodu Nc
    /// </summary>
    public class ConvertSettingsService
    {
        private readonly ConvertMainProgram _convertMainProgram;
        private readonly PathDataBase _pathData;
        public ConvertSettingsService()
        {
            _convertMainProgram = new ConvertMainProgram();
            _pathData = new PathDataBase();
        }
        public ConvertMainProgram SetConvertParameters(string machine, string mainProgram, string newProgramName)
        {
            var machineServiceFactory = new MachineServiceFactory();
            var orgMachine = machineServiceFactory.CreateMachine(TypeOfFile.ncFile).GetMachine(mainProgram).MachineName;
            _convertMainProgram.OrgMachine = orgMachine;
            _convertMainProgram.ProgramName = mainProgram;
            _convertMainProgram.NewProgramName = newProgramName;

            if (machine == MachineEnum.HSTM500.ToString())
            {
                _convertMainProgram.MachineType = MachineEnum.HSTM500;
                _convertMainProgram.AddPreload = true;
                _convertMainProgram.DeletePreload = false;
                _convertMainProgram.DeleteRaport = true;
                _convertMainProgram.AddRaport = false;
                _convertMainProgram.ReplaceCycleTool = false;
                _convertMainProgram.ReplaceToolCycle = true;
                _convertMainProgram.TemplateMainProgram = _pathData.GetFileMainProgramTemplate(MachineEnum.HSTM500.ToString());
            }
            else if (machine == MachineEnum.HSTM300HD.ToString())
            {
                _convertMainProgram.MachineType = MachineEnum.HSTM300HD;
                _convertMainProgram.AddPreload = true;
                _convertMainProgram.DeletePreload = false;
                _convertMainProgram.DeleteRaport = true;
                _convertMainProgram.AddRaport = false;
                _convertMainProgram.ReplaceCycleTool = false;
                _convertMainProgram.ReplaceToolCycle = true;
                _convertMainProgram.TemplateMainProgram = _pathData.GetFileMainProgramTemplate(MachineEnum.HSTM300HD.ToString());
            }
            else if (machine == MachineEnum.HX151.ToString())
            {
                _convertMainProgram.MachineType = MachineEnum.HX151;
                _convertMainProgram.AddPreload = true;
                _convertMainProgram.DeletePreload = false;
                _convertMainProgram.DeleteRaport = true;
                _convertMainProgram.AddRaport = false;
                _convertMainProgram.ReplaceCycleTool = false;
                _convertMainProgram.ReplaceToolCycle = false;
                _convertMainProgram.TemplateMainProgram = _pathData.GetFileMainProgramTemplate(MachineEnum.HX151.ToString()); ;
            }
            else if (machine == MachineEnum.HSTM300.ToString())
            {
                _convertMainProgram.MachineType = MachineEnum.HSTM300;
                _convertMainProgram.AddPreload = false;
                _convertMainProgram.DeletePreload = true;
                _convertMainProgram.DeleteRaport = false;
                _convertMainProgram.AddRaport = false;
                _convertMainProgram.ReplaceCycleTool = true;
                _convertMainProgram.ReplaceToolCycle = false;
                _convertMainProgram.TemplateMainProgram = _pathData.GetFileMainProgramTemplate(MachineEnum.HSTM300.ToString());
            }
            else if (machine == MachineEnum.HSTM500M.ToString())
            {
                _convertMainProgram.MachineType = MachineEnum.HSTM500M;
                _convertMainProgram.AddPreload = false;
                _convertMainProgram.DeletePreload = true;
                _convertMainProgram.DeleteRaport = true;
                _convertMainProgram.AddRaport = false;
                _convertMainProgram.ReplaceCycleTool = true;
                _convertMainProgram.ReplaceToolCycle = false;
                _convertMainProgram.TemplateMainProgram = _pathData.GetFileMainProgramTemplate(MachineEnum.HSTM500M.ToString());
            }
            else if (machine == MachineEnum.HSTM1000.ToString())
            {
                _convertMainProgram.MachineType = MachineEnum.HSTM1000;
                _convertMainProgram.AddPreload = false;
                _convertMainProgram.DeletePreload = true;
                _convertMainProgram.DeleteRaport = true;
                _convertMainProgram.AddRaport = false;
                _convertMainProgram.ReplaceCycleTool = true;
                _convertMainProgram.ReplaceToolCycle = false;
                _convertMainProgram.TemplateMainProgram = _pathData.GetFileMainProgramTemplate(MachineEnum.HSTM1000.ToString());
            }
            else
            {
                Serilog.Log.Error($"No Suport this machine {machine}");
                return new ConvertMainProgram();
            }

            if ((MachineEnum.HX151.ToString() == _convertMainProgram.MachineType.ToString() &&
                _convertMainProgram.OrgMachine.ToString() != MachineEnum.HX151.ToString()) ||
                (MachineEnum.HX151.ToString() == _convertMainProgram.OrgMachine.ToString() &&
                _convertMainProgram.MachineType.ToString() != MachineEnum.HX151.ToString()))
            {
                Serilog.Log.Error("Programu nie przerobiono, inna kinematyka maszyny");
                return new ConvertMainProgram();
            }

            return _convertMainProgram;
        }
        public ConvertMainProgram GetByMainProgram(string id)
        {
            var machineServiceFactory = new MachineServiceFactory();
            var orgMachine = machineServiceFactory.CreateMachine(TypeOfFile.ncFile).GetMachine(id).MachineName;
            _convertMainProgram.OrgMachine = orgMachine;
            _convertMainProgram.ProgramName = id;
            _convertMainProgram.NewProgramName = "test";//default value
            _convertMainProgram.MachineType = MachineEnum.HSTM500;//default value
            return _convertMainProgram;
        }
    }
}
