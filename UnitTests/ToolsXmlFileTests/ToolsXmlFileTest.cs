using BladeMill.BLL.Models;
using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace UnitTests.ToolsXmlFileTests
{
    public class ToolsXmlFileTest
    {
        private string _toolsXmlFile= Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A888888.tools.xml");
        public ToolsXmlFileTest()
        {
            Sut = new ToolsXmlFile(_toolsXmlFile);
        }
        private ToolsXmlFile Sut { get; }

        [Fact]
        public void MACHINE_FromXmlFile_WhenIsNotEmpty()
        {
            var machine = Sut.MACHINE;
            machine.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void PRGNUMBER_FromXmlFile_WhenIsNotEmpty()
        {
            var machine = Sut.PRGNUMBER;
            machine.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void LROH_FromXmlFile_WhenIsNotEmpty()
        {
            var machine = Sut.LROH;
            machine.Should().BeGreaterThan(0);
        }

        [Fact]
        public void BROH_FromXmlFile_WhenIsNotEmpty()
        {
            var machine = Sut.BROH;
            machine.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HROH_FromXmlFile_WhenIsNotEmpty()
        {
            var machine = Sut.HROH;
            machine.Should().BeGreaterThan(0);
        }
    }
}
