using BladeMill.BLL.SourceData;
using System.IO;
using System.Xml;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane z pliku konfiguracji BM Application.xml.conf
    /// </summary>
    public class AppXmlConfFiles
    {
        public string XmlFile { get; set; }
        public string TOOL_LIST { get; set; }
        public string MACHINE_LIST { get; set; }
        public string APP_RET_LIST { get; set; }
        public string MFG_PROCESS_LIST { get; set; }
        public string TECHNOLOGY_LIST { get; set; }
        public string MEASURINGLAW_LIST { get; set; }
        public string AUXCOMMAND_LIST { get; set; }
        public string SUPPORT_SITE { get; set; }
        public AppXmlConfFiles(string appXmlConfFile)
        {
            PathDataBase pathDataBase = new PathDataBase();
            XmlFile = pathDataBase.GetApplicationConfFile();
            var selectNod = "/server/files";
            if (File.Exists(XmlFile))
            {
                TOOL_LIST = GetFromFileValue(selectNod, "TOOL_LIST");
                MACHINE_LIST = GetFromFileValue(selectNod, "MACHINE_LIST");
                APP_RET_LIST = GetFromFileValue(selectNod, "APP_RET_LIST");
                MFG_PROCESS_LIST = GetFromFileValue(selectNod, "MFG_PROCESS_LIST");
                TECHNOLOGY_LIST = GetFromFileValue(selectNod, "TECHNOLOGY_LIST");
                MEASURINGLAW_LIST = GetFromFileValue(selectNod, "MEASURINGLAW_LIST");
                AUXCOMMAND_LIST = GetFromFileValue(selectNod, "AUXCOMMAND_LIST");
                SUPPORT_SITE = GetFromFileValue(selectNod, "SUPPORT_SITE");
            }
        }
        private string GetFromFileValue(string selectNode, string findtext)
        {
            string value = "-";
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFile);
            XmlNodeList nodeList = doc.DocumentElement.SelectNodes(selectNode);
            foreach (XmlNode node in nodeList) { value = (node.SelectSingleNode(findtext).InnerText); }
            return $"{value}";
        }
    }
}
