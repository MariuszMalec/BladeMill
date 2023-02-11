using BladeMill.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BladeMill.BLL.Services
{
    public class MachineService
    {
        public string GetNcMachineFromNC(string mainProgram)
        {
            var ncService = new ToolNcService();
            var incService = new ToolService(ncService);
            return incService.GetMachineFromFile(mainProgram);
        }
        public string GetNcMachineFromXmlFile(string toolsXml)
        {
            var xmlService = new ToolXmlService();
            var ixmlService = new ToolService(xmlService);
            return ixmlService.GetMachineFromFile(toolsXml);
        }
        public List<MachineEnum> GetListMachineEnum()
        {
            var enumList = new List<MachineEnum>();
            var enumMemberCount = Enum.GetNames(typeof(MachineEnum)).Length;
            if (enumMemberCount > 0)
            {
                enumList = Enum.GetValues(typeof(MachineEnum)).OfType<MachineEnum>().ToList();
            }
            return enumList;
        }

        public string GetSimpleMachineName(string machineName)
        {
            var machine = string.Empty;
            machine = SimplyMachineName(machineName);
            return machine;
        }
        private string SimplyMachineName(string machine)
        {
            switch (machine)
            {
                case ("HSTM_300_SIM840D_Py"):
                    machine = MachineEnum.HSTM300.ToString();
                    break;
                case ("SH_HX151_24_SIM840D"):
                    machine = MachineEnum.HX151.ToString();
                    break;
                case ("HSTM_500M_SIM840D_Py"):
                    machine = MachineEnum.HSTM500M.ToString();
                    break;
                case ("HURON_EX20_SIM840D"):
                    machine = MachineEnum.HURON.ToString();
                    break;
                case ("HSTM_1000_SIM840D_Py"):
                    machine = MachineEnum.HSTM1000.ToString();
                    break;
                case ("HSTM_300HD_SIM840D_Py"):
                    machine = MachineEnum.HSTM300HD.ToString();
                    break;
                case ("HSTM_500_SIM840D_Py"):
                    machine = MachineEnum.HSTM500.ToString();
                    break;
                default:
                    machine = string.Empty;
                    break;
            }
            return machine;
        }

        public List<string> GetListMachines()
        {
            var list = new List<string>();
            GetListMachineEnum().ForEach(machine => list.Add(machine.ToString()));
            return list;
        }

    }
}
