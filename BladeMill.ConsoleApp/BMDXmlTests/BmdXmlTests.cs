using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System;

namespace BladeMill.ConsoleApp.BMDXmlTests
{
    public static class BmdXmlTests
    {
        public static void Tests(string bmdFile)
        {
            var bmdService = new XMLBmdService();
            var bmdList = bmdService.GetAll(bmdFile);
            foreach (var bmd in bmdList)
            {
                Console.WriteLine($"{bmd.Name} = {bmd.Value}");
            };
            Console.WriteLine("-------------------------------------------------------------------------------");
            string search = EnumBmdFile.BD.ToString();
            var parameter = bmdService.GetByName(search);
            Console.WriteLine($"{parameter.Name} = {parameter.Value} => {parameter.Flag}");
        }
    }
}
