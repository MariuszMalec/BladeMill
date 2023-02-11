using BladeMill.BLL.SourceData;
using System.IO;
using System.Xml;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane z pliku konfiguracji BM Application.xml.conf
    /// </summary>
    public class AppXmlConfDirectories
    {
        public string XmlFile { get; set; }
        public string ENGINEERING_ORDER_DIR { get; set; } = "";
        public string MFG_ORDER_DIR { get; set; } = "";
        public string TEMP { get; set; } = "";
        public string INSTALL_DIR { get; set; } = "";
        public string NC_DIR { get; set; } = "";
        public string SCRIPTS_DIR { get; set; } = "";
        public string REPRESENTATION_DIR { get; set; } = "";

        private PathDataBase _pathDataBase = new PathDataBase();
        public AppXmlConfDirectories()
        {
            XmlFile = _pathDataBase.GetApplicationConfFile();
            var selectNod = "/server/directories";
            if (File.Exists(XmlFile))
            {
                GetDirectoryConfiguration(selectNod);
            }
        }

        private void GetDirectoryConfiguration(string selectNod)
        {
            ENGINEERING_ORDER_DIR = GetFromFileValue(selectNod, "ENGINEERING_ORDER_DIR");
            MFG_ORDER_DIR = GetFromFileValue(selectNod, "MFG_ORDER_DIR");
            TEMP = GetFromFileValue(selectNod, "TEMP");
            INSTALL_DIR = GetFromFileValue(selectNod, "INSTALL_DIR");
            NC_DIR = GetFromFileValue(selectNod, "NC_DIR");
            SCRIPTS_DIR = GetFromFileValue(selectNod, "SCRIPTS_DIR");
            REPRESENTATION_DIR = GetFromFileValue(selectNod, "REPRESENTATION_DIR");
        }

        private string GetFromFileValue(string selectNode, string findtext)//selectNode = "/server/directories"
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
