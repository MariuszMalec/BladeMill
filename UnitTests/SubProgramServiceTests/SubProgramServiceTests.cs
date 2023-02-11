using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace UnitTests.SubProgramServiceTests
{
    public class SubProgramServiceTests
    {
        private string _programName = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        public SubProgramServiceTests()
        {
            Sut = new SubProgramService(_programName);
        }
        private SubProgramService Sut { get; }

        [Fact]
        public void GetSubprogramsListFromNc_ReturnError_WhenIsEmpty()
        {
            var machine = Sut.GetSubprogramsListFromNc();
            machine.Should().NotBeNullOrEmpty();
        }
    }
}
