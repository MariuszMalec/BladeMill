using BladeMill.BLL.Interfaces;
using System;
using System.Xml;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Serwis czytania wartosci z pliku xml
    /// </summary>
    public class XMLInputFileService : IXmlService
    {
        public string GetFromFileValue(string xmlFile, string findtext)
        {
            try
            {
                string value = "-";
                if (System.IO.File.Exists(xmlFile))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlFile);
                    XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/DANE");
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.SelectSingleNode(findtext) == null)
                        {
                            return "-";
                        }
                        value = node.SelectSingleNode(findtext).InnerText;
                    }
                    return $"{value}";
                }
                return $"{value}";
            }
            catch (Exception e)
            {
                throw new Exception("check function GetFromFileValue", e);
            }
        }
    }
}
