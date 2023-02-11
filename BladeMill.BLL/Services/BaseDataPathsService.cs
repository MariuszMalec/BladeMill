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
    public class BaseDataPathsService
    {
        private string _inputdata = @"C:\temp\basepaths.xml";
        private string _outputdata = @"C:\temp\basepaths_test.xml";
        private IEnumerable<BasePaths> _datas;        
        public BaseDataPathsService()
        {
            _datas = BaseDataPaths.Create();
        }
        public void SetData()
        {
            var xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");
            //create xml
            var serializer = new XmlSerializer(typeof(List<BasePaths>));
            using (var writer = File.CreateText(_outputdata))
            {
                serializer.Serialize(writer, new List<BasePaths>(_datas), xmlnsEmpty);
            }
            //usuniecie roota
            XDocument input = XDocument.Load(_outputdata);
            XElement firstChild = input.Root.Elements().First();
            firstChild.Save(_outputdata);
            Console.WriteLine($"Create file {_outputdata}");
        }
        public BasePaths GetData(string file)
        {
            var data = new BasePaths();
            if (File.Exists(file))
            {
                try
                {
                    using var reader = new StreamReader(file, true);
                    var fileNAME = reader.ReadToEnd();
                    reader.Close();
                    data = Deserialize<BasePaths>(fileNAME);
                    return data;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException());
                }
            }
            else
            {
                Console.WriteLine($"brak pliku {file}");
                throw new Exception($"brak pliku {file}");
            }
            return data;
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
    }
}
