using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace UnitTests.MachineServiceTests
{
    //TODO stara wersja do wyrzucenia , zastapi ja GetMachineFromNcTests
    public class MachineServiceTests
    {
        private string _programName = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private string _currentXmlName = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "current.xml");
        private string _programName61 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888861.SPF");
        public MachineServiceTests()
        {
            Sut = new MachineService();
        }
        private MachineService Sut { get; }

        [Fact]
        public void CheckIfMachine_NotExist_ReturnEror()
        {
            var machines = Sut.GetListMachines();
            string[] existMachines = new string[] { "HSTM300",
                                                    "HSTM500",
                                                    "HSTM1000",
                                                    "HSTM1500",
                                                    "HX151",
                                                    "HURON",
                                                    "HSTM300HD",
                                                    "AVIA",
                                                    "HEC",
                                                    "HSTM500M" };
            foreach (var item in machines)
            {
                var isMachine = existMachines.Contains(item);
                Assert.True(isMachine);
            }
        }

        [Fact]
        public void GetNcMachineFromNC_ReturnError_WhenIsEmpty()
        {
            var machine = Sut.GetNcMachineFromNC(_programName);
            machine.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetNcMachineFromNC_61ReturnError_WhenIsEmpty()
        {
            var machine = Sut.GetNcMachineFromNC(_programName61);
            machine.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetNcMachineFromXmlFile_ReturnError_WhenIsEmpty()
        {
            var machine = Sut.GetNcMachineFromXmlFile(_currentXmlName);
            machine.Should().NotBeNullOrEmpty();
        }

    }
}
