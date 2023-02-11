using BladeMill.BLL.Interfaces;
using System;

namespace BladeMill.ConsoleApp.HowToUseInterface
{
    public class InterfaceNauka
    {
        private readonly IToolService _xmlService;
        private string _xmlFile;
        public InterfaceNauka(IToolService xmlService, string xmlFile)
        {
            _xmlService = xmlService;
            _xmlFile = xmlFile;
        }
        public void Tests()
        {

            _xmlService.LoadToolsFromFile(_xmlFile).ForEach(t => Console.WriteLine($"{t.BatchFile}"));

            //first interface
            //var service1 = new XMLInputFileService();//objekt rozny
            //var service2 = new XMLVarpoolService();
            //var service3 = new XMLToolService();
            //IXmlService iXmlService = service1; // service1 or service2 or service3
            //////ponizej dziala juz interface nie classa!!
            //Console.WriteLine($"{iXmlService.GetFromFileValue(inputxmlfile, "machine")}");
            //Console.WriteLine($"{iXmlService.GetFromFileValue(toolxmlfile, "MACHINE")}");
            //Console.WriteLine($"{iXmlService.GetFromFileValue(varpoolxmlfile, "MfgSystem")}");

            //var service4 = new ToolXmlService();//objekt rozny
            //var service5 = new ToolNcService();
            //IService iService = service4;
            //iService.LoadToolsFromFile(toolxmlfile).ForEach(t=>Console.WriteLine($"{t.BatchFile}"));
            //iService.LoadToolsFromFile(mpffile).ForEach(t => Console.WriteLine($"{t.BatchFile}"));
        }
    }
}
