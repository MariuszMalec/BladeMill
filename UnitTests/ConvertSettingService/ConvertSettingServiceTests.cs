using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.ConvertSettingService
{
    public class ConvertSettingServiceTests
    {
        private static string _mainprogramHSTM300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private static string _mainprogramHSTM300HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "D99999901.MPF");
        private static string _mainprogramHSTM500HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B01143001.MPF");
        private static string _mainprogramHSTM500M = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "C00048901.MPF");
        private static string _mainprogramHX151 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "1404601.MPF");
        private static string _mainprogramHSTM500MPrzerobiony = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "C00091801.MPF");

        private ConvertSettingsService _sut { get; }
        public ConvertSettingServiceTests()
        {
            _sut = new ConvertSettingsService();
        }

        [Fact]
        public void SetConvertParametersNewProgramName_ShoudReturnErrors_WhenSettingsAreNull()
        {
            // Arrange
            var mainProgram = _mainprogramHSTM300;
            var newProgramName = "test";
            var newMachine = MachineEnum.HSTM300HD.ToString();
            var settings = _sut.SetConvertParameters(newMachine, mainProgram, newProgramName);

            // Act
            var newPrg = settings.NewProgramName;

            // Assert
            newPrg.Should().NotBeNullOrEmpty();
        }


        [Theory]
        [MemberData(nameof(Datas))]
        public void CheckOrgMachine_WhenNotPass_ReturnError(string file, string newMachine)
        {
            var mainProgram = file;
            var newProgramName = "test";
            var settings = _sut.SetConvertParameters(newMachine, mainProgram, newProgramName);

            var result = settings.OrgMachine;

            Assert.Equal(newMachine, result);
        }

        public static IEnumerable<object[]> Datas
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ _mainprogramHSTM300, MachineEnum.HSTM300.ToString()},
                    new string[]{ _mainprogramHSTM500HD, MachineEnum.HSTM500.ToString()},
                    new string[]{ _mainprogramHSTM500M, MachineEnum.HSTM500M.ToString()},
                    new string[]{ _mainprogramHSTM300HD, MachineEnum.HSTM300HD.ToString()},
                    new string[]{ _mainprogramHX151, MachineEnum.HX151.ToString()},
                    new string[]{ _mainprogramHSTM500MPrzerobiony, MachineEnum.HSTM500M.ToString()}
                };
            }
        }


        [Theory]
        [MemberData(nameof(Prefixes))]
        public void CheckPrefix_WhenNotPass_ReturnError(string file, string newMachine, string prefix)
        {
            var mainProgram = file;
            var newProgramName = prefix;
            var settings = _sut.SetConvertParameters(newMachine, mainProgram, newProgramName);

            var result = settings.Prefix;

            Assert.Equal(prefix, result);
        }

        public static IEnumerable<object[]> Prefixes
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ _mainprogramHSTM300, MachineEnum.HSTM300.ToString(), "A"},
                    new string[]{ _mainprogramHSTM500HD, MachineEnum.HSTM500.ToString(), "B"},
                    new string[]{ _mainprogramHSTM500M, MachineEnum.HSTM500M.ToString(), "C"},
                    new string[]{ _mainprogramHSTM300HD, MachineEnum.HSTM300HD.ToString(), "D"},
                    new string[]{ _mainprogramHX151, MachineEnum.HX151.ToString(), ""}
                };
            }
        }
    }
}
