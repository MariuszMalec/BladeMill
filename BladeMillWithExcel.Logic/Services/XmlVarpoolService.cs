using BladeMillWithExcel.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace BladeMillWithExcel.Logic.Services
{
    public class XmlVarpoolService
    {
        public Dictionary<string, string> GetPropertiesAsDictionaryFromVarpool(string varpoolFile)
        {
            var dict = new Dictionary<string, string>();
            var varpoolXml = new VarpoolXmlFile(varpoolFile);
            foreach (var prop in varpoolXml.GetType().GetProperties())
            {
                Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(varpoolXml, null));
                dict.Add(prop.Name, (string)prop.GetValue(varpoolXml, null));
            }
            return dict;
        }

        public string GetCurrentVarpoolFile()
        {
            //var order = new BMOrder(_pathData.GetFileCurrentToolsXml());
            //var currentVarpoolFile = Path.Combine(order.OrderNameDir, order.OrderName + "_varpool.xml");
            //if (File.Exists(currentVarpoolFile))
            //{
            //    return currentVarpoolFile;
            //}
            return string.Empty;
        }
    }
}
