using BladeMill.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.ApplicationXmlConfTest
{
    public class AppXmlConfFilesModelTests
    {
        private static string _applicationXmlFile = Path.Combine(@"C:\Users", 
                                         Environment.UserName, 
                                         @"source\repos\BladeMill\UnitTests\SourceData", "Application.xml.conf");

        private static string _toolList = Path.Combine(@"C:\Clever\V300\BladeMill\BladeMillServer\AppData\Tools_elb_V300.cle");
        private static string _machineList = Path.Combine(@"C:\Clever\V300\BladeMill\BladeMillServer\AppData\Machines_elb_V330.cle");
        private static string _app_retList = Path.Combine(@"C:\Clever\V300\BladeMill\BladeMillServer\AppData\AppRets_elb_V300.cle");
        private static string _mfgprocessList = Path.Combine(@"C:\Clever\V300\BladeMill\BladeMillServer\AppData\Default_MfgProcesses.cle");
        private static string _technologyList = Path.Combine(@"C:\Clever\V300\BladeMill\BladeMillServer\AppData\Technologies_elb_V300.cle");
        private static string _measuringList = Path.Combine(@"C:\Clever\V300\BladeMill\BladeMillServer\AppData\MeasuringLaws_elb.cle");
        private static string _auxcommandList = Path.Combine(@"C:\Clever\V300\BladeMill\BladeMillServer\AppData\AuxiliaryCommands_elb.xml");

        [Theory]
        [MemberData(nameof(Datas))]
        public void AppXmlConfDirectories_WhenNotPass_ReturnError(string appXmlConfDirectories, string expected)
        {
            Assert.Equal(appXmlConfDirectories, expected);
        }

        public static IEnumerable<object[]> Datas
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ new AppXmlConfFiles(_applicationXmlFile).TOOL_LIST , _toolList },
                    new string[]{ new AppXmlConfFiles(_applicationXmlFile).MACHINE_LIST , _machineList },
                    new string[]{ new AppXmlConfFiles(_applicationXmlFile).APP_RET_LIST , _app_retList },
                    new string[]{ new AppXmlConfFiles(_applicationXmlFile).MFG_PROCESS_LIST , _mfgprocessList },
                    new string[]{ new AppXmlConfFiles(_applicationXmlFile).TECHNOLOGY_LIST , _technologyList },
                    new string[]{ new AppXmlConfFiles(_applicationXmlFile).MEASURINGLAW_LIST , _measuringList },
                    new string[]{ new AppXmlConfFiles(_applicationXmlFile).AUXCOMMAND_LIST , _auxcommandList }
                };
            }
        }

    }
}
