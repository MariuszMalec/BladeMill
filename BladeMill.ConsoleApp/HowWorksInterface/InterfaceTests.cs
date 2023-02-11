using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using System;

namespace BladeMill.ConsoleApp.HowWorksInterface
{
    public static class InterfaceTests
    {
        public static void Tests(PathDataBase paths)
        {
            var toolxmlService = new XMLToolService();
            var varpoolxmlService = new XMLVarpoolService();
            var inputxmlService = new XMLInputFileService();
            var xmlService = new XmlService(inputxmlService);//tu wstrzykujemy serwisy
            Console.WriteLine(xmlService.GetFromFileValue(paths.GetFileCurrentToolsXml(), ToolXmlEnum.PRGNUMBER.ToString()));
            Console.WriteLine(xmlService.GetFromFileValue(varpoolxmlService.GetCurrentVarpoolFile(), VarpoolEnum.BladeMaterial.ToString()));
            Console.WriteLine(xmlService.GetFromFileValue(@"C:/temp/inputdata.xml", InputXmlEnum.machine.ToString()));
        }
    }
}
