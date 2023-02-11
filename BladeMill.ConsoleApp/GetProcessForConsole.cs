using BladeMill.BLL.Services;
using System;

namespace BladeMill.ConsoleApp
{
    class GetProcessForConsole
    {
        public void KillProcess(string procesName)
        {
            var procesService = new ProcessService();
            var excel = procesService.GetProcess("EXCEL");
            if (excel != null)
            {
                Console.WriteLine($"Czy zabic proces {excel} 1-Yes, 2-No ?");
                int selectedKey;
                while (!int.TryParse(Console.ReadLine(), out selectedKey))
                {
                    Console.WriteLine("This is not a number! Enter 1 or 2");
                    Console.ReadKey();
                }
                if (selectedKey == 1) { excel.Kill(); }
                if (selectedKey == 2) { Environment.Exit(1); }
            }
        }
    }
}
