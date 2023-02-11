using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace UnitTests.NcCodeCheckServiceTests
{
    public class NcCodeCheckSubProgramServiceTests : IClassFixture<NcCodeCheckService>
    {
        private string _subprogramAvia = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "CZOLO_BANDAZ_ZGR.SPF");
        private string _subprogramHec = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "CZOLO_B_WK_SKOS.SPF");
        private string _subprogramHuron = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "8313368.nc");
        private string _subprogramHSTM300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888849.SPF");
        private string _subprogramHX151 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "1406149.SPF");
        private string _subprogramHSTM500HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B01134835.SPF");
        private string _subprogramHSTM500M = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "C00086035.SPF");
        private string _subprogramHSTM300HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "D99999935.SPF");
        private string _subprogramHSTM500HD_33N = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B011382_33N.spf");
        private string _subprogramHSTM500HD_33B = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B01147133B.SPF");


        public NcCodeCheckSubProgramServiceTests(NcCodeCheckService sut)
        {
            _sut = sut;
        }
        private NcCodeCheckService _sut = new NcCodeCheckService();

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_M6)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_G64)]
        public void FindErrorsInSubProgram_ForAvia_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramAvia, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_L300)]
        public void FindErrorsInSubProgram_ForHec_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHec, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_M6)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_E_ZDARZ_3)]
        [InlineData(CheckingNcOperationEnum.Check_TRAORI)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        public void FindErrorsInSubProgram_ForHSTM300_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHSTM300, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_M6)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_E_ZDARZ_3)]
        [InlineData(CheckingNcOperationEnum.Check_TRAORI)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        [InlineData(CheckingNcOperationEnum.Check_M348)]
        public void FindErrorsInSubProgram_ForHSTM500M_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHSTM500M, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_M6)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_E_ZDARZ_3)]
        [InlineData(CheckingNcOperationEnum.Check_TRAORI)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        public void FindErrorsInSubProgram_ForHSTM500HD_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHSTM500HD, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_M6)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_E_ZDARZ_3)]
        [InlineData(CheckingNcOperationEnum.Check_TRAORI)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        public void FindErrorsInSubProgram_ForHSTM300HD_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHSTM300HD, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }


        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_M6)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_TRAORI)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_X_R9)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_X_R3)]
        [InlineData(CheckingNcOperationEnum.Check_G54_D3)]
        [InlineData(CheckingNcOperationEnum.Check_G54_D1)]
        public void FindErrorsInSubProgram_ForHSTM500HD_33N_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHSTM500HD_33N, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_M6)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_TRAORI)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_X_R8)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_X_R4)]
        [InlineData(CheckingNcOperationEnum.Check_G54_D4)]
        [InlineData(CheckingNcOperationEnum.Check_G54_D2)]
        public void FindErrorsInSubProgram_ForHSTM500HD_33B_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHSTM500HD_33B, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M17)]
        [InlineData(CheckingNcOperationEnum.Check_M6)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_E_ZDARZ_3)]
        [InlineData(CheckingNcOperationEnum.Check_TRAORI)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        [InlineData(CheckingNcOperationEnum.Check_Lenght_of_tool)]
        public void FindErrorsInSubProgram_ForHX151_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHX151, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A_DC_360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M30)]
        [InlineData(CheckingNcOperationEnum.Check_L9006)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        public void FindErrorsInSubProgram_ForHURON_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            _sut.FindErrorsInSubProgram(_subprogramHuron, 1);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_limit_axis_Z, 590.0, 150.0, true)]
        [InlineData(CheckingNcOperationEnum.Check_limit_axis_Y, 590.0, 150.0, true)]
        public void FindErrorsLimitsNcCode_ForHURON_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage, double zMax, double yMin, bool check)
        {
            _sut.FindErrorsLimitsNcCode(_subprogramHuron, zMax, yMin, check);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

    }
}
