using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace BladeMill.ConsoleApp.ToolsXmlTests
{
    public class ToolXmlFile
    {
        public static void Tests()
        {
            var toolXmlModel = new ToolsXmlFile();
            Console.WriteLine($"PRGNUMBER       = {toolXmlModel.PRGNUMBER}");
            Console.WriteLine($"MACHINE         = {toolXmlModel.MACHINE}");
            Console.WriteLine($"MainProgramFile = {toolXmlModel.GetMainProgramFileFromCurrentToolsXml()}");
        }

        public static void ShowToolsFromToolsXml(string file, ILogger logger)
        {
            if (File.Exists(file))
            {
                Console.WriteLine($"------------------------------------------------------------------");
                var toolsxmlService = new ToolXmlService();
                var toolsxml = toolsxmlService.LoadToolsFromFile(file);
                toolsxml.Where(t => t.BatchFile != null)
                    .ToList()
                    .ForEach(t => Console.WriteLine($"{t.BatchFile.ToString().PadRight(15, ' ')}" +
                    $" | {t.Description.ToString().PadRight(30, ' ')} " +
                    $" | {t.ToolSet.ToString().PadRight(6, ' ')} " +
                    $" | {t.ToolID.ToString().PadRight(6, ' ')} " +
                    $" | {t.ToolDiam.ToString().PadRight(8, ' ')} " +
                    $" | {t.ToolCrn.ToString().PadRight(6, ' ')} " +
                    $" | {t.Toollen.ToString().PadRight(6, ' ')} " +
                    $" | {t.Spindle.ToString().PadRight(6, ' ')} " +
                    $" | {t.Feedrate.ToString().PadRight(6, ' ')} " +
                    $" | {t.MaxMillTime.ToString().PadRight(6, ' ')} " +
                    $" | {t.Machine.ToString().PadRight(10, ' ')}"));
                Console.WriteLine($"------------------------------------------------------------------");
            }
            else { logger.Error($"Brak pliku : {file}"); }
        }
    }
}
