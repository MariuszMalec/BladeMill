using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using Serilog;
using System;
using System.IO;
using System.Xml.XPath;
using System.Xml;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane z pliku ...tools.xml probowac to wylaczyc 
    /// zastapic modelem ToolXml!!
    /// </summary>
    public class ToolsXmlFile
    {
        public string AIRFOILTYPE { get; set; }
        public double BF { get; set; }
        public string BFISOTOL { get; set; }
        public string BFSYMTOL { get; set; }
        public string BH { get; set; }
        public string BHISOTOL { get; set; }
        public string BHSYMTOL { get; set; }
        public string BLADEORIENTATION { get; set; }
        public string BMDTYPE { get; set; }
        public bool BMTemplate { get; set; }
        public double BROH { get; set; }
        public string CLAMPMETHOD { get; set; }
        public string CONTROL { get; set; }
        public DateTime DATE { get; set; }
        public double DFA { get; set; }
        public double DMFB { get; set; }
        public double DMVB { get; set; }
        public string DWGNR { get; set; }
        public string DWGREV { get; set; }
        public double DZA { get; set; }
        public string FIGSHROUD { get; set; }
        public string FIG_N { get; set; }
        public string FIRSTNAME { get; set; }
        public bool FOURHOOK { get; set; }
        public double HDD { get; set; }
        public double HROH { get; set; }
        public string LASTNAME { get; set; }
        public double LROH { get; set; }
        public string MACHINE { get; set; }
        public string MATERIAL { get; set; }
        public string NAMEPROJECT { get; set; }
        public string NBEA { get; set; }
        public string PRGNUMBER { get; set; }
        public string PROJECTNO { get; set; }
        public string SHROUDDRAWING { get; set; }
        public string STAGENO { get; set; }
        public string last_ident { get; set; }

        private XMLToolService _xmlToolService = new XMLToolService();
        private PathDataBase _data = new PathDataBase();

        private string _toolsXmlFile;
        public object value;

        /// <summary>
        /// Bierze dane z biezacego current.xml
        /// </summary>
        public ToolsXmlFile()//TODO do wyrzucenia
        {
            _toolsXmlFile = _data.GetFileCurrentToolsXml();
            PRGNUMBER = GetFromFileValue("PRGNUMBER");
            MACHINE = GetFromFileValue("MACHINE");
        }

        public ToolsXmlFile(string toolxmlfile)
        {
            _toolsXmlFile = toolxmlfile;

            bool ignore = true;

            AIRFOILTYPE = GetFromFileValue("AIRFOILTYPE");
            BF = double.Parse(GetFromFileValue("BF"));
            BFISOTOL = GetFromFileValue("BFISOTOL");
            BFSYMTOL = GetFromFileValue("BFSYMTOL");
            BH = GetFromFileValue("BH");
            BHISOTOL = GetFromFileValue("BHISOTOL");
            BHSYMTOL = GetFromFileValue("BHSYMTOL");
            BLADEORIENTATION = GetFromFileValue("BLADEORIENTATION");
            BMDTYPE = GetFromFileValue("BMDTYPE");
            BMTemplate = bool.TryParse(GetFromFileValue("BMTemplate"), out ignore);
            BROH = double.Parse(GetFromFileValue("BROH"));
            CLAMPMETHOD = GetFromFileValue("CLAMPMETHOD");
            CONTROL = GetFromFileValue("CONTROL");
            DATE = DateTime.Parse(GetFromFileValue("DATE"));
            DFA = double.Parse(GetFromFileValue("DFA"));
            DMFB = double.Parse(GetFromFileValue("DMFB"));
            DMVB = double.Parse(GetFromFileValue("DMVB"));
            DWGNR = GetFromFileValue("DWGNR");
            DWGREV = GetFromFileValue("DWGREV");
            DZA = double.Parse(GetFromFileValue("DZA"));
            FIGSHROUD = GetFromFileValue("FIGSHROUD");
            FIG_N = GetFromFileValue("FIG_N");
            FIRSTNAME = GetFromFileValue("FIRSTNAME");
            FOURHOOK = bool.Parse(GetFromFileValue("FOURHOOK"));
            HDD = double.Parse(GetFromFileValue("HDD"));
            HROH = double.Parse(GetFromFileValue("HROH"));
            LASTNAME = GetFromFileValue("LASTNAME");
            LROH = double.Parse(GetFromFileValue("LROH"));
            MACHINE = GetFromFileValue("MACHINE");
            MATERIAL = GetFromFileValue("MATERIAL");
            NAMEPROJECT = GetFromFileValue("NAMEPROJECT");
            NBEA = GetFromFileValue("NBEA");
            PRGNUMBER = GetFromFileValue("PRGNUMBER");
            PROJECTNO = GetFromFileValue("PROJECTNO");
            SHROUDDRAWING = GetFromFileValue("SHROUDDRAWING");
            STAGENO = GetFromFileValue("STAGENO");
            last_ident = GetFromFileValue("last_ident");
        }


        private string GetFromFileValue(string findtext)
        {
            try
            {
                string navigator = "/TOOLLIST";
                string element = findtext;
                var value = "-";
                if (File.Exists(_toolsXmlFile))
                {
                    //create list	
                    XmlDocument document = new XmlDocument();
                    document.Load(_toolsXmlFile);
                    XPathNavigator navigator2 = document.CreateNavigator();
                    XPathNodeIterator nodes2 = navigator2.Select(navigator);
                    //				
                    string line;
                    while (nodes2.MoveNext())
                    {
                        line = nodes2.Current.GetAttribute(element, "");
                        value = line;
                    }
                    return ValidateResult(value, findtext);
                }
                Log.Warning($"Brak pliku w ToolsXmlFile {_toolsXmlFile}! nie znaleziono {findtext}");
                return $"{value}";
            }
            catch (Exception e)
            {
                throw new Exception("check function GetFromFileValue", e);
            }
        }

        private string ValidateResult(string value, string findText)
        {
            var result = $"{value.Replace(" ", "").Replace("?", "0")}";
            if (result == "")
            { 
                return "0";
            }
            if (result == "NO")
                return "False";
            if (result == "YES")
                return "True";
            return result;
        }

        public string GetMainProgramFileFromCurrentToolsXml()
        {
            var _appService = new AppXmlConfService();
            var orderService = new BMOrder(_data.GetFileCurrentToolsXml());
            var mainProgram = Path.Combine(_appService.GetNcDir(), orderService.OrderName + "01.MPF");
            if (MACHINE == MachineEnum.HSTM1000.ToString())
            {
                mainProgram = Path.Combine(_appService.GetNcDir(), orderService.OrderName + "_01.mpf");//TODO do poprawy w PP
            }
            if (MACHINE == MachineEnum.HURON.ToString())
            {
                mainProgram = Path.Combine(_appService.GetNcDir(), orderService.OrderName + "01.NC");
            }
            return mainProgram;
        }
        public string GetMainProgramFileFromToolsXml(string mainProgramNameWithDir, string machine)
        {
            var mainDir = Path.GetDirectoryName(mainProgramNameWithDir);
            var mainName = Path.GetFileName(mainProgramNameWithDir).Replace(".tools.xml","");

            if (mainProgramNameWithDir.Contains("current.xml"))
            {
                mainName = PRGNUMBER;
            }

            var mainProgram = Path.Combine(mainDir, mainName + "01.MPF");

            if (machine == MachineEnum.HSTM1000.ToString())
            {
                mainProgram = Path.Combine(mainDir, mainName + "_01.mpf");//TODO do poprawy w PP
            }
            if (machine == MachineEnum.HURON.ToString())
            {
                mainProgram = Path.Combine(mainDir, mainName + "01.NC");
            }
            return mainProgram;
        }
    }
}
