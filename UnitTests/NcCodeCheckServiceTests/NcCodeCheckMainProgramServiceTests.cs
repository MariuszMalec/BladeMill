using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace UnitTests.NcCodeCheckServiceTests
{
    public class NcCodeCheckMainProgramServiceTests : IClassFixture<NcCodeCheckService>
    {
        private string _mainprogramHuron = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "8312701.NC");
        private string _mainprogramHSTM500HD = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "B01143001.MPF");
        private string _mainprogramAvia = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "00000061.MPF");
        private string _mainprogramHSTM500M = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "C99999901.MPF");
        private string _mainprogramHSTM300 = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData", "A88888801.MPF");

        public NcCodeCheckMainProgramServiceTests(NcCodeCheckService sut)
        {
            _sut = sut;
        }
        private NcCodeCheckService _sut = new NcCodeCheckService();

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_A_DC_360)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        [InlineData(CheckingNcOperationEnum.Check_M30)]
        [InlineData(CheckingNcOperationEnum.Check_L9006)]
        [InlineData(CheckingNcOperationEnum.Check_TRANS_for_drilling)]
        [InlineData(CheckingNcOperationEnum.Check_EVENT_1)]
        [InlineData(CheckingNcOperationEnum.Check_EVENT_2)]
        [InlineData(CheckingNcOperationEnum.Check_EVENT_3)]
        [InlineData(CheckingNcOperationEnum.Check_duplicate_tools)]
        public async void FindErrorsInMainProgram_ForHURON_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            await _sut.FindErrorsInNcCode(_mainprogramHuron);
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
        [InlineData(CheckingNcOperationEnum.Check_preload)]
        [InlineData(CheckingNcOperationEnum.Check_GOTO)]
        public async void FindErrorsInMainProgram_ForHSTM500HD_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            await _sut.FindErrorsInNcCode(_mainprogramHSTM500HD);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_preload)]
        public async void FindErrorsInMainProgram_ForHSTM500M_ReturnFalse_WhenMethodIsCall(CheckingNcOperationEnum checkMessage)
        {
            await _sut.FindErrorsInNcCode(_mainprogramHSTM500M);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.False(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_preload)]
        public async void FindErrorsInMainProgram_ForHSTM300_ReturnFalse_WhenMethodIsCall(CheckingNcOperationEnum checkMessage)
        {
            await _sut.FindErrorsInNcCode(_mainprogramHSTM300);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.False(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_Spindle)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError)]
        [InlineData(CheckingNcOperationEnum.CheckSyntaxError_in_TRANS)]
        [InlineData(CheckingNcOperationEnum.Check_M30)]
        [InlineData(CheckingNcOperationEnum.Check_E_ZDARZ_1)]
        [InlineData(CheckingNcOperationEnum.Check_E_ZDARZ_2)]
        [InlineData(CheckingNcOperationEnum.Check_G41_G42_G40)]
        [InlineData(CheckingNcOperationEnum.Check_4_or_5_axis_if_G41)]
        public async void FindErrorsInMainProgram_ForAvia_ReturnFalse_WhenMethodIsNotCall(CheckingNcOperationEnum checkMessage)
        {
            await _sut.FindErrorsInNcCode(_mainprogramAvia);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.True(result);
        }

        [Theory]
        [InlineData(CheckingNcOperationEnum.Check_preload)]
        [InlineData(CheckingNcOperationEnum.Check_TRAORI)]
        [InlineData(CheckingNcOperationEnum.Check_M348)]
        public async void FindErrorsInMainProgram_ForAvia_ReturnFalse_WhenMethodIsCall(CheckingNcOperationEnum checkMessage)
        {
            await _sut.FindErrorsInNcCode(_mainprogramAvia);
            var messages = _sut.GetAllErrors();
            var result = messages.Any(s => s.Message.Contains(checkMessage.ToString()));
            Assert.False(result);
        }
    }
}
