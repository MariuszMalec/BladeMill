using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace BladeMill.BLL.Services
{
    public class XMLVarpoolService : IXmlService
    {
        private string _currentVarpoolFile;
        private PathDataBase _pathData = new PathDataBase();
        private AppXmlConfDirectories _appConf = new AppXmlConfDirectories();
        public XMLVarpoolService()
        {
            _currentVarpoolFile = GetCurrentVarpoolFile();
        }
        public string GetCurrentVarpoolFile()
        {
            var order = new BMOrder(_pathData.GetFileCurrentToolsXml());
            var currentVarpoolFile = Path.Combine(order.OrderNameDir, order.OrderName + "_varpool.xml");
            if (File.Exists(currentVarpoolFile))
            {
                return currentVarpoolFile;
            }
            return string.Empty;
        }
        public string GetVarpolFileAccXmlFile(string xmlFile)
        {
            var order = new BMOrder(xmlFile);
            var currentVarpoolFile = Path.Combine(order.OrderNameDir, order.OrderName + "_varpool.xml");
            if (File.Exists(currentVarpoolFile))
            {
                return currentVarpoolFile;
            }
            return string.Empty;
        }
        public string GetVarpolFileAccNcFile(string ncFile)
        {
            if (File.Exists(ncFile))
            {
                var ncdir = Path.GetDirectoryName(ncFile);
                var orderName = Path.GetFileNameWithoutExtension(ncFile);
                if (!ncFile.Contains("01."))
                {
                    Serilog.Log.Warning($"Wybralese bledny program NC,wybierz program glowny 01");
                    return string.Empty;
                }
                orderName = orderName.Remove(orderName.Length - 2);
                var varpoolFile = Path.Combine(_appConf.MFG_ORDER_DIR, orderName, orderName + "_varpool.xml");
                if (!File.Exists(varpoolFile))
                {
                    Serilog.Log.Warning($"Brak pliku varpool {varpoolFile}");
                    return string.Empty;
                }
                return varpoolFile;
            }
            Serilog.Log.Error($"Brak pliku nc {ncFile}");
            return string.Empty;
        }
        public string GetFromFileValue(string xmlFile, string findtext)
        {
            try
            {
                string Value = string.Empty;
                if (File.Exists(xmlFile))
                {
                    List<string> listvarpoolNames = new List<string>(new string[] { });
                    List<string> listvarpoolValues = new List<string>(new string[] { });
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlFile);
                    XPathNavigator navigator = doc.CreateNavigator();
                    XPathNodeIterator nodes = navigator.Select("/VarPool/Overview");
                    string Name = "";
                    XPathNodeIterator nodesName = navigator.Select("/VarPool/Var/Name");
                    foreach (XPathNavigator oCurrent in nodesName)
                    {
                        Name = oCurrent.InnerXml;//Name
                        listvarpoolNames.Add(Name);
                    }
                    XPathNodeIterator nodesValue = navigator.Select("/VarPool/Var/Value");
                    foreach (XPathNavigator oCurrent in nodesValue)
                    {
                        Value = oCurrent.InnerXml;//Name
                        listvarpoolValues.Add(Value);
                    }
                    Value = string.Empty;
                    int count = 0;
                    foreach (string element in listvarpoolNames)
                    {
                        //if(element.Contains(textfind))
                        if (element == findtext)
                        {
                            Value = listvarpoolValues[count].ToString().Replace(" ", "");
                        }
                        count++;
                    }
                    return $"{Value}";
                }
                return $"{Value}";
            }
            catch (Exception e)
            {
                throw new Exception("check function GetFromXmlFileValue", e);
            }
        }
        public VarpoolXmlFile GetAllDataFromVarpoolFile()
        {
            return new VarpoolXmlFile(_currentVarpoolFile);
        }
        public Dictionary<string, string> GetPropertiesAsDictionaryFromVarpool()
        {
            var dict = new Dictionary<string, string>();
            var varpoolXml = new VarpoolXmlFile(_currentVarpoolFile);
            foreach (var prop in varpoolXml.GetType().GetProperties())
            {
                Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(varpoolXml, null));
                dict.Add(prop.Name, (string)prop.GetValue(varpoolXml, null));
            }
            return dict;
        }
    }
}
