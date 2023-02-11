using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.SourceData;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.PathsDataTests
{
    public class PathsDataTests_notUseYet
    {
        public PathsDataTests_notUseYet()
        {
            Sut = new PathDataBase();
        }
        private PathDataBase Sut { get; }

        [Fact]
        public void GetVcProjectFiles_WhenIsNotEmpty()
        {
            IEnumerable<string> files = Sut.GetAllVcProjectTemplateFiles();
            foreach (var item in files)
            {
                Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(item);
                act.Should().NotThrow<FileNotFoundException>();
            }
        }

        [Fact]
        public void GetGITInitfile_WhenFileNotExist()
        {
            var file = Sut.GetGITInitfile();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void GetFileVericutToolsLibrary_WhenFileNotExist()
        {
            var file = Sut.GetFileVericutToolsLibrary();
            if (!File.Exists(file))
            {
                file = string.Empty;
            }
            file.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetApplicationConfFile_WhenFileNotExist()
        {
            var file = Sut.GetApplicationConfFile();
            if (!File.Exists(file))
            {
                file = string.Empty;
            }
            file.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetFileCurrentToolsXml_WhenFileNotExist()
        {
            var file = Sut.GetFileCurrentToolsXml();
            if (!File.Exists(file))
            {
                file = string.Empty;
            }
            file.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetFileAutomationScriptLauncher_WhenFileNotExist()
        {
            var file = Sut.GetDirAutomationScriptLauncher();
            if (!Directory.Exists(file))
            {
                file = string.Empty;
            }
            file.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetDirProgramExe_WhenIsNotCorrectDirName()
        {
            var dirOrders = Sut.GetDirProgramExe();
            if (!Directory.Exists(dirOrders))
            {
                dirOrders = string.Empty;
            }
            dirOrders.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public void GetDirOrders_WhenIsNotCorrectDirName()
        {
            var dirOrders = Sut.GetDirOrders();
            if (!Directory.Exists(dirOrders))
            {
                dirOrders = string.Empty;
            }
            dirOrders.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetDirOneDriveCleverer_WhenIsNotEmpty()
        {
            var file = Sut.GetDirOneDriveClever();
            file.Should().NotBeNullOrEmpty();
            file.Should().NotBe("");
        }
        [Fact]
        public void GetDirDrive_WhenIsNotEmpty()
        {
            var file = Sut.GetDirDrive();
            if (!Directory.Exists(file))
            {
                file = string.Empty;
            }
            file.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public void GetFileExcelTemplate_WhenIsNotEmpty()
        {
            var file = Sut.GetFileExcelTemplate();
            file.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetVcProjectTemplateFile_WhenIsNotEmpty()
        {
            var  files = Sut.GetAllVcProjectTemplateFiles();
            files.Should().NotBeNullOrEmpty();
        }
    }
}
