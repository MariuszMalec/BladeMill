using BladeMill.BLL.Services;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace BladeMill.ConsoleApp.ToolNcTests
{
    public static class ToolNc
    {
        public static void ShowToolsFromNCCode(string file, ILogger log)
        {
            if (File.Exists(file))
            {
                var toolNcService = new ToolNcService();
                var tools = toolNcService.LoadToolsFromFile(file);
                tools.Where(t => t.BatchFile != null)
                .ToList()
                .ForEach(t => Console.WriteLine($"{t.BatchFile.ToString().PadRight(15, ' ')}" +
                $" | {t.BatchFile.ToString().Select(x => File.Exists(t.BatchFile)).FirstOrDefault().ToString().PadRight(6, ' ')} " +
                $" | {t.Description.ToString().PadRight(30, ' ')} " +
                $" | {t.ToolSet.ToString().PadRight(6, ' ')} " +
                $" | {t.ToolID.ToString().PadRight(6, ' ')} " +
                $" | {t.ToolIDPreLoad.ToString().PadRight(6, ' ')} " +
                $" | {t.ToolDiam.ToString().PadRight(8, ' ')} " +
                $" | {t.ToolCrn.ToString().PadRight(6, ' ')} " +
                $" | {t.Toollen.ToString().PadRight(6, ' ')} " +
                $" | {t.Spindle.ToString().PadRight(6, ' ')} " +
                $" | {t.Feedrate.ToString().PadRight(6, ' ')} " +
                $" | {t.MaxMillTime.ToString().PadRight(6, ' ')} " +                
                $" | {t.Machine.ToString().PadRight(10, ' ')}"));
                Console.WriteLine($"------------------------------------------------------------------");
            }
            else
            {
                log.Error($"Brak pliku : {file}");
            }
        }
    }
}
