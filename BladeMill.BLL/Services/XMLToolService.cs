using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace BladeMill.BLL.Services
{
    public class XMLToolService : IXmlService
    {
        public List<Tool> LoadToolsFromXml(string file)//xml
        {
            return new List<Tool>() { };
        }
        public string GetFromFileValue(string xmlFile, string findtext)
        {
            string navigator = "/TOOLLIST";
            string element = findtext;
            string value = string.Empty;
            try
            {
                if (File.Exists(xmlFile))
                {
                    //create list	
                    XmlDocument document = new XmlDocument();
                    document.Load(xmlFile);
                    XPathNavigator navigator2 = document.CreateNavigator();
                    XPathNodeIterator nodes2 = navigator2.Select(navigator);
                    //				
                    string line;
                    while (nodes2.MoveNext())
                    {
                        line = nodes2.Current.GetAttribute(element, "");
                        value = line;
                    }
                    return $"{value.Replace(" ", "")}";
                }
                return $"{value}";
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}
