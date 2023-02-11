using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace UnitTests.XMLVCProjectServiceTests
{
    public class XMLVCProjectServiceTests
    {
        private string _currentToolsXmlFile = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "current.xml");
        public XMLVCProjectServiceTests()
        {
            Sut = new XMLVCProjectService(_currentToolsXmlFile);
        }
        private XMLVCProjectService Sut { get; }

        [Fact]
        public void GetVcProjectTemplate_WhenIsNullOrEmpty_ReturnError()
        {
            var file = Sut.GetVcProjectTemplate();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void GetAllDataFromVcProject_WhenIsNullOrEmpty_ReturnError()
        {
            var getAll = Sut.GetAllDataFromVcProject();
            var machineCtl = getAll.ControlCtl.ToString();
            var machineFile = getAll.MachineMch.ToString();
            var ncProgramFile = getAll.NcProgram.ToString();
            var subroutines = getAll.Subroutine;
            var stls = getAll.StlGeometry;
            machineCtl.Should().NotBeNullOrEmpty();
            machineFile.Should().NotBeNullOrEmpty();
            ncProgramFile.Should().NotBeNullOrEmpty();
            subroutines.Should().NotBeNullOrEmpty();
            stls.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ReplaceToolLbrary_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceToolLbrary();
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
        public void RotationStl_WhenIsFalse_ReturnError()
        {
            var check = Sut.RotationStl("m_CLAMP_ADAPTER.stl", "K", 180);
            Assert.True(check);
        }
        [Fact]
        public void ReplaceGEOMETRY_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceGEOMETRY("m_CLAMP_ADAPTER");
            Assert.True(check);
        }
        [Fact]
        public void ReplaceGEOMETRY_WithNewName_WhenIsFalse_ReturnError()
        {
            var check = Sut.ReplaceGEOMETRY("m_CLAMP_ADAPTER", "nozka");
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
