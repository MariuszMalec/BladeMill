using BladeMillWithExcel.Logic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace BladeMillWithExcel.Logic.Services
{
    public class ExcelOleService//TODO nie dziala oledb blad providera!
    {
        private string _excelFile;
        private string _pOCConnection;
        //private OleDbConnection _pOCcon;
        private static List<Technology> _allCnc;
        public ExcelOleService(string excelFile)
        {
            _excelFile = excelFile;
            if (File.Exists(excelFile))
            {
                //_pOCConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFile + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";
                //_pOCcon = oleDbConnection();
            }
        }
        public static void KillExcel()
        {
            var excelService = new ExcelService();
            excelService.KillSoftware("Excel", true);
        }
        //private OleDbConnection oleDbConnection()
        //{
        //    OleDbConnection _pOCcon = new OleDbConnection(_pOCConnection);
        //    return _pOCcon;
        //}

        public List<Technology> GetAll()
        {            
            if (File.Exists(_excelFile) && !_excelFile.Contains("KDT") && !_excelFile.Contains("P.XLS") && !_excelFile.Contains("P.xls"))
            {
                Console.WriteLine($"{_excelFile}");
                //OleDbCommand DANEcommand = new OleDbCommand();
                //DataTable CNCdt = new DataTable();
                //OleDbDataAdapter CNCCommand = new OleDbDataAdapter("select * from [CNC$] ", _pOCcon);
                //CNCCommand.Fill(CNCdt);
                //_allCnc = new List<Technology>();
                //for (int i = 4; i < 50; i++)
                //{
                //    string name = CNCdt.Rows[i][0].ToString();
                //    string value = CNCdt.Rows[i][1].ToString();
                //    if (name == null)
                //        break;
                //    _allCnc.Add(new Technology() { Id = i, Name = name, Value = value });
                //}
                //KillExcel();
                //return _allCnc;

                string POCConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _excelFile + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";
                Console.WriteLine(POCConnection);

                OleDbConnection POCcon = new OleDbConnection(POCConnection);
                DataTable CNCdt = new DataTable();
                OleDbDataAdapter CNCCommand = new OleDbDataAdapter("select * from [CNC$] ", POCcon);
                CNCCommand.Fill(CNCdt);

                _allCnc = new List<Technology>();
                for (int i = 4; i < 50; i++)
                {
                    string name = CNCdt.Rows[i][0].ToString();
                    string value = CNCdt.Rows[i][1].ToString();
                    if (name == null)
                        break;
                    _allCnc.Add(new Technology() { Id = i, Name = name, Value = value });
                }
                KillExcel();
                return _allCnc;


            }
            return default(List<Technology>);
        }
    }
}
