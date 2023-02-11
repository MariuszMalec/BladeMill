using BladeMill.BLL.Entities;
using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace UnitTests.ConvertSubProgramsServiceTests
{
    public class ConvertSubProgramsServiceTests
    {
        private static string _mainprogramHSTM300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");
        private static string _mainprogramHSTM300HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "D99999901.MPF");
        private static string _mainprogramHSTM500HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B01143001.MPF");
        private static string _mainprogramHSTM500M = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "C00048901.MPF");
        private static string _mainprogramHX151 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "1404601.MPF");

        [Theory]
        [InlineData("; MACHINE     : HSTM_300_SIM840D_Py")]
        [InlineData("TOL(")]
        [InlineData("RAPORT")]
        public void CheckSubProgramForHSTM300AfterConvert_ShoudReturnAllChanges_WhenNotFail(string value)
        {
            var newNamePrg = "Atest1";
            ConvertMainProgram myConvertMainProgram = ConvertToHSTM300(newNamePrg);

            var sut = new ConvertSubProgramsService(myConvertMainProgram);

            //kasowanie pliku
            var outPutDir = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName);
            if (Directory.Exists(outPutDir))
                Directory.Delete(outPutDir, true);

            sut.FixSubPrograms();

            var newSubPrg = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName,
                myConvertMainProgram.NewProgramName + "49.SPF");

            var subProgram = new SubProgram()
            {
                SubProgramNameWithDir = newSubPrg,
            };

            var newSubProgram = sut.GetNewSubprogramName(subProgram);

            var content = File.ReadAllLines(subProgram.SubProgramNameWithDir);

            var result = content.Any(x => x.Contains(value));

            // Assert
            newSubProgram.Should().NotBeNullOrEmpty();
            content.Should().NotBeNullOrEmpty();
            result.Should().BeTrue();
        }

        private static ConvertMainProgram ConvertToHSTM300(string newNamePrg)
        {
            var myConvertMainProgram = new ConvertMainProgram()
            {
                NewProgramName = newNamePrg,
                AddPreload = true,
                AddRaport = false,
                ChangeNameProgram = true,
                DeletePreload = true,
                MachineType = MachineEnum.HSTM300,
                ProgramName = _mainprogramHSTM300,
                DeleteRaport = false,
                OrgMachine = MachineEnum.HSTM300.ToString(),
                ReplaceCycleTool = true,
                ReplaceToolCycle = false,
                TemplateMainProgram = ""
            };
            return myConvertMainProgram;
        }

        [Theory]
        [InlineData("; MACHINE     : HSTM_300HD_SIM840D_Py")]
        [InlineData("CYCLE832(")]
        [InlineData(";RAPORT")]
        [InlineData("; (nastepne narzedzie)")]
        public void CheckSubProgramForHSTM300HDAfterConvert_ShoudReturnAllChanges_WhenNotFail(string value)
        {
            // Arrange
            var newNamePrg = "Dtest1";
            ConvertMainProgram myConvertMainProgram = ConvertToHSTM300HD(newNamePrg);

            var sut = new ConvertSubProgramsService(myConvertMainProgram);

            //kasowanie pliku
            var outPutDir = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName);
            if (Directory.Exists(outPutDir))
                Directory.Delete(outPutDir, true);

            sut.FixSubPrograms();

            var newSubPrg = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName,
                myConvertMainProgram.NewProgramName + "49.SPF");

            var subProgram = new SubProgram()
            {
                SubProgramNameWithDir = newSubPrg,
            };

            var newSubProgram = sut.GetNewSubprogramName(subProgram);

            var content = File.ReadAllLines(subProgram.SubProgramNameWithDir);

            var result = content.Any(x => x.Contains(value));

            // Assert
            newSubProgram.Should().NotBeNullOrEmpty();
            content.Should().NotBeNullOrEmpty();
            result.Should().BeTrue();
        }

        private static ConvertMainProgram ConvertToHSTM300HD(string newNamePrg)
        {
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
                TemplateMainProgram = ""
            };
            return myConvertMainProgram;
        }

        [Theory]
        [InlineData("; MACHINE     : HSTM_500M_SIM840D_Py")]
        [InlineData("TOL(")]
        [InlineData("RAPORT")]
        [InlineData("M346")]
        public void CheckSubProgramForHSTM500MAfterConvert_ShoudReturnAllChanges_WhenNotFail(string value)
        {
            // Arrange
            var newNamePrg = "Ctest1";
            ConvertMainProgram myConvertMainProgram = ConvertToHSTM500M(newNamePrg);

            var sut = new ConvertSubProgramsService(myConvertMainProgram);

            //kasowanie pliku
            var outPutDir = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName);
            if (Directory.Exists(outPutDir))
                Directory.Delete(outPutDir, true);

            sut.FixSubPrograms();

            var newSubPrg = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName,
                myConvertMainProgram.NewProgramName + "11.SPF");

            var subProgram = new SubProgram()
            {
                SubProgramNameWithDir = newSubPrg,
            };

            var newSubProgram = sut.GetNewSubprogramName(subProgram);

            var content = File.ReadAllLines(subProgram.SubProgramNameWithDir);

            var result = content.Any(x => x.Contains(value));

            // Assert
            newSubProgram.Should().NotBeNullOrEmpty();
            content.Should().NotBeNullOrEmpty();
            result.Should().BeTrue();
        }

        private static ConvertMainProgram ConvertToHSTM500M(string newNamePrg)
        {
            var myConvertMainProgram = new ConvertMainProgram()
            {
                NewProgramName = newNamePrg,
                AddPreload = false,
                AddRaport = false,
                ChangeNameProgram = true,
                DeletePreload = true,
                MachineType = MachineEnum.HSTM500M,
                ProgramName = _mainprogramHSTM300,
                DeleteRaport = true,
                OrgMachine = MachineEnum.HSTM300.ToString(),
                ReplaceCycleTool = true,
                ReplaceToolCycle = false,
                TemplateMainProgram = ""
            };
            return myConvertMainProgram;
        }

        [Theory]
        [InlineData("; MACHINE     : HSTM_500_SIM840D_Py")]
        [InlineData("CYCLE832(")]
        [InlineData(";RAPORT")]
        public void CheckSubProgramForHSTM500HDAfterConvert_ShoudReturnAllChanges_WhenNotFail(string value)
        {
            // Arrange
            var newNamePrg = "Btest1";
            ConvertMainProgram myConvertMainProgram = ConvertToHSTM500HD(newNamePrg);

            var sut = new ConvertSubProgramsService(myConvertMainProgram);

            //kasowanie pliku
            var outPutDir = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName);
            if (Directory.Exists(outPutDir))
                Directory.Delete(outPutDir, true);

            sut.FixSubPrograms();

            var newSubPrg = Path.Combine(@"c:/tempnc",
                myConvertMainProgram.NewProgramName,
                myConvertMainProgram.NewProgramName + "49.SPF");

            var subProgram = new SubProgram()
            {
                SubProgramNameWithDir = newSubPrg,
            };

            var newSubProgram = sut.GetNewSubprogramName(subProgram);

            var content = File.ReadAllLines(subProgram.SubProgramNameWithDir);

            var result = content.Any(x => x.Contains(value));

            // Assert
            newSubProgram.Should().NotBeNullOrEmpty();
            content.Should().NotBeNullOrEmpty();
            result.Should().BeTrue();
        }

        private static ConvertMainProgram ConvertToHSTM500HD(string newNamePrg)
        {
            var myConvertMainProgram = new ConvertMainProgram()
            {
                NewProgramName = newNamePrg,
                AddPreload = true,
                AddRaport = false,
                ChangeNameProgram = true,
                DeletePreload = false,
                MachineType = MachineEnum.HSTM500,
                ProgramName = _mainprogramHSTM300,
                DeleteRaport = true,
                OrgMachine = MachineEnum.HSTM300.ToString(),
                ReplaceCycleTool = false,
                ReplaceToolCycle = true,
                TemplateMainProgram = ""
            };
            return myConvertMainProgram;
        }
    }
}
