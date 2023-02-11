using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using BladeMillWithExcel.Logic.Services;
using Serilog;
using System;
using System.IO;

namespace BladeMill.ConsoleApp.CreateToolsExcel
{
    static class CreateToolListFromToolsXml
    {
        public static void Tests(string xmlFile, ILogger logger)
        {
            //kill excel
            var excelService = new ExcelService();
            excelService.KillSoftware("Excel", true);

            //get excel template
            var pathService = new PathDataBase();
            var excelTemplate = pathService.GetFileExcelTemplate();
            //Console.WriteLine(excelTemplate);

            //set new name excel
            var dirNew = Path.GetDirectoryName(xmlFile);
            //Console.WriteLine(dirNew);
            var order = Path.GetFileNameWithoutExtension(xmlFile).Replace(".tools", "");
            var newExcelFile = Path.Combine(dirNew, order + ".xlsm");
            excelService.DeleteExcelFile(newExcelFile);
            excelService.CopyExcelTemplate(excelTemplate, newExcelFile);

            //get tools
            var toolXmlService = new BladeMillWithExcel.Logic.Services.ToolXmlService();
            var incService = new BladeMillWithExcel.Logic.Services.ToolService(toolXmlService);
            var tools = incService.LoadToolsFromFile(xmlFile);

            //get varpool
            var varpoolService = new XMLVarpoolService();
            var varpoolxmlfile = varpoolService.GetCurrentVarpoolFile();

            excelService.startExcell(newExcelFile, tools, xmlFile, varpoolxmlfile);
        }
    }
}
