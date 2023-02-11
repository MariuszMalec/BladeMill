using BladeMill.BLL.Services;
using System;
using System.Linq;

namespace BladeMill.ConsoleApp.HelpProgramms
{
    public class MainMenuShowHelpPrograms
    {
        private static string[] mainMenuItem = { };

        private static ProgramExeService programExeService = new ProgramExeService();
        public static void ShowMainMenu()
        {
            var collection = programExeService.GetFullName().ToArray();
            foreach (var item in collection)
            {
                mainMenuItem = mainMenuItem.Append(item).ToArray();
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
                    Console.WriteLine("||    Welcome in Help Programs                        ||");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("========================================================");
                    for (int i = 0; i < mainMenuItem.Length; i++)
                    {
                        if (currentItem == i)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(">>");
                            Console.WriteLine(System.IO.Path.GetFileName(mainMenuItem[i]) + "<<");
                        }
                        else
                        {
                            Console.WriteLine(System.IO.Path.GetFileName(mainMenuItem[i]));
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
                    StartExe(mainMenuItem[currentItem]);

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

        private static void StartExe(string currentItem)
        {
            Console.WriteLine($"------------------------------------------------------------------");
            programExeService.StartNewProcess(currentItem);
            Console.WriteLine($"------------------------------------------------------------------");
        }
    }
}
