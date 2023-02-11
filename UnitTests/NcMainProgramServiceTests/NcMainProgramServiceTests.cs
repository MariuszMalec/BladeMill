using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace UnitTests.NcMainProgramServiceTests
{
    public class NcMainProgramServiceTests
    {
        private string _programName = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        public NcMainProgramServiceTests()
        {
            Sut = new NcMainProgramService(_programName);
        }
        private NcMainProgramService Sut { get; }

        [Fact]
        public void GetMachine_ReturnError_WhenIsEmpty()
        {
            var machine = Sut.GetMachine();
            machine.Should().NotBeNullOrEmpty();
        }
    }
}
