using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.MachineServiceTests
{
    public class GetMachineFromVarpoolTests
    {
        private static string _hstm300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A888888_varpool.xml");
        private static string _hx = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "66666_varpool.xml");

        public GetMachineFromVarpoolTests()
        {
            Sut = new GetMachineFromVarpool();
        }
        private GetMachineFromVarpool Sut { get; }


        [Theory]
        [MemberData(nameof(Datas))]
        public void GetMachineFromVarpool_WhenNotPass_ReturnError(string file, string machineEpected)
        {

            var shapeFactory = new MachineServiceFactory();
            var ncFile = shapeFactory.CreateMachine(TypeOfFile.varpoolFile);
            Console.WriteLine($"{ncFile.GetMachine(file).MachineName}");

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
                    new string[]{ _hstm300 , "HSTM300"},
                    new string[]{ _hx, "HX151"},
                };
            }
        }
    }
}
