using BladeMill.BLL.Enums;
using BladeMill.BLL.Validators;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.ValidateConvertMainProgramTests
{
    public class ValidateConvertMainProgramTests
    {
        private readonly ValidateConvertMainProgram _sut;
        private readonly Mock<ILogger> _mockLogger = new Mock<ILogger>();

        private static string _mainprogramHSTM300_NotExist = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "AXXXXX01.MPF");
        private static string _mainprogramHSTM300_WrongName = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888803.MPF");

        private static string _mainprogramHSTM300_OK = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private static string _mainprogramHSTM300HD_OK = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "D99999901.MPF");
        private static string _mainprogramHSTM500HD_OK = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B01143001.MPF");
        private static string _mainprogramHSTM500M_OK = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "C00048901.MPF");
        private static string _mainprogramHX151_OK = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "1404601.MPF");
        private static string _mainprogramHURON_OK = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "8312701.NC");

        public ValidateConvertMainProgramTests()
        {
            _sut = new ValidateConvertMainProgram(_mockLogger.Object);
        }

        [Theory]
        [MemberData(nameof(CheckMachinesFromNcCorrect))]
        public void ValidationMachine_ReturnsTrue_WhenValidationSuccess(string file, string machine)
        {
            //Arrange
            // Act
            var result = _sut.ValidationMachine(machine, file);
            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(CheckMachinesFromNcWithErrors))]
        public void ValidationMachine_ReturnsFalse_WhenValidationFails(string file, string machine)
        {
            //Arrange
            // Act
            var result = _sut.ValidationMachine(machine, file);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(MainProgramsCorrect))]
        public void ValidationMainProgram_ReturnsTrue_WhenValidationSuccess(string file)
        {
            //Arrange
            // Act
            var result = _sut.ValidationMainProgram(file);
            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("123456")]
        public void ValidationNewNameProgram_ReturnsTrue_WhenValidationSuccess(string name)
        {
            //Arrange
            // Act
            var result = _sut.ValidationNewNameProgram(name);
            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("1234567")]
        [InlineData("")]
        [InlineData(null)]
        public void ValidationNewNameProgram_ReturnsFalse_WhenValidationFails(string name)
        {
            //Arrange
            // Act
            var result = _sut.ValidationNewNameProgram(name);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(MainProgramsWithErrors))]
        public void ValidationMainProgram_ReturnsFalse_WhenValidationFails(string file)
        {
            //Arrange
            // Act
            var result = _sut.ValidationMainProgram(file);
            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> MainProgramsCorrect
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ _mainprogramHSTM300_OK},
                    new string[]{ _mainprogramHSTM500HD_OK},
                    new string[]{ _mainprogramHSTM500M_OK},
                    new string[]{ _mainprogramHSTM300HD_OK},
                    new string[]{ _mainprogramHX151_OK},
                    new string[]{ _mainprogramHURON_OK }
                };
            }
        }

        public static IEnumerable<object[]> MainProgramsWithErrors
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ _mainprogramHSTM300_NotExist},
                    new string[]{ _mainprogramHSTM300_WrongName}
                };
            }
        }

        public static IEnumerable<object[]> CheckMachinesFromNcCorrect
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ _mainprogramHSTM300_OK, MachineEnum.HSTM300.ToString()},
                    new string[]{ _mainprogramHSTM500HD_OK, MachineEnum.HSTM500.ToString()},
                    new string[]{ _mainprogramHSTM500M_OK, MachineEnum.HSTM500M.ToString()},
                    new string[]{ _mainprogramHSTM300HD_OK, MachineEnum.HSTM300HD.ToString()},
                    new string[]{ _mainprogramHX151_OK, MachineEnum.HX151.ToString()},
                    new string[]{ _mainprogramHURON_OK, MachineEnum.HURON.ToString()}
                };
            }
        }
        public static IEnumerable<object[]> CheckMachinesFromNcWithErrors
        {
            get
            {
                return new List<object[]>
                {
                    new string[]{ _mainprogramHSTM300_NotExist, MachineEnum.HSTM300.ToString()},
                    new string[]{ _mainprogramHSTM300_WrongName, MachineEnum.HSTM300.ToString()},
                    new string[]{ _mainprogramHX151_OK, MachineEnum.HSTM300.ToString()},
                    new string[]{ _mainprogramHSTM300_OK, MachineEnum.HX151.ToString()}
                };
            }
        }

    }
}
