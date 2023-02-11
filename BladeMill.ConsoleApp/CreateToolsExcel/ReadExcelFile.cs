using BladeMillWithExcel.Logic.Enums;
using BladeMillWithExcel.Logic.Models;
using BladeMillWithExcel.Logic.Services;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace BladeMill.ConsoleApp.CreateToolsExcel
{
    public static class ReadExcelFile
    {
        public static List<Technology> GetAll(string excelFile)
        {
            var excelService = new ExcelService(excelFile);
            return excelService.GetAll();
        }

        internal static Technology GetValue(TechnologyEnum bladeType, string excelFile, List<Technology> technologies)
        {
            var excelService = new ExcelService(excelFile);
            return excelService.GetValue(bladeType.ToString(), technologies);
        }
    }
}
