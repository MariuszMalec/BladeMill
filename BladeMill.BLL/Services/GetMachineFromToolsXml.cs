using BladeMill.BLL.Models;
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Maszyna jako wzorzec fabryka
    /// </summary>
    public class GetMachineFromToolsXml : MachineSettings
    {
        public override Machine GetMachine(string file)
        {
            if (File.Exists(file) && ( file.Contains(".tools.xml") || file.Contains("current.xml") ))
            {
                return new Machine()
                {
                    Id = 1,
                    Created = DateTime.Now,
                    MachineName = GetMachineName(file),
                    MachineControl = GetControl(file),
                    MachineVericutTemplate = ""
                };
            }
            else
            {
                if (!File.Exists(file))
                    throw new Exception($"Warning, doesn't exist file! {file}");
                throw new Exception($"Warning, wrong input file! {file}");
            }
        }

        private string GetFromFileValue(string xmlFile, string findtext)
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
        private string GetMachineName(string file)
        {
            return GetFromFileValue(file, "MACHINE");
        }
        private string GetControl(string file)
        {
            return GetFromFileValue(file, "CONTROL");
        }
    }
}
