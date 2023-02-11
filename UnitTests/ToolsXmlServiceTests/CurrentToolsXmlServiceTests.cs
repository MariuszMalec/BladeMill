using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using FluentAssertions;
using Xunit;

namespace UnitTests.ToolsXmlServiceTests
{
    public class CurrentToolsXmlServiceTests
    {
        private PathDataBase _pathService = new PathDataBase();
        private string toolxmlfile;
        public CurrentToolsXmlServiceTests()
        {
            Sut = new ToolXmlService();
            toolxmlfile = _pathService.GetFileCurrentToolsXml();
        }
        private ToolXmlService Sut { get; }

        [Fact]
        public void GetToolsFromCurrentXmlFile_ReturnError_WhenIsEmpty()
        {
            var tools = Sut.LoadToolsFromFile(toolxmlfile);
            tools.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMachineFromFile_ReturnError_WhenIsEmpty()
        {
            var machine = Sut.GetMachineFromFile(toolxmlfile);
            machine.Should().NotBeNullOrEmpty();
            machine.Should().NotContain("-");
        }

        [Fact]
        public void GetFromFileValue_ReturnError_WhenIsEmpty()
        {
            var prg = Sut.GetFromFileValue(toolxmlfile, "PRGNUMBER");
            prg.Should().NotBeNullOrEmpty();
            prg.Should().NotContain("-");
        }

        [Fact]
        public void LoadToolsFromFile_ReturnError_WhenIsEmpty()
        {
            var machine = Sut.LoadToolsFromFile(toolxmlfile);
            machine.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("HROH")]
        [InlineData("BROH")]
        [InlineData("LROH")]
        public void GetDimensionBox_WhenIsEmpty_ReturnError(object value)
        {
            var result = Sut.GetFromFileValue(toolxmlfile, value.ToString());
            result.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("HROH")]
        [InlineData("BROH")]
        [InlineData("LROH")]
        public void GetDimensionBox_WhenIsNotDouble_ReturnError(object value)
        {
            var getResult = Sut.GetFromFileValue(toolxmlfile, value.ToString());

            var result = GetDouble(getResult);
            
            result.Should().BeTrue();
        }

        private bool GetDouble(string value)
        {
            return double.TryParse(value, out double number);
        }

    }
}
