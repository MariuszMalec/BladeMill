using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.SourceData;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.PathsDataTests
{
    public class PathsDataCheckFilesTests
    {
        private PathDataBase Sut { get; }

        public PathsDataCheckFilesTests()
        {
            Sut = new PathDataBase();
        }

        [Theory]
        [InlineData("HSTM300")]
        [InlineData("HSTM300HD")]
        [InlineData("HSTM500")]
        [InlineData("HSTM500M")]
        [InlineData("HSTM1000")]
        [InlineData("HX151")]
        public void GetFileMainProgramTemplate_WhenNotExist_ReturnNoTThrowFileNotFoundException(string machine)
        {
            var file = Sut.GetFileMainProgramTemplate(machine);
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Theory]
        [InlineData("HSTM300")]
        [InlineData("HSTM300HD")]
        [InlineData("HSTM500")]
        [InlineData("HSTM500M")]
        [InlineData("HSTM1000")]
        [InlineData("HX151")]
        [InlineData("HURON")]
        public void GetMchFile_WhenIsEmpty_ReturnError(string machine)
        {
            var file = Sut.GetMchFile(machine);
            file.Should().NotBeNullOrEmpty();
            file.Should().NotBe("");
        }

        [Theory]
        [InlineData("HSTM300")]
        [InlineData("HSTM300HD")]
        [InlineData("HSTM500")]
        [InlineData("HSTM500M")]
        [InlineData("HSTM1000")]
        [InlineData("HX151")]
        [InlineData("HURON")]
        public void GetCtlFile_WhenIsEmpty_ReturnError(string machine)
        {
            var file = Sut.GetCtlFile(machine);
            file.Should().NotBeNullOrEmpty();
            file.Should().NotBe("");
        }

        [Theory]
        [ClassData(typeof(FirstNameClassData))]
        public void GetFile_WhenFileNotExist_ReturnError(string value)
        {
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(value);
            act.Should().NotThrow<FileNotFoundException>();
        }
        internal class FirstNameClassData : IEnumerable<object[]>
        {
            static PathDataBase _paths = new PathDataBase();
            string file0 = _paths.GetFileCurrentToolsXml();
            string file2 = _paths.GetFileExcelTemplate();
            string file3 = _paths.GetApplicationConfFile();
            string file4 = _paths.GetFileVericutToolsLibrary();
            string file5 = _paths.GetGITInitfile();
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new string[] { file0 };
                yield return new string[] { file2 };
                yield return new string[] { file3 };
                yield return new string[] { file4 };
                yield return new string[] { file5 };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
