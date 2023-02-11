using BladeMill.BLL.Models;
using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace UnitTests.XmlVarpoolServicesTests
{
    public class VarpoolXmlFileModelTests
    {
        private string _currentVarpool = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A888888_varpool.xml");

        public VarpoolXmlFileModelTests()
        {
            Sut = new VarpoolXmlFile(_currentVarpool);
        }
        private VarpoolXmlFile Sut { get; }

        [Fact]
        public void VarpoolFile_WhenIsNullOrEmpty_ReturnError()
        {
            var check = Sut.VarpoolFile;
            check.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void MfgSystem_WhenIsNullOrEmpty_ReturnError()
        {
            var check = Sut.MfgSystem;
            check.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ClampMethod_WhenIsNullOrEmpty_ReturnError()
        {
            var check = Sut.ClampMethod;
            check.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void LROH_WhenIsNullOrEmpty_ReturnError()
        {
            var check = Sut.LROH;
            check.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void BladeMaterial_WhenIsNullOrEmpty_ReturnError()
        {
            var check = Sut.BladeMaterial;
            check.Should().NotBeNullOrEmpty();
        }

    }
}
