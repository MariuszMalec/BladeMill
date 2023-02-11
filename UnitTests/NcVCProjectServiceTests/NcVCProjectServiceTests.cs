using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Services;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;

namespace UnitTests.NcVCProjectServiceTests
{
    public class NcVCProjectServiceTests
    {
        private string _currentMainProgram = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");

        private string _mainprogramHSTM500HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B01143001.MPF");
        private string _mainprogramHSTM500M = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "C99999901.MPF");
        private string _mainprogramHSTM300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private string _mainprogramHSTM300HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "D99999901.MPF");
        public NcVCProjectServiceTests()
        {
            Sut = new NcVCProjectService(_currentMainProgram);
        }
        private NcVCProjectService Sut { get; }

        [Fact]
        public void GetVcProjectTemplateForHSTM300_WhenIsNullOrEmpty_ReturnError()
        {
            NcVCProjectService sut = new NcVCProjectService(_mainprogramHSTM300);
            var file = sut.GetVcProjectTemplate();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void GetVcProjectTemplateForHSTM300HD_WhenIsNullOrEmpty_ReturnError()
        {
            NcVCProjectService sut = new NcVCProjectService(_mainprogramHSTM300HD);
            var file = sut.GetVcProjectTemplate();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void GetVcProjectTemplateForHSTM500HD_WhenIsNullOrEmpty_ReturnError()
        {
            NcVCProjectService sut = new NcVCProjectService(_mainprogramHSTM500HD);
            var file = sut.GetVcProjectTemplate();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void GetVcProjectTemplateForHSTM500M_WhenIsNullOrEmpty_ReturnError()
        {
            NcVCProjectService sut = new NcVCProjectService(_mainprogramHSTM500M);
            var file = sut.GetVcProjectTemplate();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void ReplaceToolLbrary_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceToolLbrary();
            Assert.True(check);
        }

        [Fact]
        public void RotationStl_WhenIsFalse_ReturnError()
        {
            var check = Sut.RotationStl("m_CLAMP_ADAPTER.stl", "K", 180);
            Assert.True(check);
        }
        [Fact]
        public void ReplaceGEOMETRY_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceGEOMETRY("m_CLAMP_ADAPTER", "nozka");
            Assert.True(check);
        }
        [Fact]
        public void ReplaceMachineFile_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceMachineFile();
            Assert.True(check);
        }
        [Fact]
        public void ReplaceControlFile_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceControlFile();
            Assert.True(check);
        }
        [Fact]
        public void ReplaceSubroutines_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceSubroutines();
            Assert.True(check);
        }
        [Fact]
        public void ReplaceNcProgramFile_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceNcProgramFile();
            Assert.True(check);
        }
    }
}
