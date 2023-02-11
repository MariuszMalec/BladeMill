using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using BladeMill.ConsoleApp.AppConfiguraton;
using BladeMill.ConsoleApp.BladeMillChecker;
using BladeMill.ConsoleApp.BMDXmlTests;
using BladeMill.ConsoleApp.ConvertMainNc;
using BladeMill.ConsoleApp.CreateToolsExcel;
using BladeMill.ConsoleApp.FindErrorsInNc;
using BladeMill.ConsoleApp.GitTests;
using BladeMill.ConsoleApp.HelpProgramms;
using BladeMill.ConsoleApp.HowToReadConfFromJson;
using BladeMill.ConsoleApp.HowToUseInterface;
using BladeMill.ConsoleApp.HowWorksInterface;
using BladeMill.ConsoleApp.InputDataXmlTests;
using BladeMill.ConsoleApp.ToolNcTests;
using BladeMill.ConsoleApp.ToolsXmlTests;
using BladeMill.ConsoleApp.TransferOrder;
using BladeMill.ConsoleApp.VarpoolTests;
using BladeMill.ConsoleApp.VcProjectCreation;
using BladeMillWithExcel.Logic.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Serilog;
using SQLitePCL;
using System;
using System.IO;

namespace BladeMill.ConsoleApp
{
    public class AppStarting
    {
        private readonly ILogger _log;
        private IConfiguration _config;

        public AppStarting(ILogger log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public void Run(string[] args)
        {
            //bez argumentow z appsetings.json testy
            if (args.Length == 0)
            {
                //---------------------------------
                //wczytanie sciezek
                //---------------------------------
                var paths = new PathDataBase();

                //---------------------------------
                //wczytanie bmd xml
                //---------------------------------
                //BmdXmlTests.Tests(@"C:\Clever\V300\BladeMill\data\RootEngDir\301T002184 (IMR-007792)\301T002184P1002_-_BMD.xml");

                //---------------------------------
                //czytanie jsona
                //---------------------------------
                //TestsJson();

                //---------------------------------
                //czytanie uzytkownikow z serwisu                
                //---------------------------------
                //TestsUsers();

                //---------------------------------
                //przerabianie programu
                //---------------------------------
                //ConvertMainProgram.Tests(_log, @"C:\tempNC\C00091801.MPF", "123456", MachineEnum.HSTM300HD);

                //---------------------------------
                //order transfer
                //---------------------------------
                //OrderTransfer.Make(_log);

                //---------------------------------
                //CreateVcproject
                //---------------------------------
                //CreateVcproject.CreateFromNc(@"C:\tempNC\B12345601.MPF", _log);

                //---------------------------------
                //CreateVcproject
                //---------------------------------
                //CreateVcproject.CreateFromXml(@"C:\tempNC\current.xml", _log);

                //---------------------------------
                //ToolXmlFile
                //---------------------------------
                //ToolXmlFile.Tests();               

                //---------------------------------
                //Varpool
                //---------------------------------
                //Varpool.Tests();

                //---------------------------------
                //AppConfFile
                //---------------------------------
                //AppConfFile.Tests();

                //---------------------------------
                //PathsData
                //---------------------------------
                //PathsData.Tests(_log);

                //---------------------------------
                //Git
                //---------------------------------
                //Git.Tests();

                //---------------------------------
                //InputDataXml
                //---------------------------------
                //InputDataXml.Tests();

                //---------------------------------
                //CreateToolListFromToolsXml and NC
                //---------------------------------
                //CreateToolListFromToolsXml.Tests(@"C:/tempnc/current.xml", _log);
                //CreateToolListFromNC.Tests(@"C:/tempnc/A99999901.MPF", _log);

                //---------------------------------
                //CheckBladeMill
                //---------------------------------
                //CheckBladeMill.Test();

                //---------------------------------
                //CheckAllNcCodes
                //---------------------------------
                //CheckAllNcCodes.Test(_log);

                //---------------------------------
                // Wczytaj narzedzia
                //---------------------------------
                //ToolXmlFile.ShowToolsFromToolsXml(@"C:\tempNC\A012997.tools.xml", _log);
                //ToolNc.ShowToolsFromNCCode(@"C:\tempNC\A99999901.MPF", _log);

                //ShowFilesInDir();
                //GetListOfEnumExe();
                //CreateNewInputXmlFile();
                //LoadNewInputXmlFile(@"C:\Temp\inputdata_test.xml");//wersja nowa

                //-----------------------------------
                // przyklad Action, Func, Predicate
                //-----------------------------------
                //ProgramExeService startProgram = new ProgramExeService();
                //LambdaTests(startProgram);        

                //-----------------------------------
                //  programy pomocniczeXMLInputFileService
                //-----------------------------------
                //CheckAllProgamExe.Test(_log);

                //-----------------------------------
                //  Testy z interfejsami
                //-----------------------------------
                //InterfaceTests.Tests(paths);

                //-----------------------------------
                // Special tests
                //-----------------------------------
                //Test.Tests();

                //-----------------------------------
                // Menu
                //-----------------------------------
                //MainMenuCreateToolsFromNC.ShowMainMenu();
                //MainMenuCreateToolsListFromXml.ShowMainMenu();                
                //MainMenuShowToolsFromXml.ShowMainMenu();
                //MainMenuShowHelpPrograms.ShowMainMenu();
                //MainMenuFindErrors.ShowMainMenu();                

                //Nowa wersja menu plynniejsze menu Users
                //Menu menu = new Menu();
                //menu.StartProgram();

                return;
            }
        }

        private void LambdaTests(ProgramExeService startProgram)
        {
            var dir = new PathDataBase();
            //Lambda
            Action<string, string> ActionVoid = (x, y) => Path.Combine(x, y);//void ??
            Func<string, string, string> funcMethodCombine = (x, y) => Path.Combine(x, y);//method
            Predicate<string> predicateExistFile = x => File.Exists(x);//return true/false
            Console.WriteLine($"------------------------------------------------------------------");
            Console.WriteLine($"------------------------------------------------------------------");
            Console.WriteLine($"{funcMethodCombine(dir.GetDirTask(), ExeEnum.ConvertXMLtoTXTfile + ".exe")}");
            Console.WriteLine($"------------------------------------------------------------------");
            Console.WriteLine($"{predicateExistFile(funcMethodCombine(dir.GetDirTask(), ExeEnum.ConvertXMLtoTXTfile + ".exe"))}");
            Console.WriteLine($"------------------------------------------------------------------");
        }

        private void LoadNewInputXmlFile(string file)
        {
            var loadXML = new InputDataXmlService();
            Console.WriteLine($"Load plik : {file}");
            Console.WriteLine($"------------------------------------------------------------------");
            Console.WriteLine(loadXML.GetDataFromInputDataXml().catpart);
            loadXML.GetAllDataFromInputDataXml().ForEach(x => Console.WriteLine(x));
        }

        private void CreateNewInputXmlFile()
        {
            var xmlData = XMLDataBase.CreateInputDataXml();
            //var loadXML = new InputDataXmlService(xmlData);
            Console.WriteLine($"Zapis do pliku: {XMLDataBase.GetXmlFile()}");
            //loadXML.ExportDataToXml(XMLDataBase.GetXmlFile());
            Console.WriteLine($"------------------------------------------------------------------");
            //Console.WriteLine($"Load plik : {XMLDataBase.GetXmlFile()}");
            //loadXML.ImportDataFromXml(XMLDataBase.GetXmlFile());
            //Console.WriteLine($"------------------------------------------------------------------");
            //loadXML._datas.ToList().ForEach(d => Console.WriteLine($"{d.ToString()}"));
        }

        private void GetListOfEnumExe()
        {
            var programExeService = new ProgramExeService();
            programExeService.GetListEnemExe().ForEach(a => _log.Information($"{a}"));
        }

        private void ShowFilesInDir()
        {
            var files = new NcMainProgramService();
            var list = files.GetListMainPrograms(@"C:\tempNC", "01.MPF");
            list.ForEach(f => _log.Information($"{f.Id} {f.MainProgram} {f.Machine} {f.Clamping}"));
        }

        private void TestsUsers()
        {
            UserServiceWithoutDatabase userService = new UserServiceWithoutDatabase();
            var users = userService.GetAll();
            users.ForEach(u => _log.Information($"{u.ToString()}"));
        }

        private void TestsJson()
        {
            ReadJsonFileAsConfig.ReaderJson(_log);
            var users = ReadJson.ReadUserFromJsonFile();
            users.ForEach(u => _log.Information($"{u.LastName}"));
        }
    }
}

