using Serilog;
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace BladeMillWithExcel.Logic.Models
{
    public class ToolsXml
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

        private readonly string _toolsXmlFile;

        public ToolsXml(string toolsXmlFile)
        {
            _toolsXmlFile = toolsXmlFile;
            AIRFOILTYPE = GetFromFileValue("AIRFOILTYPE");
            BF = double.Parse(GetFromFileValue("BF"));
            BFISOTOL = GetFromFileValue("BFISOTOL");
            BFSYMTOL = GetFromFileValue("BFSYMTOL");
            BH = GetFromFileValue("BH");
            BHISOTOL = GetFromFileValue("BHISOTOL");
            BHSYMTOL = GetFromFileValue("BHSYMTOL");
            BLADEORIENTATION = GetFromFileValue("BLADEORIENTATION");
            BMDTYPE = GetFromFileValue("BMDTYPE");
            BMTemplate = bool.Parse(GetFromFileValue("BMTemplate"));
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
                    return ValidateResult(value);
                }
                Log.Warning($"Brak pliku {_toolsXmlFile}!");
                return $"{value}";
            }
            catch (Exception e)
            {
                throw new Exception("check function GetFromFileValue", e);
            }
        }

        private string ValidateResult(string value)
        {
            var result = $"{value.Replace(" ", "").Replace("?", "0")}";
            if (result == "")
                return "0";
            if (result == "NO")
                return "False";
            if (result == "YES")
                return "True";
            return result;
        }

    }
}
