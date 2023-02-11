using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using BladeMillWithExcel.Logic.Models;
using BladeMillWithExcel.Logic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMill.ConsoleApp.CreateToolsExcel
{
    public class MainMenuCreateToolsFromNC
    {
        private static string[] mainMenuItem = { };
        public static void ShowMainMenu()
        {

            var files = new FileService();
            var list = files.GetListSelectedFiles(@"C:\tempNC", "01.MPF");
            list.AddRange(files.GetListSelectedFiles(@"C:\tempNC", "01.NC"));

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
                    ViewToolsFromNC(currentItem);
                    Console.WriteLine($"Press any key to continue");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Will be soon!");
                    Console.ReadKey();
                }
            }
            while (true);
        }
        private static void ViewToolsFromNC(short currentItem)
        {
            var ncService = new BladeMillWithExcel.Logic.Services.ToolNcService();
            var incService = new BladeMillWithExcel.Logic.Services.ToolService(ncService);
            var toolsFromNc = incService.LoadToolsFromFile(mainMenuItem[currentItem]);

            var machineServiceFactory = new MachineServiceFactory();
            var machine = machineServiceFactory.CreateMachine(TypeOfFile.ncFile).GetMachine(mainMenuItem[currentItem]).MachineName;

            if (machine.Contains("HURON"))
            {
                toolsFromNc = ncService.LoadNCMultiple(mainMenuItem[currentItem]);
            }

            Console.WriteLine($"--------------------------------------------------------------------------------------");
            toolsFromNc.Where(t => t.BatchFile != null)
                .ToList()
                .ForEach(t => Console.WriteLine($"{t.BatchFile.ToString().PadRight(25, ' ')}" +
                $" | {t.Description.ToString().PadRight(30, ' ')} " +
                $" | {t.ToolSet.ToString().PadRight(6, ' ')} " +
                $" | {t.ToolID.ToString().PadRight(6, ' ')} " +
                $" | {t.ToolIDPreLoad.ToString().PadRight(6, ' ')} " +
                $" | {t.ToolDiam.ToString().PadRight(8, ' ')} " +
                $" | {t.ToolCrn.ToString().PadRight(6, ' ')} " +
                $" | {t.Toollen.ToString().PadRight(6, ' ')} " +
                $" | {t.Spindle.ToString().PadRight(6, ' ')} " +
                $" | {t.Machine}"));
            Console.WriteLine($"-------------------------------------------------------------------------------------");

            CreateToolsXls(currentItem, toolsFromNc);

        }

        private static void CreateToolsXls(short currentItem, List<Tool> toolsFromNc)
        {
            var machineService = new MachineService();
            var machine = machineService.GetNcMachineFromNC(mainMenuItem[currentItem]);
            var excelService = new ExcelService();
            excelService.KillSoftware("Excel", true);
            var pathService = new PathDataBase();
            var excelTemplate = pathService.GetFileExcelTemplate();

            var toolxmlfile = string.Empty;
            if (machine.Contains("HURON"))
            {
                toolxmlfile = mainMenuItem[currentItem].Replace(".NC", "tools.xml").Replace("01", ".");
            }
            else
            {
                toolxmlfile = mainMenuItem[currentItem].Replace("MPF", "tools.xml").Replace("01.", ".");
            }
            var dirNew = Path.GetDirectoryName(toolxmlfile);
            var order = Path.GetFileNameWithoutExtension(toolxmlfile);

            var newExcelFile = string.Empty;
            if (machine.Contains("HURON"))
            {
                newExcelFile = Path.Combine(dirNew, order.Replace("tools", "") + "xlsm");
            }
            else
            {
                newExcelFile = Path.Combine(dirNew, order.Replace("tools", "") + "xlsm");
            }

            excelService.DeleteExcelFile(newExcelFile);
            excelService.CopyExcelTemplate(excelTemplate, newExcelFile);
            var varpoolService = new XMLVarpoolService();
            var varpoolxmlfile = varpoolService.GetVarpolFileAccNcFile(mainMenuItem[currentItem]);
            excelService.startExcell(newExcelFile, toolsFromNc, toolxmlfile, varpoolxmlfile);
        }
    }
}
