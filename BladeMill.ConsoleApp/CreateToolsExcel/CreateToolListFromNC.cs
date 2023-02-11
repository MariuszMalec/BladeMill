using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using BladeMillWithExcel.Logic.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.ConsoleApp.CreateToolsExcel
{
    static class CreateToolListFromNC
    {
        public static void Tests(string ncFile, ILogger logger)
        {
            //kill excel
            var excelService = new ExcelService();
            excelService.KillSoftware("Excel", true);

            //get excel template
            var pathService = new PathDataBase();
            var excelTemplate = pathService.GetFileExcelTemplate();
            //Console.WriteLine(excelTemplate);

            //get machine 
            var machineServiceFactory = new MachineServiceFactory();
            var machine = machineServiceFactory.CreateMachine(BLL.Enums.TypeOfFile.ncFile).GetMachine(ncFile).MachineName;


            var toolxmlfile = string.Empty;
            if (machine.Contains("HURON"))
            {
                toolxmlfile = ncFile.Replace(".NC", "tools.xml").Replace("01", ".");
            }
            else
            {
                toolxmlfile = ncFile.Replace("MPF", "tools.xml").Replace("01.", ".");
            }
            var dirNew = Path.GetDirectoryName(toolxmlfile);
            var order = Path.GetFileNameWithoutExtension(toolxmlfile);

            var newExcelFile = string.Empty;
            if (machine.Contains("HURON"))
            {
                newExcelFile = Path.Combine(dirNew, order.Replace("tools", "") + "xlsm");
            }
            else
            {
                newExcelFile = Path.Combine(dirNew, order.Replace("tools", "") + "xlsm");
            }

            excelService.DeleteExcelFile(newExcelFile);
            excelService.CopyExcelTemplate(excelTemplate, newExcelFile);
            var varpoolService = new XMLVarpoolService();
            var varpoolxmlfile = varpoolService.GetVarpolFileAccNcFile(ncFile);

            var ncService = new BladeMillWithExcel.Logic.Services.ToolNcService();
            var incService = new BladeMillWithExcel.Logic.Services.ToolService(ncService);
            var toolsFromNc = incService.LoadToolsFromFile(ncFile);
            if (machine.Contains("HURON"))
            {
                toolsFromNc = ncService.LoadNCMultiple(ncFile);
            }

            excelService.startExcell(newExcelFile, toolsFromNc, toolxmlfile, varpoolxmlfile);
        }
    }
}
