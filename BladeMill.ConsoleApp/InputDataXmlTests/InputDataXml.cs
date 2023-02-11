using BladeMill.BLL.Services;
using System;
using System.Linq;

namespace BladeMill.ConsoleApp.InputDataXmlTests
{
    static class InputDataXml
    {
        public static void Tests()
        {
            var readxml = new InputDataXmlService();
            var data = readxml.GetDataFromInputDataXml();
            Console.WriteLine($"{data.catpart}");
            Console.WriteLine($"{data}");
            readxml.SetDataToInputDataXml();
            Console.WriteLine(readxml._datas.Select(d => d.TypeOfProcess).FirstOrDefault());
        }
    }
}
