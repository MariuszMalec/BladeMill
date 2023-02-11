using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.FileServiceTests
{
    public class FileServiceTests
    {
        private string _nc = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private string _ncAvia = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "00000030.MPF");
        private string _dir = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData");
        private string _exe = "SPF";
        private string _name = "A888888";
        private FileService _sut { get; }
        public FileServiceTests()
        {
            _sut = new FileService();
        }

        [Fact]
        public void GetSubprogramsListFromNc_ForAvia__WhenIsNullOrEmpty_ReturnError()
        {
            var ncLines = _sut.GetSubprogramsListFromNc(_ncAvia);

            var result = ncLines.Any(x => 
            x.SubProgramNameWithDir == null &&
            x.SubProgramNameWithDir == string.Empty);

            result.Should().BeFalse();
            ncLines.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetLinesFromFile_WhenIsNullOrEmpty_ReturnError()
        {
            var ncLines = _sut.GetLinesFromFile(_nc);
            ncLines.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetSubprogramsListFromNc_WhenIsNullOrEmpty_ReturnError()
        {
            var ncLines = _sut.GetSubprogramsListFromNc(_nc);
            ncLines.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetListSelectedFiles_WhenIsNullOrEmpty_ReturnError()
        {
            var ncLines = _sut.GetListSelectedFiles(_dir,_exe);
            ncLines.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetSubprogramsListFromNcAsIEnumerable_WhenIsNullOrEmpty_ReturnError()
        {
            var ncLines = _sut.GetSubprogramsListFromNcAsIEnumerable(_nc);
            ncLines.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetListFilesWithName_WhenIsNullOrEmpty_ReturnError()
        {
            var ncLines = _sut.GetListFilesWithName(_dir, _exe, _name);
            ncLines.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMainProgromFromDir_WhenIsNullOrEmpty_ReturnError()
        {
            var ncLines = _sut.GetMainProgromFromDir(_dir, _exe);
            ncLines.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetAll_WhenIsNullOrEmpty_ReturnError()
        {
            var ncLines = _sut.GetAll();
            ncLines.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetById_WhenIsNullOrEmpty_ReturnError()
        {
            _sut.GetAll();
            var ncMainProgram = _sut.GetById(1);
            ncMainProgram.Should().NotBeNull();
        }
    }
}
