using BladeMill.BLL.SourceData;
using FluentAssertions;
using System.IO;
using Xunit;

namespace UnitTests.PathsDataTests
{
    public class PathDataCheckTemplateVcProject
    {
        public PathDataCheckTemplateVcProject()
        {
            Sut = new PathDataBase();
        }
        private PathDataBase Sut { get; }

        [Theory]
        [InlineData("HSTM300")]
        [InlineData("HSTM300HD")]
        [InlineData("HSTM500")]
        [InlineData("HSTM500M")]
        [InlineData("HSTM1000")]
        [InlineData("HX151")]
        [InlineData("HURON")]
        public void GetFileVericutProjectTemplate_WhenIsEmpty_ReturnError(object value)
        {
            var result = Sut.GetFileVericutProjectTemplate(value.ToString());
            result.Should().NotBeNullOrEmpty();
        }
    }
}
