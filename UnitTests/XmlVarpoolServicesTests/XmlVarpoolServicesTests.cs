using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Services;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;

namespace UnitTests.XmlVarpoolServicesTests
{
    public class XmlVarpoolServicesTests
    {
        private string _currentVarpool = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A888888_varpool.xml");
        private string _currentXmlFile = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "current.xml");
        private string _currentMainProgram = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");

        public XmlVarpoolServicesTests()
        {
            Sut = new XMLVarpoolService();
        }
        private XMLVarpoolService Sut { get; }

        [Fact]
        public void GetCurrentVarpoolFile_WhenIsNullOrEmpty_ReturnError()
        {
            var file = Sut.GetCurrentVarpoolFile();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void GetVarpolFileAccXmlFile_WhenIsNullOrEmpty_ReturnError()
        {
            var file = Sut.GetVarpolFileAccXmlFile(_currentXmlFile);
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void GetVarpolFileAccNcFile_WhenIsNullOrEmpty_ReturnError()
        {
            var file = Sut.GetVarpolFileAccNcFile(_currentMainProgram);
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }
        [Fact]
        public void GetFromFileValue_WhenIsNullOrEmpty_ReturnError()
        {
            var orderName = Sut.GetFromFileValue(_currentVarpool, "OrderName");
            orderName.Should().NotBeNullOrEmpty();
        }
    }
}
