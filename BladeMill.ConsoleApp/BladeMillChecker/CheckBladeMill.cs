using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.ConsoleApp.BladeMillChecker
{
    static class CheckBladeMill
    {
        public static void Test()
        {
            //------------------------------------------------
            //sprawdz czy wlaczony bladeMill
            //------------------------------------------------
            Console.WriteLine("Checking Blade Mill if is open ... ");
            var proces = new ProcessService();
            var procesBM = proces.GetProcess("Alstom.BladeMill.Gui");
            if (procesBM != null)
            {
                Console.WriteLine($"Proces {procesBM} will be killed!");
                Log.Warning($"Proces {procesBM} will be killed!");
                procesBM.Kill();
            }
            //------------------------------------------------
            //sprawdz zmienna CleverHome
            //------------------------------------------------
            Log.Information("Checking variable CleverHome ... ");
            var pathData = new PathDataBase();
            Log.Information(pathData.GetCleverHome());//to musi byc brane z ze zmiennej systemu!
            //------------------------------------------------
            //sprawdz RootEngDir
            //------------------------------------------------            
            Log.Information($"Checking RootEngDir ... ");
            var appService = new AppXmlConfService();
            var rootEngDir = appService.GetRootEngDir();
            if (!Directory.Exists(rootEngDir))
            {
                Log.Warning($"no exist dir {rootEngDir}");
            }
            else { Console.WriteLine($"RootEngDir set to {rootEngDir}"); }
            //-----------------------------------
            //sprawdz wersje skryptow
            //-----------------------------------
            var pathDataBase = new PathDataBase();
            var appXmlFile = pathDataBase.GetApplicationConfFile();

            if (File.Exists(appXmlFile))
            {
                if (appService.GetBladeMillScriptsDir().Contains("General Electric"))
                {
                    Console.WriteLine("Witam na onedrive wersji skryptow");
                }
                else if (appService.GetBladeMillScriptsDir().Contains("S:"))
                {
                    Console.WriteLine("Witam na sieciowej wersji skryptow");
                }
                else if (appService.GetBladeMillScriptsDir().Contains("Users"))
                {
                    Console.WriteLine("Witam na domyślnej wersji skryptow! Automat nie bedzie dzialal!!");
                }
                else
                {
                    Console.WriteLine("Witam na lokalnej wersji skryptow");
                }
            }
            else
            {
                Log.Error($"BladeMill i Automat nie bedzie dzialal!! brak pliku {appXmlFile}");
                Console.WriteLine($"BladeMill i Automat nie bedzie dzialal!! brak pliku {appXmlFile}");
                Console.WriteLine("Program zostanie zatrzymany. Wcisnij dowolny klawisz!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            //--------------------------------------------
            //Pokaz dane z pliku xml
            //--------------------------------------------

            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"--- Wybierz konfiguracje BladeMilla ---");
            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"1 - lokalna");
            Console.WriteLine($"2 - sieciowa");
            Console.WriteLine($"3 - onedrive");
            Console.WriteLine($"4 - exit");
            Console.WriteLine($"---------------------------------------");
            int selectedKey;
            while (!int.TryParse(Console.ReadLine(), out selectedKey))
            {
                Console.WriteLine("This is not a number! Select again!");
                Console.ReadKey();
            }

            var xmlService = new FixAppConfFileService();

            if (selectedKey == 1)
            {
                Console.WriteLine("Wersja lokalna");
                var mainDir = @"C:\";
                xmlService.CreateNewAppConfFile(mainDir, "C");
            }
            else if (selectedKey == 2)
            {
                Console.WriteLine("Wersja sieciowa");
                var mainDir = @"S:\SHARES\CLEVER";
                if (!Directory.Exists(mainDir))
                {
                    Log.Error($"Brak dysku {mainDir}");
                }
                xmlService.CreateNewAppConfFile(mainDir, "S");
            }
            else if (selectedKey == 3)
            {
                Console.WriteLine("Wersja onedrive");
                var mainDir = pathDataBase.GetDirOneDriveClever();
                xmlService.CreateNewAppConfFile(mainDir, "OneDrive");
            }
            else if (selectedKey == 4)
            {
                Console.WriteLine("Koniec");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Wybrales bledny klawisz!");
                Environment.Exit(0);
            }
        }
    }
}
