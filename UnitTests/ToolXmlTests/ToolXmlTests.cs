using BladeMill.BLL.Models;
using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace UnitTests.ToolXmlTests
{
    public class ToolXmlTests
    {
        private string toolxmlfile = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A888888.tools.xml");

        public ToolXmlTests()
        {
            Sut = new ToolsXml(toolxmlfile);
        }
        private ToolsXml Sut { get; }

        [Fact]
        public void GetBROH_WhenIsNotDouble_ReturnError()
        {
            var check = Sut.BROH;
            check.Should().BeGreaterThan(0);
        }

    }
}
