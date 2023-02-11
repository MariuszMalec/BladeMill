using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using BladeMillWithExcel.Logic.Services;
using System;
using System.IO;
using System.Linq;

namespace BladeMill.ConsoleApp.CreateToolsExcel
{
    public class MainMenuShowToolsFromXml
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
                    ViewToolsFromXml_interface(currentItem);
                    //CreateToolsFromXml(currentItem);
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

        private static void ViewToolsFromXml_interface(short currentItem)
        {
            var xmlService = new BladeMillWithExcel.Logic.Services.ToolXmlService();
            
            var toolsFromXml = xmlService.LoadToolsFromFile(mainMenuItem[currentItem]);
            Console.WriteLine($"--------------------------------------------------------------------------------------");
            toolsFromXml.Where(t => t.BatchFile != null)
                .ToList()
                .ForEach(t => Console.WriteLine($"{t.BatchFile.ToString().PadRight(25, ' ')}" +
                $" | {t.Id.ToString().PadRight(5, ' ')} " +
                $" | {t.Description.ToString().PadRight(30, ' ')} " +
                $" | {t.ToolID.ToString().PadRight(6, ' ')} " +
                $" | {t.ToolIDPreLoad.PadRight(6, ' ')} " +
                $" | {t.ToolDiam.ToString().PadRight(8, ' ')} " +
                $" | {t.ToolCrn.ToString().PadRight(6, ' ')} " +
                $" | {t.Toollen.ToString().PadRight(6, ' ')} " +
                $" | {t.Spindle.ToString().PadRight(6, ' ')} " +
                $" | {t.Feedrate.ToString().PadRight(6, ' ')} " +
                $" | {t.Machine}"));
            Console.WriteLine($"-------------------------------------------------------------------------------------");
        }

        private static void CreateToolsFromXml(short currentItem)
        {
            var excelService = new ExcelService();
            excelService.KillSoftware("Excel", true);

            //get excel template
            var pathService = new PathDataBase();
            var excelTemplate = pathService.GetFileExcelTemplate();
            //Console.WriteLine(excelTemplate);

            var toolxmlfile = mainMenuItem[currentItem];
            var dirNew = Path.GetDirectoryName(toolxmlfile);
            //Console.WriteLine(dirNew);
            var order = Path.GetFileNameWithoutExtension(toolxmlfile).Replace(".tools", "");
            var newExcelFile = Path.Combine(dirNew, order + ".xlsm");
            excelService.DeleteExcelFile(newExcelFile);
            excelService.CopyExcelTemplate(excelTemplate, newExcelFile);
            var toolService = new BladeMillWithExcel.Logic.Services.ToolXmlService();
            var tools = toolService.LoadToolsFromFile(toolxmlfile);
            var varpoolService = new XMLVarpoolService();
            var varpoolxmlfile = varpoolService.GetCurrentVarpoolFile();
            excelService.startExcell(newExcelFile, tools, toolxmlfile, varpoolxmlfile);
        }
    }
}
