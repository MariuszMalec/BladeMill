using BladeMill.BLL.SourceData;
using System.IO;
using System.Xml;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane z pliku konfiguracji BM Application.xml.conf
    /// </summary>
    public class AppXmlConfCom
    {
        public string XmlFile { get; set; }
        public string? APP_SERVER_IP { get; }
        public string APP_SERVER_PORT { get; set; }
        public string CAD_PLUGIN_IP { get; set; }
        public string CAD_PLUGIN_PORT { get; set; }
        public string LOG_ACCURACY { get; set; }
        public string BACKUP_ORDER { get; set; }
        public string DATAUNIT { get; set; }

        public AppXmlConfCom(string appXmlConfFile)
        {
            PathDataBase pathDataBase = new PathDataBase();
            XmlFile = pathDataBase.GetApplicationConfFile();
            var selectNod = "/server/com";
            if (File.Exists(XmlFile))
            {
                APP_SERVER_IP = GetFromFileValue(selectNod, "APP_SERVER_IP");
                APP_SERVER_PORT = GetFromFileValue(selectNod, "APP_SERVER_PORT");
                CAD_PLUGIN_IP = GetFromFileValue(selectNod, "CAD_PLUGIN_IP");
                CAD_PLUGIN_PORT = GetFromFileValue(selectNod, "CAD_PLUGIN_PORT");
                LOG_ACCURACY = GetFromFileValue(selectNod, "LOG_ACCURACY");
                BACKUP_ORDER = GetFromFileValue(selectNod, "LOG_ACCURACY");
                DATAUNIT = GetFromFileValue(selectNod, "LOG_ACCURACY");
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
