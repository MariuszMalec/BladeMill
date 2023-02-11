using BladeMillWithExcel.Logic.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;

namespace BladeMillWithExcel.Logic.Services
{
    public class ToolXmlService : IToolService
    {
        private int _count = 0;
        public List<Tool> LoadToolsFromFile(string toolsXmlFile)//xml
        {
            if (File.Exists(toolsXmlFile) && toolsXmlFile.Contains(".xml"))
            {
                int nmbFile = 0;
                List<Tool> tools = new List<Tool>();
                XmlDocument document = new XmlDocument();
                document.Load(toolsXmlFile);
                XmlNodeList aNodes0 = document.GetElementsByTagName("PROG");
                foreach (XmlNode aNode in aNodes0)
                {
                    XmlAttribute idAttributefilename = aNode.Attributes["FILENAME"];
                    XmlAttribute idAttributedescript = aNode.Attributes["DESCRIPT"];
                    XmlNodeList toolitems = aNode.ChildNodes;
                    if (!idAttributefilename.Value.ToString().Contains("75.") && !idAttributefilename.Value.ToString().Contains("85."))//ignoruje poczatkowe i koncowe
                    {
                        string fixsubprogram;
                        if (idAttributefilename.Value.ToString().Contains("DUMMY"))
                        {
                            fixsubprogram = idAttributefilename.Value.ToString().Replace("DUMMY_", "");
                        }
                        else
                        {
                            fixsubprogram = idAttributefilename.Value.ToString().Replace(".SPF", "");
                        }
                        for (int i = 0; i < toolitems.Count; i++)
                        {
                            XmlElement toolitem = (XmlElement)toolitems[i];
                            string currentOffset = "";//default
                            XmlNodeList offsets = toolitem.ChildNodes;
                            for (int j = 0; j < offsets.Count; j++)
                            {
                                XmlElement offset = (XmlElement)offsets[j];
                                if (offset.Attributes["VALUE"].Value != null)
                                {
                                    if (offset.Name == "OFFSET")
                                    {
                                        currentOffset += offset.Attributes["VALUE"].Value + ",";
                                    }
                                }
                            }
                            string newofsetString = "";
                            var currentOffsetToList = currentOffset.Split(',');
                            currentOffsetToList.Select(o => o.ToString()).ToList()
                                .Distinct().ToList()
                                .ForEach(o => newofsetString += "," + o);
                            currentOffset = newofsetString;
                            //usun order z nazwy programu
                            var order = Path.GetFileNameWithoutExtension(toolsXmlFile).Replace(".tools", "");
                            string toolProgram = fixsubprogram.Replace(order, "");
                            string toolDesc = aNode.Attributes["DESCRIPT"].Value.ToString();
                            string toolId = toolitem.Attributes["TOOLNR"].Value.ToString();
                            string toolIdPreload = "-";
                            string toolSet = toolitem.Attributes["TOOLSET"].Value.ToString();
                            string toolLen = toolitem.Attributes["TOOLLEN"].Value.ToString();
                            string toolDiam = toolitem.Attributes["TOOLDIAM"].Value.ToString();
                            string toolCrn = toolitem.Attributes["TOOLCRN"].Value.ToString();
                            string toolSpindle = toolitem.Attributes["S"].Value.ToString();
                            string toolFeedrate = toolitem.Attributes["F"].Value.ToString().Split('.')[0];
                            string toolMaxmilltime = "-";
                            string machine = GetFromFileValue(toolsXmlFile, "MACHINE");
                            try
                            {
                                if (toolitem.Attributes["MAXMILLTIME"] != null)
                                {
                                    toolMaxmilltime = toolitem.Attributes["MAXMILLTIME"].Value.ToString();
                                }
                            }
                            catch { toolMaxmilltime = "-"; }
                            nmbFile++;
                            tools.Add(new Tool(nmbFile,
                                toolProgram,
                                toolDesc,
                                toolSet,
                                toolId,
                                toolIdPreload,
                                toolLen,
                                toolDiam,
                                toolCrn,
                                toolSpindle,
                                toolFeedrate,
                                toolMaxmilltime,
                                currentOffset,
                                machine,
                                false));
                        }
                    }
                }
                //take last calculated subprogram
                var newtools = tools
                    .GroupBy(t => t.BatchFile).Select(t => t.LastOrDefault())
                    .ToList();
                //fix Id
                _count = 0;
                newtools.ForEach(t => t.Id = ++_count);
                return newtools;
            }
            return new List<Tool>() { };
        }

        public bool CheckToolsXml(string xmlfile)
        {
            bool result = true;
            if (File.Exists(xmlfile))
            {
                if (GetFromFileValue(xmlfile, "MACHINE") == "-")
                {
                    return false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        public string GetFromFileValue(string xmlFile, string findtext)
        {
            try
            {
                string navigator = "/TOOLLIST";
                string element = findtext;
                string value = "-";
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
                throw new Exception("check function GetFromFileValue", e);
            }
        }
        public string GetMachineFromFile(string xmlFile)
        {
            try
            {
                string navigator = "/TOOLLIST";
                string element = "MACHINE";
                string value = "-";
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
                throw new Exception("check function GetMachineFromFile", e);
            }
        }
    }
}
