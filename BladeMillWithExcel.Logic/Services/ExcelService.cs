using BladeMillWithExcel.Logic.Models;
using Microsoft.Office.Interop.Excel;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;

namespace BladeMillWithExcel.Logic.Services
{
    public class ExcelService
    {
        private ILogger _logger;
        private static ToolXmlService _toolXmlService;
        private Application _appl;
        private string _excelFile;
        private Workbook _workbook;
        Worksheet _excelWorksheetCnc;
        private Sheets _excelSheets;
        private List<Technology> _allCnc;

        public ExcelService()
        {
            _toolXmlService = new ToolXmlService();
        }
        public ExcelService(string excelFile)
        {
            KillSoftware("Excel", true);
            _excelFile = excelFile;
            Application _appl = new Application();
            _appl.DisplayAlerts = false;
            Workbook _workbook = _appl.Workbooks.Open(_excelFile);
            Sheets _excelSheets = _workbook.Worksheets;
            string cncSheet = "CNC";
            _excelWorksheetCnc = (Worksheet)_excelSheets.get_Item(cncSheet);
            _toolXmlService = new ToolXmlService();
            _allCnc = new List<Technology>();
        }
        public void CopyExcelTemplate(string excelTemplate, string newExcelFile)
        {
            if (File.Exists(excelTemplate))
            {
                if (!File.Exists(newExcelFile))
                {
                    File.Copy(excelTemplate, newExcelFile, true);
                }
            }
        }
        public void DeleteExcelFile(string newExcelFile)
        {
            if (File.Exists(newExcelFile))
            {
                try
                {
                    File.Delete(newExcelFile);
                }
                catch { throw new IOException($"Plik jest uzywany! Zamknij plik {newExcelFile}"); }
            }
        }
        public void startExcell(string newExcelfile, List<Tool> tools, string toolsXmlFile, string varpoolFile)
        {
            try
            {
                if (File.Exists(newExcelfile))
                {
                    Application appl = new Application();
                    appl.DisplayAlerts = false;
                    Workbook workbook = appl.Workbooks.Open(newExcelfile);
                    Sheets excelSheets = workbook.Worksheets;

                    //-------------------------------------------------------------
                    // POPRAW NAZWE PODPROGRAMU
                    //-------------------------------------------------------------
                    tools.ForEach(t => t.BatchFile = t.BatchFile.Replace(t.BatchFile, GetShortNameSubProgram(t.BatchFile, toolsXmlFile, varpoolFile)));

                    //-------------------------------------------------------------
                    // WYPELNIJ KARTE NARZEDZI
                    //-------------------------------------------------------------
                    string currentSheet = "KARTA NARZEDZI OBROBCZYCH";
                    Worksheet excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelSheets.get_Item(currentSheet);
                    int startRow = 7;
                    if (tools.Count > 0)
                    {
                        SetDataForTools(tools, excelWorksheet, startRow);
                    }
                    //-------------------------------------------------------------
                    // WYPELNIJ KARTE UWAGI
                    //-------------------------------------------------------------
                    currentSheet = "POMOC";
                    Worksheet excelWorksheetPomoc = (Worksheet)excelSheets.get_Item(currentSheet);
                    if (File.Exists(toolsXmlFile))//wybranytoolsxml.Text
                    {
                        SetDataFromToolsXmlFile(toolsXmlFile, excelWorksheetPomoc);
                    }
                    else
                    {
                        throw new Exception($"{toolsXmlFile} doesn't exist tools.xml!");
                    }
                    if (File.Exists(varpoolFile))
                    {
                        var varpoolService = new VarpoolXmlFile(varpoolFile);
                        excelWorksheetPomoc.Cells[1, 2] = varpoolService.ProjectName;
                        if (!File.Exists(toolsXmlFile))
                        {
                            SetDataFromVarpoolFile(varpoolFile, excelWorksheetPomoc);
                        }
                    }
                    //------------------------------------------------------------------------
                    //start makro excela
                    //------------------------------------------------------------------------
                    try
                    {
                            workbook.Application.Run("Print1_Click");
                    }
                    catch// (Exception e)
                    {
                        //TODO ja wyrzucic blad
                        //throw new Exception("Uwaga nie dziala makro Print1_Click sprawdz template excela Nowa Lista Narzedziowa.xlsm! ", e);
                        //_logger.Warning("Uwaga nie dziala makro Print1_Click sprawdz template excela Nowa Lista Narzedziowa.xlsm");
                    }
                    workbook.SaveAs(newExcelfile);
                    Console.WriteLine($"Plik excel zostal zapiasany tutaj : {newExcelfile}");
                    workbook.Close(0);
                    appl.Quit();
                }
            }
            catch (Exception e)
            {
                throw new Exception("check function startExcell! ", e);
            }
        }

        private string GetShortNameSubProgram(string file, string toolsXmlFile, string varpoolFile)
        {
            if (File.Exists(toolsXmlFile))
            {
                if (File.Exists(toolsXmlFile))
                {
                    var order = _toolXmlService.GetFromFileValue(toolsXmlFile, "PRGNUMBER");
                    var name = Path.GetFileName(file).Replace(".spf", "").Replace(".SPF", "").Replace(order, "");
                    return name;
                }
                return string.Empty;
            }
            else if (!File.Exists(toolsXmlFile))
            {
                if (File.Exists(varpoolFile))
                {
                    var varpoolXmlFile = new VarpoolXmlFile(varpoolFile);
                    var name = Path.GetFileName(file).Replace(".spf", "").Replace(".SPF", "").Replace(varpoolXmlFile.OrderName, "");
                    return name;
                }
                var subprogram = Path.GetFileName(file).Replace(".SPF", "").Replace(".spf", "");
                return subprogram;
            }
            else
            {
                return string.Empty;
            }
        }

        private void SetDataFromVarpoolFile(string varpoolFile, Worksheet excelWorksheetPomoc)
        {
            var dataFromVarpool = new VarpoolXmlFile(varpoolFile);
            excelWorksheetPomoc.Cells[13, 2] = dataFromVarpool.OrderName;
            excelWorksheetPomoc.Cells[5, 2] = dataFromVarpool.MfgSystem;
            excelWorksheetPomoc.Cells[15, 2] = dataFromVarpool.airfoiltype;
            excelWorksheetPomoc.Cells[14, 2] = dataFromVarpool.ClampMethod.Replace("&amp;", "&");
            excelWorksheetPomoc.Cells[9, 2] = dataFromVarpool.BladeMaterial;
            excelWorksheetPomoc.Cells[12, 2] = dataFromVarpool.BmdType;
            excelWorksheetPomoc.Cells[4, 2] = dataFromVarpool.ProjectNameAndPassword;
            excelWorksheetPomoc.Cells[10, 2] = Path.GetFileName(dataFromVarpool.BpmFilePath).Replace(".CATPart", "");
            excelWorksheetPomoc.Cells[2, 2] = dataFromVarpool.BladeOrientation;
            excelWorksheetPomoc.Cells[28, 2] = dataFromVarpool.FIG_N;
            excelWorksheetPomoc.Cells[17, 2] = dataFromVarpool.DFA;
            excelWorksheetPomoc.Cells[38, 2] = dataFromVarpool.BROH;
            excelWorksheetPomoc.Cells[39, 2] = dataFromVarpool.HROH;
            excelWorksheetPomoc.Cells[40, 2] = dataFromVarpool.LROH;
            excelWorksheetPomoc.Cells[43, 2] = dataFromVarpool.Ltol_HE;
            excelWorksheetPomoc.Cells[44, 2] = dataFromVarpool.Utol_HE;
        }

        private static void SetDataForTools(List<Tool> tools, Worksheet excelWorksheet, int startRow)
        {
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 1] = t.BatchFile);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 3] = t.Description);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 5] = t.ToolID);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 7] = t.ToolSet);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 8] = t.Offsets);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 9] = t.Toollen);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 10] = t.ToolDiam);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 11] = t.ToolCrn);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 12] = t.Spindle);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 13] = t.Feedrate);
            tools.ForEach(t => excelWorksheet.Cells[startRow + t.Id, 15] = t.MaxMillTime);
        }
        private static void SetDataFromToolsXmlFile(string toolsXmlFile, Worksheet excelWorksheetPomoc)
        {
            excelWorksheetPomoc.Cells[13, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "PRGNUMBER");
            excelWorksheetPomoc.Cells[5, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "MACHINE") + " " + _toolXmlService.GetFromFileValue(toolsXmlFile, "CONTROL");
            excelWorksheetPomoc.Cells[15, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "AIRFOILTYPE");
            excelWorksheetPomoc.Cells[14, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "CLAMPMETHOD");
            excelWorksheetPomoc.Cells[9, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "MATERIAL");
            excelWorksheetPomoc.Cells[12, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BMDTYPE");
            excelWorksheetPomoc.Cells[4, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "PROJECTNO");
            excelWorksheetPomoc.Cells[6, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "FIRSTNAME") + " " + _toolXmlService.GetFromFileValue(toolsXmlFile, "LASTNAME");
            excelWorksheetPomoc.Cells[10, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "DWGNR");
            excelWorksheetPomoc.Cells[2, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BLADEORIENTATION");
            excelWorksheetPomoc.Cells[3, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "STAGENO");
            excelWorksheetPomoc.Cells[16, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "FOURHOOK");
            excelWorksheetPomoc.Cells[27, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "FIGSHROUD");
            excelWorksheetPomoc.Cells[28, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "FIG_N");
            excelWorksheetPomoc.Cells[17, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "DFA");
            excelWorksheetPomoc.Cells[29, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BF");
            excelWorksheetPomoc.Cells[30, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BH");
            excelWorksheetPomoc.Cells[31, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BFSYMTOL");
            excelWorksheetPomoc.Cells[32, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BHSYMTOL");
            excelWorksheetPomoc.Cells[33, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BFISOTOL");
            excelWorksheetPomoc.Cells[34, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BHISOTOL");
            excelWorksheetPomoc.Cells[35, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "HDD"); ;
            excelWorksheetPomoc.Cells[36, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BF_PROG"); ;
            excelWorksheetPomoc.Cells[37, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BH_PROG");
            excelWorksheetPomoc.Cells[38, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "BROH");
            excelWorksheetPomoc.Cells[39, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "HROH");
            excelWorksheetPomoc.Cells[40, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "LROH");
            excelWorksheetPomoc.Cells[43, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "Ltol_HE");
            excelWorksheetPomoc.Cells[44, 2] = _toolXmlService.GetFromFileValue(toolsXmlFile, "Utol_HE");
        }
        public void KillSoftware(string soft, bool zabijbezpytania)
        {
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName(soft);
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        if (zabijbezpytania == true)
                        {
                            p.Kill();
                        }
                        else
                        {
                            //TODO zapytac czy kilowac , alae jak?
                            if (zabijbezpytania == true)
                            {
                                p.Kill();
                            }
                            else
                            {
                                Environment.Exit(1);
                            }
                        }
                    }
                    catch { }
                }
            }
        }
        
        public Technology GetValue(string search, List<Technology> technologies)
        {
            KillSoftware("Excel", true);
            if (File.Exists(_excelFile))
            {
                var technology = technologies.Where(p => p.Name == search).FirstOrDefault();
                return technology;
            }
            return default(Technology);
        }

        public List<Technology> GetAll()
        {
            _allCnc.Clear();
            if (File.Exists(_excelFile))
            {     
                for (int i = 4; i < 50; i++)
                {
                    string name = (_excelWorksheetCnc.Cells[i, 1] as Range).Value2;
                    string value = Convert.ToString((_excelWorksheetCnc.Cells[i, 2] as Range).Value2) ?? default;
                    if (name == null)
                        break;
                    _allCnc.Add(new Technology() { Id=i, Name = name , Value = value });
                }
                KillSoftware("Excel", true);
                return _allCnc;
            }
            return default(List<Technology>);
        }
    }
}
