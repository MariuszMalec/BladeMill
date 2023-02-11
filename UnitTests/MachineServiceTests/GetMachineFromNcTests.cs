using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace UnitTests.MachineServiceTests
{
    public class GetMachineFromNcTests
    {
        private readonly static string _hstm300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private readonly static string _hstm500hd = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B01134835.SPF");
        private readonly static string _huron = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "8889949.nc");
        private readonly static string _avia = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "CZOLO_BANDAZ_ZGR.SPF");
        private readonly static string _hec = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "CZOLO_B_WK_SKOS.SPF");
        private readonly static string _subProgram61 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888861.SPF");
        private readonly static string _hx151 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "4444401.MPF");
        private readonly static string _hstm500M = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "C00091801.MPF");

        public GetMachineFromNcTests()
        {
            Sut = new GetMachineFromNc();
        }
        private GetMachineFromNc Sut { get; }

        [Fact]
        public void GetMachine_WhenIsEmpty_ReturnError()
        {
            var machine = Sut.GetMachine(_subProgram61);

            var result = machine.MachineName.Contains('-');

            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(Datas))]
        public void GetMachineFromNc_WhenNotPass_ReturnError(string file, string machineEpected)
        {
            var machine = Sut.GetMachine(file.ToString());

            var result = machine.MachineName;

            Assert.Equal(machineEpected, result);
        }

        public static IEnumerable<object[]> Datas
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ _hstm300 ,  MachineEnum.HSTM300.ToString()},
                    new string[]{ _hstm500hd, MachineEnum.HSTM500.ToString()},
                    new string[]{ _huron, MachineEnum.HURON.ToString()},
                    new string[]{ _avia, "AVIA"},
                    new string[]{ _hec, "HEC"},
                    new string[]{ _hx151, "HX151"},
                    new string[]{ _subProgram61, MachineEnum.HSTM300.ToString()},
                    new string[]{ _hstm500M, MachineEnum.HSTM500M.ToString()}
                };
            }
        }

        [Theory]
        [MemberData(nameof(Machines))]
        public void CheckMachineInList_WhenNotPass_ReturnError(string machineEpected)
        {
            var machines = Sut.GetAllMachines();

            var result = machines.Any(m=>m.Contains(machineEpected));

            Assert.True(result);
        }

        public static IEnumerable<object[]> Machines
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ MachineEnum.HSTM300.ToString()},
                    new string[]{ MachineEnum.HSTM300HD.ToString()},
                    new string[]{ MachineEnum.HSTM500.ToString()},
                    new string[]{ MachineEnum.HSTM500M.ToString()},
                    new string[]{ MachineEnum.HURON.ToString()},
                    new string[]{ MachineEnum.HSTM1000.ToString()},
                    new string[]{ MachineEnum.HX151.ToString()},
                    new string[]{ MachineEnum.AVIA.ToString()},
                    new string[]{ MachineEnum.HEC.ToString()}
                };
            }
        }
    }
}
