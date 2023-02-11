using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using System.Collections.Generic;

namespace BladeMill.BLL.SourceData
{
    /// <summary>
    /// Tworzenie danych bazowych dla inputdata.xml
    /// </summary>
    public class XMLDataBase
    {
        public static IEnumerable<InputDataXml> CreateInputDataXml()
        {
            var _datas = new InputDataXml
            {
                runconfiguration = "True",
                runbm = "False",
                runcmm = "False",
                readxls = "False",
                millshroud = "False",
                middleTol = "False",
                admin = "False",
                createvcproject = "False",
                createraport = "False",
                clickcancel = "False",
                TypeBlade = "RTBFixedBlade",
                Prerawbox = "False",
                BMTemplate = "False",
                machine = "HSTM300HD",
                outfile = GetXmlFile(),
                catpart = @"C:\Clever\V300\BladeMill\data\RootEngDir\ZTGD906477 (IMR-007162)\ZTGD906477P1012_-.CATPart",
                xmlpart = @"C:\Clever\V300\BladeMill\data\RootEngDir\ZTGD906477 (IMR-007162)\ZTGD906477P1012_-_BMD.xml",
                xlspart = @"C:\Clever\V300\BladeMill\data\RootEngDir\ZTGD906477 (IMR-007162)\ZTGD906477P1012.xls",
                catpartfirst = "",
                catpartend = "",
                xmlpartfirst = "",
                xmlpartend = "",
                Clampingmethod = "GripZabierak",
                pinwelding = "false",
                RootMfgDir = "",
                BMTemplateFile = "",
                IsXML = "true",
                ClampFromTemplate = "",
                FIG_N = "",
                infile = "",
                selectlanguage = "pol",
                TypeOfProcess = TypeOfProcessEnum.PodBeben.ToString()
            };
            return new[] { _datas };
        }
        public static string GetXmlFile()
        {
            return @"C:\Temp\inputdata_Test.xml";
        }
    }
}
