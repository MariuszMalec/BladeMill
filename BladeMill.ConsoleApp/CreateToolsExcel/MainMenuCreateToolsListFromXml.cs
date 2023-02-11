using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using BladeMill.BLL.Validators;
using BladeMillWithExcel.Logic.Services;
using System;
using System.IO;
using System.Linq;

namespace BladeMill.ConsoleApp.CreateToolsExcel
{
    class MainMenuCreateToolsListFromXml
    {
        private static string[] mainMenuItem = { };
        public static void ShowMainMenu()
        {

            var files = new FileService();
            var list = files.GetListSelectedFiles(@"C:\tempNC", "tools.xml");

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{list[i].BatchFile}");
                mainMenuItem = mainMenuItem.Append($"{list[i].BatchFile}").ToArray();
            }
            mainMenuItem = mainMenuItem.Append("Exit").ToArray();

            short currentItem = 0;
            do
            {
                ConsoleKeyInfo keyPressed;
                do
                {
                    Console.Clear();
                    Console.WriteLine("========================================================");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("||    Welcome Tool List Generator                     ||");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("========================================================");
                    for (int i = 0; i < mainMenuItem.Length; i++)
                    {
                        if (currentItem == i)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(">>");
                            Console.WriteLine(mainMenuItem[i] + "<<");
                        }
                        else
                        {
                            Console.WriteLine(mainMenuItem[i]);
                        }
                        Console.ResetColor();
                    }
                    Console.WriteLine("-----------------------------------------------");
                    Console.Write("Select your choice with the arrow keys and click (ENTER) key");
                    keyPressed = Console.ReadKey(true);
                    Console.Clear();
                    if (keyPressed.Key.ToString() == "DownArrow")
                    {
                        currentItem++;
                        if (currentItem > mainMenuItem.Length - 1) currentItem = 0;
                    }
                    else if (keyPressed.Key.ToString() == "UpArrow")
                    {
                        currentItem--;
                        if (currentItem < 0) currentItem = Convert.ToInt16(mainMenuItem.Length - 1);
                    }
                } while (keyPressed.KeyChar != 13);//if press enter selected menu
                //Selected mainmenu from loop
                if (mainMenuItem[currentItem].Contains(mainMenuItem[currentItem]))
                {
                    Console.WriteLine($"{mainMenuItem[currentItem]} ...");
                    if (mainMenuItem[currentItem] == "Exit")
                    {
                        Environment.Exit(0);
                        return;
                    }
                    CreateToolList(currentItem);
                    Console.WriteLine($"Press any key to continue");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Will be soon!");
                    Console.ReadKey();
                }
            }
            while (false);
        }

        private static void CreateToolList(short currentItem)
        {
            Console.WriteLine($"-------------------------------------------------------------------------------------");
            var excelService = new ExcelService();
            var getProcess = new GetProcessForConsole();
            getProcess.KillProcess("EXCEL");
            var pathService = new PathDataBase();
            var excelTemplate = pathService.GetFileExcelTemplate();
            var validateExcel = new ValidateTemplateExcelFile();
            if (validateExcel.CheckFile(excelTemplate).Item1 == true)
            {
                Console.WriteLine($"{validateExcel.CheckFile(excelTemplate).Item2}");
            }
            var dirNew = Path.GetDirectoryName(mainMenuItem[currentItem]);
            var order = Path.GetFileNameWithoutExtension(mainMenuItem[currentItem]).Replace(".tools", "");
            var newExcelFile = Path.Combine(dirNew, order + ".xlsm");
            excelService.DeleteExcelFile(newExcelFile);
            excelService.CopyExcelTemplate(excelTemplate, newExcelFile);

            var xmlService = new BladeMillWithExcel.Logic.Services.ToolXmlService();

            var tools = xmlService.LoadToolsFromFile(mainMenuItem[currentItem]);

            var varpoolService = new XMLVarpoolService();
            var varpoolFile = varpoolService.GetVarpolFileAccXmlFile(mainMenuItem[currentItem]);

            excelService.startExcell(newExcelFile, tools, mainMenuItem[currentItem], varpoolFile);
            excelService.KillSoftware("EXCEL", true);
            Console.WriteLine($"-------------------------------------------------------------------------------------");
        }
    }
}
