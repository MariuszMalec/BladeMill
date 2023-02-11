using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.MachineServiceTests
{
    public class GetMachineFromToolsXmlTests
    {
        private static string _hstm300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A888888.tools.xml");
        private static string _hx = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "44444.tools.xml");
        private static string _huron = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "99999.tools.xml");

        public GetMachineFromToolsXmlTests()
        {
            Sut = new GetMachineFromToolsXml();
        }
        private GetMachineFromToolsXml Sut { get; }


        [Theory]
        [MemberData(nameof(Datas))]
        public void GetMachineFromToolsXml_WhenNotPass_ReturnError(string file, string machineEpected)
        {
            var shapeFactory = new MachineServiceFactory();
            var ncFile = shapeFactory.CreateMachine(TypeOfFile.toolsXmlFile);
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
                    new string[]{ _huron, "HURON"},
                    new string[]{ _hx, "HX151"},
                };
            }
        }
    }
}
