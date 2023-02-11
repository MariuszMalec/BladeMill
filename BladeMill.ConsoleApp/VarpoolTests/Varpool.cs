using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.ConsoleApp.VarpoolTests
{
    public static class Varpool
    {
        public static void Tests()
        {
            var varpoolXmlService = new XMLVarpoolService();
            string varpoolxmlfile = varpoolXmlService.GetCurrentVarpoolFile();

            var varpoolXml = new VarpoolXmlFile(varpoolxmlfile);

            //get varpool according ncFile
            //var mainProgram = @"C:\tempNC\A44444401.MPF";
            //string varpoolxmlfileFromNc = varpoolXmlService.GetVarpolFileAccNcFile(mainProgram);

            //Console.WriteLine($"{varpoolXml.MfgSystem}");
            //Console.WriteLine($"{varpoolXml}");

            //var data = varpoolXmlService.GetAllDataFromVarpoolFile();
            //Console.WriteLine($"{data}");
            //Console.WriteLine("-------------------------------------------");
            var dict = varpoolXmlService.GetPropertiesAsDictionaryFromVarpool();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------");
            foreach (KeyValuePair<string, string> item in dict)
            {
                Console.WriteLine("Key: {0}, Value: {1}",
                item.Key, item.Value);
            }
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------");

            string matchKey = "MfgSystem";
            ViewMfgSystem(dict, matchKey);
            var mfgSystem = dict.FirstOrDefault(item => item.Key == matchKey).Value;
            Console.WriteLine(mfgSystem);
        }

        private static void ViewMfgSystem(Dictionary<string, string> dict, string matchKey)
        {
            var value = dict[matchKey];
            if (dict.TryGetValue(matchKey, out value))
            {
                Console.WriteLine(value);
            }
        }
    }
}
