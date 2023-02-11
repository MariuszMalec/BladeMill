using BladeMill.BLL.Enums;
using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Models;
using BladeMill.BLL.Repositories;
using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.ProgramExeServiceTests
{
    public class ProgramExeServiceTests
    {
        private PathDataBase _pathService;

        public ProgramExeServiceTests()
        {
            Sut = new ProgramExeService();
            _pathService= new PathDataBase();
        }
        private ProgramExeService Sut { get; }

        [Fact]
        public void GetFullName_IfFilesExist_ReturnNotThrowFileNotFoundException()
        {
            var listProgramExe = Sut.GetFullName();
            foreach (var file in listProgramExe)
            {
                Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file);
                act.Should().NotThrow<FileNotFoundException>();
            }               
        }

        [Fact]
        public void GetAll_IfFilesExist_ReturnNotThrowFileNotFoundException()
        {
            var listProgramExe = Sut.GetAll();
            foreach (var file in listProgramExe)
            {
                Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(file.FullName);
                act.Should().NotThrow<FileNotFoundException>();
            }
        }

        [Fact]
        public void GetListEnemExe_IfIsEmptyOrNull_ReturnNotBeNullOrEmpty()
        {
            var exeEnums = Sut.GetListEnemExe();
            exeEnums.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetProgramExeById_IfIsNull_ReturnNull()
        {
            // Arrange
            Sut.GetAll();

            // Act
            var programExe = Sut.GetProgramExeById(0);

            // Assert
            Assert.Null(programExe);
        }

        [Fact]
        public void GetFullNameById_IfFileExist_ReturnNotThrowFileNotFoundException()
        {
            Sut.GetAll();
            var programExe = Sut.GetFullNameById(1);
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(programExe);
            act.Should().NotThrow<FileNotFoundException>();
        }
    }
}
