using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using FluentAssertions;
using Xunit;

namespace UnitTests.ToolsXmlFileTests
{
    public class CurentToolsXmlFileTests
    {
        private string _toolxmlfile;
        private PathDataBase _pathService = new PathDataBase();

        public CurentToolsXmlFileTests()
        {
            _toolxmlfile = _pathService.GetFileCurrentToolsXml();
            Sut = new ToolsXmlFile(_toolxmlfile);            
        }
        private ToolsXmlFile Sut { get; }

        [Fact]
        public void GetMainProgramFileFromCurrentToolsXml_ReturnError_WhenIsEmpty()
        {
            var mainProgram = Sut.GetMainProgramFileFromCurrentToolsXml();
            var result = mainProgram.Contains("-01.MPF");

            mainProgram.Should().NotBeNullOrEmpty();
            Assert.False(result);
        }

        [Fact]
        public void GetMainProgramFileFromToolsXml_ReturnError_WhenIsEmpty()
        {
            var mainProgram = Sut.GetMainProgramFileFromToolsXml(_toolxmlfile, "HSTM500");
            var result = mainProgram.Contains("-01.MPF");

            mainProgram.Should().NotBeNullOrEmpty();
            Assert.False(result);
        }

        [Fact]
        public void GetMachineFromCurrentXmlFile_WhenIsNotEmpty()
        {
            var machine = Sut.MACHINE;
            machine.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public void GetMainProgramFromCurrentXmlFile_WhenIsNotEmpty()
        {
            var prgNumber = Sut.PRGNUMBER;
            prgNumber.Should().NotBeNullOrEmpty();
        }
    }
}
