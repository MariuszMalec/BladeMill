using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using System;
using System.Linq;
using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Validators;
using BladeMill.ConsoleApp.CreateToolsExcel;
using BladeMillWithExcel.Logic.Enums;

namespace BladeMill.ConsoleApp
{
    public class Test
    {
        public static void Tests()
        {
            string dirFiles = @"C:\temp\basepaths_test.xml";
            string dir = @"C:\TempNC";
            string toolxmlfile = @"C:\TempNC\A999999.tools.xml";
            string inputxmlfile = @"C:\Temp\inputdata.xml";
            string varpoolxmlfile = @"C:\Clever\V300\BladeMill\data\RootMfgDir\A666666\A666666_varpool.xml";
            string spffile = @"C:\TempNC\1373235.SPF";
            //string mpffile = @"C:\TempNC\1373201.MPF";
            string mpffile = @"C:\TempNC\8999901.NC";
            string currenttoolxmlfile = @"C:\TempNC\current.xml";

            //TODO Przyklad extention methods
            string Sso = "222222222";
            Sso.ThrowIfNullOrEmpty();
            Sso.ThrowIfTooLong(9);

            //read technology excel
            var getValues = ReadExcelFile.GetAll(@"C:\Clever\V300\BladeMill\data\RootEngDir\301T002184 (IMR-007792)\301T002184P1002.xls");
            if (getValues != null)
                getValues.ForEach(value => Console.WriteLine($"{value.Id} {value.Name} {value.Value}"));
            //var getValue = ReadExcelFile.GetValue(TechnologyEnum.Ltol_HE, @"C:\Clever\V300\BladeMill\data\RootEngDir\301T002184 (IMR-007792)\301T002184P1002.xls", getValues);
            //Console.WriteLine($"{getValue.Id} {getValue.Name} {getValue.Value}");

            //read inputdataxml
            //IXmlService xmlService = new XMLInputFileService();
            //var xml = new XmlService(xmlService);
            //Console.WriteLine(xml.GetFromFileValue(inputxmlfile, InputXmlEnum.TypeOfProcessEnum.ToString() ));

            //IXmlService read
            //IXmlService xmlService = new XMLToolService();
            //IXmlService varpoolService = new XMLVarpoolService();
            //var xml = new XmlService(varpoolService);
            //Console.WriteLine(xml.GetFromFileValue(varpoolxmlfile, "BROH"));

            //read toolsxml
            //var readXml = new ToolsXml(toolxmlfile);
            //Console.WriteLine(readXml.BLADEORIENTATION);           

            //test xmla
            //var xmlData = BaseDataPaths.Create();
            //var readxml = new BaseDataPathsService();            
            //readxml.SetData();
            //var data = readxml.GetData(dirFiles);
            //Console.WriteLine($"{data.DirBladeMillScripts}");
            //Console.WriteLine($"{data}");

            //test fabryki
            //var shapeFactory = new MachineServiceFactory();
            //var ncFile = shapeFactory.CreateMachine(TypeOfFile.varpoolFile);
            //Console.WriteLine($"{ncFile.GetMachine(varpoolxmlfile).MachineName}");

            //var machineService = new MachineService();
            //Console.WriteLine($"{machineService.GetNcMachineFromXmlFile(@"C:\tempNC\13954.tools.xml")}");
            //Console.WriteLine($"{machineService.GetNcMachineFromNC(@"C:\tempNC\1395401.MPF")}");
            //machineService.GetListMachines().ForEach(s=>Console.WriteLine(s));

            //ProgramExeService startProgram = new ProgramExeService();
            //PathDataBase paths = new PathDataBase();
            //startProgram.StartProcess("OrderTransfer.exe", paths.GetDirTask());

            //var file = new FileService();
            //file.GetLinesFromFile(spffile).ForEach(l => Console.WriteLine($"{l.Line}"));
            //file.GetMainProgromFromDir(dir, "MPF").ForEach(f => Console.WriteLine($"{f.MainProgram}"));
            //file.GetSubprogramsListFromNc(mpffile).ForEach(s => Console.WriteLine($"{s}"));

            //var toolXml = new ToolXmlService();
            //IToolService iToolXml = toolXml;
            //iToolXml.LoadToolsFromFile(toolxmlfile).ForEach(t => Console.WriteLine($"{t.BatchFile} {t.ToolID}"));
            //Console.WriteLine("------------------------------------------------------");
            //var toolNc = new ToolNcService();
            //IToolService iToolNc = toolNc;
            //iToolNc.LoadToolsFromFile(mpffile).ForEach(t=>Console.WriteLine($"{t.BatchFile} {t.ToolID}"));           

            //toolxmlfile = @"C:\TempNC\A123123.xml";
            //spffile = @"C:\TempNC\A12312335.SPF";
            //mpffile = @"C:\TempNC\A12312301.MF";
            //var validateToolsXml = new ValidateToolsXmlFile();
            //IValidator iToolXml = validateToolsXml;
            //if (iToolXml.CheckFile(toolxmlfile).Item1 == true)
            //{
            //    Console.WriteLine($"{iToolXml.CheckFile(toolxmlfile).Item2}");
            //}
            //Console.WriteLine($"---------------------------------------");
            //var validateNC = new ValidateNcFile();
            //IValidator iNc = validateNC;
            //if (iNc.CheckFile(mpffile).Item1 == true)
            //{
            //    Console.WriteLine($"{iNc.CheckFile(mpffile).Item2}");
            //}
            //Console.WriteLine($"---------------------------------------");
        }
    }
}