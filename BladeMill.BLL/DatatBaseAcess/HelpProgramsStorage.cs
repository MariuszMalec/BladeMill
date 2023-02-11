using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using System.Collections.Generic;

namespace BladeMill.BLL.DatatBaseAcess
{
    public class HelpProgramsStorage
    {
        private static PathDataBase _pathService = new PathDataBase();

        public static IEnumerable<ProgramExe> programs = new List<ProgramExe>()
        {
                new ProgramExe(1, _pathService.GetDirHtml(), "ProgramyPomocnicze", ExeEnum.Helpprogramypomocnicze.ToString(),".html"),
                new ProgramExe(2, _pathService.GetDirAutomationScriptLauncher(), "", ExeEnum.AutomationScriptLauncher.ToString(), ".exe"),
                new ProgramExe(3, _pathService.GetDirTask(), ExeEnum.OrderTransfer3.ToString(), ExeEnum.OrderTransfer3.ToString(),".exe"),
                new ProgramExe(4, _pathService.GetDirTask(), ExeEnum.CheckBladeMill.ToString(), ExeEnum.CheckBladeMill.ToString(),".exe"),
                new ProgramExe(5, _pathService.GetDirTask(), ExeEnum.CheckBladeMill.ToString(), ExeEnum.CheckBladeMill.ToString(),".exe"),
                new ProgramExe(6, _pathService.GetDirTask(), ExeEnum.CheckNcFiles.ToString(), ExeEnum.CheckNcFiles.ToString(),".exe"),
                new ProgramExe(7, _pathService.GetDirTask(), ExeEnum.CreateToollist.ToString(), ExeEnum.CreateToollist.ToString(),".exe"),
                new ProgramExe(8, _pathService.GetDirTask(), ExeEnum.CreateVcProject.ToString(), ExeEnum.CreateVcProject.ToString(),".exe"),
                new ProgramExe(9, _pathService.GetCleverHome(), "", "Start.BladeMill",".bat"),
                new ProgramExe(10, _pathService.GetDirTask(), ExeEnum.AppKiller.ToString(), ExeEnum.AppKiller.ToString(),".exe"),
                new ProgramExe(11, _pathService.GetDirTask(), ExeEnum.CreateListToolsFromNc.ToString(), ExeEnum.CreateListToolsFromNc.ToString(),".Wpf.exe"),
                new ProgramExe(12, _pathService.GetDirTask(), ExeEnum.ConverterNcCodes.ToString(), ExeEnum.ConverterNcCodes.ToString(),".Wpf.exe"),
                new ProgramExe(13, _pathService.GetDirTask(), "", ExeEnum.ProgramLaczacyDlaHurona.ToString(),".exe"),
                new ProgramExe(14, _pathService.GetDirTask(), "", ExeEnum.PrzerobProgramy.ToString(),".exe"),
                new ProgramExe(15, _pathService.GetDirTask(), "", ExeEnum.KopiowanieTechnologi3.ToString(),".exe"),
                new ProgramExe(16, _pathService.GetDirTask(), "", ExeEnum.PorownanieTechnologi.ToString(),".exe"),
                new ProgramExe(17, _pathService.GetDirTask(), "", ExeEnum.Export_CATPart_To_IGS.ToString(),".xlsm"),
                new ProgramExe(18, _pathService.GetDirTask(), "", ExeEnum.DrukowanieListNarzedzi.ToString(),".exe"),
                new ProgramExe(19, _pathService.GetDirTask(), "", ExeEnum.ShowRowsForProject.ToString(),".exe"),
                new ProgramExe(20, _pathService.GetDirTask(), "", ExeEnum.WyczyscTempNc.ToString(),".exe"),
                new ProgramExe(21, _pathService.GetDirTask(), "", ExeEnum.ReadXMLsFiles.ToString(),".exe"),
                new ProgramExe(22, _pathService.GetDirTask(), "", ExeEnum.FixVarpool.ToString(),".exe"),
                new ProgramExe(23, _pathService.GetNxDir(), "", "BM3.21_" + ExeEnum.nx19.ToString(),".bat")
        };
    }
}
