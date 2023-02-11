using BladeMill.BLL.Enums;
using System;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Maszyna jako wzorzec fabryka
    /// </summary>
    public class MachineServiceFactory
    {
        public MachineSettings CreateMachine(TypeOfFile type)
        {
            switch (type)
            {
                case TypeOfFile.ncFile:
                    return new GetMachineFromNc();
                case TypeOfFile.toolsXmlFile:
                    return new GetMachineFromToolsXml();
                case TypeOfFile.varpoolFile:
                    return new GetMachineFromVarpool();
                default:
                    throw new Exception($"Shape type {type} is not support");
            }
        }
    }
}
