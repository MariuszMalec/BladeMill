using BladeMill.BLL;
using BladeMill.BLL.Enums;
using BladeMill.BLL.Services;
using System;
using System.Linq;
using System.Threading;

namespace BladeMill.ConsoleApp.FindErrorsInNc
{
    public class MainMenuFindErrors
    {
        private static string[] mainMenuItem = { };
        public static void ShowMainMenu()
        {

            var files = new FileService();
            var list = files.GetListSelectedFiles(@"C:\tempNC", ".MPF");
            list.AddRange(files.GetListSelectedFiles(@"C:\tempNC", "01.NC"));
            list.AddRange(files.GetListSelectedFiles(@"C:\tempNC", ".mpf"));

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
                    Console.WriteLine("||    Welcome checking of NCCode                      ||");
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
                    ViewErrors(currentItem);
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
        private async static void ViewErrors(short currentItem)
        {
            var checkNc = new NcCodeCheckService();
            Console.WriteLine($"Czekaj, sprawdzanie programow ...");
            var fileService = new FileService();
            var ncService = new ToolNcService();
            var mainProgram = mainMenuItem[currentItem];

            var machineFactory = new MachineServiceFactory();
            var ncFile = machineFactory.CreateMachine(TypeOfFile.ncFile);
            var Machine = ncFile.GetMachine(mainProgram).MachineName;
                        
            await checkNc.FindErrorsInNcCode(mainProgram);

            Console.WriteLine($"\n--------------------------------------------------------------------------------------");
            checkNc.FindErrorsLimitsNcCode(mainMenuItem[currentItem], 590.0, 250.0, true);
            checkNc.SetWarningIfIsToManySubprograms(mainMenuItem[currentItem]);
            var errors = checkNc.GetAllErrors();
            Console.WriteLine($"--------------------------------------------------------------------------------------");
            Console.WriteLine("Pokaz co sprawdza program");
            checkNc.GetWhatIsCheck().ForEach(m => Console.WriteLine(m));
            Console.WriteLine($"--------------------------------------------------------------------------------------");
            checkNc.ShowAllErrors();
            Console.WriteLine($"--------------------------------------------------------------------------------------");
            checkNc.ShowPreoloadTools(mainMenuItem[currentItem]);
            Console.WriteLine($"--------------------------------------------------------------------------------------");
        }
    }
}
