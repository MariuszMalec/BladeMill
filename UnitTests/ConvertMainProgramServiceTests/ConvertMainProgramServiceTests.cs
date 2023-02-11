using BladeMill.BLL.Entities;
using BladeMill.BLL.Enums;
using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace UnitTests.ConvertMainProgramServiceTests
{
    public class ConvertMainProgramServiceTests
    {
        private static string _mainprogramHSTM300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private static string _mainprogramHSTM300HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "D99999901.MPF");
        private PathDataBase _pathData;

        public ConvertMainProgramServiceTests()
        {
            _pathData = new PathDataBase();
        }

        [Theory]
        [InlineData("; MACHINE: HSTM_500_SIM840D_PY")]
        [InlineData("R61")]
        [InlineData("N22 G0 U=R61+10; dojazd przed klocek")]
        [InlineData("M38")]
        [InlineData("N24 PEAKFW(1000)")]
        [InlineData("R99=$AA_IW[U]")]
        [InlineData("N27 PRESS_ON(900)")]
        [InlineData("E_ZDARZ=1")]
        [InlineData("E_ZDARZ=2")]
        public void CheckConvertedToHSTM500_ShoudReturnAllChanges_WhenNotFail(string value)
        {
            // Arrange
            var newNamePrg = "Btest1";
            ConvertMainProgram myConvertMainProgram = ConvertToHstm500HD(newNamePrg);
            var sut = new ConvertMainProgramService(myConvertMainProgram);
            // Act

            //ksaowanie pliku
            var outPutDir = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName);
            if (Directory.Exists(outPutDir))
                Directory.Delete(outPutDir, true);

            sut.FixMainProgram();

            //read file
            var newProgram = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName,
                myConvertMainProgram.NewProgramName + "01.MPF");

            var serviceFile = new FileService();
            var lines = serviceFile.GetLinesFromFile(newProgram);

            var result = lines.Any(l => l.Line.Contains(value));

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("; MACHINE: HSTM_300_SIM840D_PY")]
        [InlineData("R61=")]
        [InlineData("N22 G0 U=R61+10; dojazd przed klocek")]
        [InlineData("M38")]
        [InlineData("N24 PEAKFW(1000)")]
        [InlineData("R99=$AA_IW[U]")]
        [InlineData("N27 PRESS_ON(900)")]
        [InlineData("E_ZDARZ=1")]
        [InlineData("E_ZDARZ=2")]
        public void CheckConvertedToHSTM300_ShoudReturnAllChanges_WhenNotFail(string value)
        {
            // Arrange
            var newNamePrg = "Atest2";
            ConvertMainProgram myConvertMainProgram = ConvertToHSTM300(newNamePrg);
            var sut = new ConvertMainProgramService(myConvertMainProgram);
            // Act

            //ksaowanie pliku
            var outPutDir = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName);
            if (Directory.Exists(outPutDir))
                Directory.Delete(outPutDir, true);

            sut.FixMainProgram();

            //read file
            var newProgram = Path.Combine(@"c:/tempnc",                 
                myConvertMainProgram.NewProgramName,
                myConvertMainProgram.NewProgramName + "01.MPF");

            var serviceFile = new FileService();
            var lines = serviceFile.GetLinesFromFile(newProgram);

            var result = lines.Any(l=>l.Line.Contains(value));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FixMainProgramHSTM300_HSTM300HD_ShoudExistTemplateMainProgram_WhenNotFail()
        {
            // Arrange
            var newNamePrg = "Dtest1";
            ConvertMainProgram myConvertMainProgram = ConvertToHstm300HD(newNamePrg);
            var sut = new ConvertMainProgramService(myConvertMainProgram);
            // Act
            sut.FixMainProgram();
            // Assert
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(myConvertMainProgram.TemplateMainProgram);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void FixMainProgramHSTM300HD_HSTM300_ShoudExistTemplateMainProgram_WhenNotFail()
        {
            // Arrange
            var newNamePrg = "Atest1";
            ConvertMainProgram myConvertMainProgram = ConvertToHSTM300(newNamePrg);
            var sut = new ConvertMainProgramService(myConvertMainProgram);
            // Act
            sut.FixMainProgram();
            // Assert
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(myConvertMainProgram.TemplateMainProgram);
            act.Should().NotThrow<FileNotFoundException>();
        }

        private ConvertMainProgram ConvertToHSTM300(string newNamePrg)
        {
            var templatemp = _pathData.GetFileMainProgramTemplate(MachineEnum.HSTM300.ToString());
            var myConvertMainProgram = new ConvertMainProgram()
            {
                NewProgramName = newNamePrg,
                AddPreload = true,
                AddRaport = false,
                ChangeNameProgram = true,
                DeletePreload = false,
                MachineType = MachineEnum.HSTM300,
                ProgramName = _mainprogramHSTM300HD,
                DeleteRaport = true,
                OrgMachine = MachineEnum.HSTM300HD.ToString(),
                ReplaceCycleTool = false,
                ReplaceToolCycle = true,
                TemplateMainProgram = templatemp
            };
            return myConvertMainProgram;
        }
        private ConvertMainProgram ConvertToHstm300HD(string newNamePrg)
        {
            var templatemp = _pathData.GetFileMainProgramTemplate(MachineEnum.HSTM300HD.ToString());
            var myConvertMainProgram = new ConvertMainProgram()
            {
                NewProgramName = newNamePrg,
                AddPreload = true,
                AddRaport = false,
                ChangeNameProgram = true,
                DeletePreload = false,
                MachineType = MachineEnum.HSTM300HD,
                ProgramName = _mainprogramHSTM300,
                DeleteRaport = true,
                OrgMachine = MachineEnum.HSTM300.ToString(),
                ReplaceCycleTool = false,
                ReplaceToolCycle = true,
                TemplateMainProgram = templatemp
            };
            return myConvertMainProgram;
        }
        private ConvertMainProgram ConvertToHstm500HD(string newNamePrg)
        {
            var templatemp = _pathData.GetFileMainProgramTemplate(MachineEnum.HSTM500.ToString());
            var myConvertMainProgram = new ConvertMainProgram()
            {
                NewProgramName = newNamePrg,
                AddPreload = true,
                AddRaport = false,
                ChangeNameProgram = true,
                DeletePreload = false,
                MachineType = MachineEnum.HSTM500,
                ProgramName = _mainprogramHSTM300HD,
                DeleteRaport = true,
                OrgMachine = MachineEnum.HSTM300HD.ToString(),
                ReplaceCycleTool = false,
                ReplaceToolCycle = true,
                TemplateMainProgram = templatemp
            };
            return myConvertMainProgram;
        }
    }
}
