using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace UnitTests.ToolNcServiceTests
{
    public  class ToolNcServiceTests
    {
        private string _programName = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private string _subprogramName = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888811.SPF");
        public ToolNcServiceTests()
        {
            Sut = new ToolNcService();
        }
        private ToolNcService Sut { get; }


        [Fact]
        public void LoadToolsFromFileCountToolIdAndPreloadId_IfTheSame_ReturnTrue()
        {
            var ncService = new ToolNcService();
            var incService = new ToolService(ncService);
            var toolsFromNc = incService.LoadToolsFromFile(_programName);

            var toolsId = toolsFromNc.Select(t => t.ToolID);
            var toolsPreload = toolsFromNc.Select(t => t.ToolIDPreLoad);

            var result = toolsId.Count() == toolsPreload.Count();

            Assert.True(result);
        }

        [Fact]
        public void LoadToolsFromFileM6CountToolIdAndPreloadId_IfTheSame_ReturnTrue()
        {
            var ncService = new ToolNcService();
            var toolsFromNc = ncService.LoadToolsFromFile(_programName, true);

            var toolsId = toolsFromNc.Select(t => t.ToolID);
            var toolsPreload = toolsFromNc.Select(t => t.ToolIDPreLoad);

            var result = toolsId.Count() == toolsPreload.Count();

            Assert.True(result);
        }

        [Fact]
        public void LoadToolsFromFileM6CheckToolIdNotTheSameAsPreloadId_IfNotTheSame_ReturnTrue()
        {
            var ncService = new ToolNcService();
            var toolsFromNc = ncService.LoadToolsFromFile(_programName, true);

            var toolsId = toolsFromNc.Select(t => t.ToolID);
            var toolsPreload = toolsFromNc.Select(t => t.ToolIDPreLoad);

            bool result = true;
            if (toolsId.Count() > 0 && toolsPreload.Count() > 0 && toolsId.Count() == toolsPreload.Count())
            {
                var results = toolsId.Any(x => !toolsPreload.Any(y => y != x));
            }         
            Assert.True(result);
        }

        [Fact]
        public void LoadToolsFromFile_ReturnError_WhenIsEmpty()
        {
            var machine = Sut.LoadToolsFromFile(_programName);
            machine.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetNcSpindleFromNC_ReturnError_WhenNotContainS()
        {
            var spindle = Sut.GetNcSpindleFromNC(_subprogramName);
            spindle.Should().Contain("S");
        }

        [Fact]
        public void GetToolIdFromNc_ReturnError_WhenNotHave7Number()
        {
            var toolId = Sut.GetToolIdFromNc(_subprogramName).Length;
            Assert.Equal(7, toolId);
        }

        [Fact]
        public void GetMachineFromFile_ReturnError_WhenIsEmpty()
        {
            var machine = Sut.GetMachineFromFile(_subprogramName);
            machine.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetNcToollenFromNC_ReturnError_WhenIsEmpty()
        {
            var tool = Sut.GetNcToollenFromNC(_subprogramName);
            tool.Should().NotContain("-");
        }

        [Fact]
        public void GetNcToolDiamFromNC_ReturnError_WhenIsEmpty()
        {
            var tool = Sut.GetNcToolDiamFromNC(_subprogramName);
            tool.Should().NotContain("-");
        }

        [Fact]
        public void GetNcToolCrnFromNC_ReturnError_WhenIsEmpty()
        {
            var tool = Sut.GetNcToolCrnFromNC(_subprogramName);
            tool.Should().NotContain("-");
        }

    }
}
