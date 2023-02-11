using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BladeMill.BLL.Services
{
    public class InputDataXmlService
    {
        private string _inputdata = @"C:\temp\inputdata.xml";
        private string _outputdata = @"C:\temp\inputdata_test.xml";
        public IEnumerable<InputDataXml> _datas;
        public InputDataXmlService()
        {
            _datas = XMLDataBase.CreateInputDataXml();
        }
        public void SetDataToInputDataXml()
        {
            CheckDirectory(_outputdata);
            //usuniecie xml declarations from root
            var xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");
            //create xml
            var serializer = new XmlSerializer(typeof(List<InputDataXml>));
            using (var writer = File.CreateText(_outputdata))
            {
                serializer.Serialize(writer, new List<InputDataXml>(_datas), xmlnsEmpty);
            }
            //usuniecie roota
            XDocument input = XDocument.Load(_outputdata);
            XElement firstChild = input.Root.Elements().First();
            firstChild.Save(_outputdata);
            Console.WriteLine($"Create file {_outputdata}");
        }
        public static T Deserialize<T>(string data) where T : class
        {
            if (data == null)
            {
                return null;
            }

            if (data.Trim().Length == 0)
            {
                return null;
            }

            var ser = new XmlSerializer(typeof(T));

            using (var sr = new StringReader(data))
            {
                return (T)ser.Deserialize(sr);
            }
        }
        public List<InputDataXml> GetAllDataFromInputDataXml()
        {
            var data = new InputDataXml();
            if (File.Exists(_inputdata))
            {
                try
                {
                    using var reader = new StreamReader(_inputdata, true);
                    var fileNAME = reader.ReadToEnd();
                    reader.Close();
                    data = Deserialize<InputDataXml>(fileNAME);
                    List<InputDataXml> allData = new List<InputDataXml>();
                    allData.Add(data);
                    return allData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException());
                }
            }
            else
            {
                Console.WriteLine($"brak pliku {_inputdata}");
            }
            return new List<InputDataXml>();
        }

        public InputDataXml GetDataFromInputDataXml()
        {
            var data = new InputDataXml();
            if (File.Exists(_inputdata))
            {
                try
                {
                    using var reader = new StreamReader(_inputdata, true);
                    var fileNAME = reader.ReadToEnd();
                    reader.Close();
                    data = Deserialize<InputDataXml>(fileNAME);
                    return data;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException());
                }
            }
            else
            {
                Console.WriteLine($"brak pliku {_inputdata}");
            }
            return data;
        }
        private void CheckDirectory(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }
        }
        public void ExportDataToXml()
        {
            CheckDirectory(_outputdata);
            var serializer = new XmlSerializer(typeof(List<InputDataXml>), new XmlRootAttribute("DANE"));
            using (var writer = File.CreateText(_outputdata))
            {
                serializer.Serialize(writer, new List<InputDataXml>(_datas));
            }
            Console.WriteLine($"Create file {_outputdata}");
        }
        public IEnumerable<InputDataXml> ImportDataFromXml()
        {
            CheckDirectory(_outputdata);
            var serializer = new XmlSerializer(typeof(List<InputDataXml>), new XmlRootAttribute("DANE"));
            List<InputDataXml> _datas;
            using (var reader = File.OpenText(_outputdata))
            {
                _datas = (List<InputDataXml>)serializer.Deserialize(reader);
            }
            return _datas;
        }

        //public InputDataXml LoadInputXML(string file)
        //{
        //    InputDataXml dane = new InputDataXml();

        //    if (System.IO.File.Exists(file))
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(file);
        //        XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/DANE");
        //        foreach (XmlNode node in nodeList)
        //        {
        //            dane.catpart = node.SelectSingleNode("catpart").InnerText;
        //            dane.xmlpart = node.SelectSingleNode("xmlpart").InnerText;
        //            dane.xlspart = node.SelectSingleNode("xlspart").InnerText;
        //            dane.machine = node.SelectSingleNode("machine").InnerText;
        //            dane.Clampingmethod = node.SelectSingleNode("Clampingmethod").InnerText;
        //            dane.infile = node.SelectSingleNode("infile").InnerText;
        //            dane.BMTemplateFile = node.SelectSingleNode("BMTemplateFile").InnerText;
        //            dane.catpartfirst = node.SelectSingleNode("catpartfirst").InnerText;
        //            dane.xmlpartfirst = node.SelectSingleNode("xmlpartfirst").InnerText;
        //            dane.catpartend = node.SelectSingleNode("catpartend").InnerText;
        //            dane.xmlpartend = node.SelectSingleNode("xmlpartend").InnerText;
        //            dane.outfile = node.SelectSingleNode("outfile").InnerText;
        //            dane.selectlanguage = node.SelectSingleNode("selectlanguage").InnerText;
        //            dane.RootMfgDir = node.SelectSingleNode("RootMfgDir").InnerText;

        //            if (node.SelectSingleNode("ClampFromTemplate") != null)
        //            {
        //                dane.ClampFromTemplate = node.SelectSingleNode("ClampFromTemplate").InnerText.Replace("&", "");
        //            }

        //            if (node.SelectSingleNode("FIG_N") != null)
        //            {
        //                dane.FIG_N = node.SelectSingleNode("FIG_N").InnerText;
        //            }

        //            if (node.SelectSingleNode("runconfiguration").InnerText == "True")
        //            {
        //                dane.runconfiguration = "true";
        //            }
        //            if (node.SelectSingleNode("runbm").InnerText == "True")
        //            {
        //                dane.runbm = "true";
        //            }
        //            if (node.SelectSingleNode("runcmm").InnerText == "True")
        //            {
        //                dane.runcmm = "true";
        //            }
        //            if (node.SelectSingleNode("createvcproject").InnerText == "True")
        //            {
        //                dane.createvcproject = "true";
        //            }

        //            if (node.SelectSingleNode("Prerawbox").InnerText == "True")
        //            {
        //                dane.Prerawbox = "true";
        //            }
        //            if (node.SelectSingleNode("createraport").InnerText == "True")
        //            {
        //                dane.createraport = "true";
        //            }
        //            if (node.SelectSingleNode("BMTemplate").InnerText == "True")
        //            {
        //                dane.BMTemplate = "true";
        //            }
        //            if (node.SelectSingleNode("readxls").InnerText == "True")
        //            {
        //                dane.readxls = "true";
        //            }
        //            if (node.SelectSingleNode("pinwelding").InnerText == "True")
        //            {
        //                dane.pinwelding = "true";
        //            }
        //            if (node.SelectSingleNode("millshroud").InnerText == "True")
        //            {
        //                dane.millshroud = "true";
        //            }
        //            if (node.SelectSingleNode("middleTol").InnerText == "True")
        //            {
        //                dane.middleTol = "true";
        //            }
        //        }
        //    }
        //    return dane;
        //}

    }
}
