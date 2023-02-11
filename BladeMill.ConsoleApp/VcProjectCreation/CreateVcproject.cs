using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Serilog;
using System;
using System.IO;

namespace BladeMill.ConsoleApp.VcProjectCreation
{
    public class CreateVcproject
    {
        public static void CreateFromXml(string toolsXmlFile, ILogger logger)
        {
            var readvcproject = new XMLVCProjectService(toolsXmlFile);

            //var allDataFromVcProject = readvcproject.GetAllDataFromVcProject();
            //Console.WriteLine("------------------------------------------------------------------");
            //Console.WriteLine($"MachineMch : {allDataFromVcProject.MachineMch}");
            //Console.WriteLine($"ControlCtl : {allDataFromVcProject.ControlCtl}");
            //Console.WriteLine($"NcProgram  : {allDataFromVcProject.NcProgram}");
            //Console.WriteLine($"ToolLibrary: {allDataFromVcProject.ToolLibrary}");
            //Console.WriteLine("------------------------------------------------------------------");
            //allDataFromVcProject.StlGeometry.ForEach(s => Console.WriteLine($"StlGeo : {s}"));
            //Console.WriteLine("------------------------------------------------------------------");
            //allDataFromVcProject.Subroutine.ForEach(s => Console.WriteLine($"Subroutine : {s}"));

            readvcproject.ReplaceToolLbrary();
            readvcproject.ReplaceControlFile();
            readvcproject.ReplaceMachineFile();
            readvcproject.ReplaceNcProgramFile();
            readvcproject.ReplaceSubroutines();

            readvcproject.ReplacaAllClampingSystem();

            RotateStlIfNeedIfXml(readvcproject);

            var generatedVcProject = readvcproject.GetVcProjectOutPutFile();
            if (File.Exists(generatedVcProject))
                logger.Information($"VcProject zostal wygenerowny tutaj {generatedVcProject}");
            else
            {
                logger.Error($"VcProject zostal nie zostal wykonany! {generatedVcProject}");
            }
        }

        public static void CreateFromNc(string mainProgram, ILogger logger)
        {
            if (File.Exists(mainProgram))
            {
                var readvcproject = new NcVCProjectService(mainProgram);

                var adapterRoot = "adapterRoot";
                var blade = "lopatka";
                var material = "material";

                readvcproject.ReplaceToolLbrary();
                readvcproject.ReplaceControlFile();
                readvcproject.ReplaceMachineFile();
                readvcproject.ReplaceNcProgramFile();
                readvcproject.ReplaceSubroutines();
                readvcproject.ReplaceGEOMETRY("m_CLAMP_ADAPTER", adapterRoot);
                readvcproject.ReplaceGEOMETRY("m_CLAMPING_GEOMETRY", material);
                readvcproject.ReplaceGEOMETRY("m_BladeBody", blade);

                readvcproject.ReplacaAllClampingSystem();

                readvcproject.RotateStl(readvcproject, mainProgram, adapterRoot);

                var generatedVcProject = readvcproject.GetVcProjectOutPutFile();
                if (File.Exists(generatedVcProject))
                    logger.Information($"VcProject zostal wygenerowny tutaj {generatedVcProject}");
                else
                {
                    logger.Error($"VcProject zostal nie zostal wykonany! {generatedVcProject}");
                }
            }
            else
            {
                logger.Warning($"Brak programu glownego {mainProgram}");
            }
        }

        private static void RotateStlIfNeedIfXml(XMLVCProjectService readvcproject)
        {
            var toolXmlFile = new ToolsXmlFile();
            var machine = toolXmlFile.MACHINE;
            if (machine == MachineEnum.HSTM300.ToString() || machine == MachineEnum.HSTM500M.ToString())
            {
                readvcproject.RotationStl("m_CLAMP_ADAPTER.stl", "K", 180);
            }
            else if (machine == MachineEnum.HX151.ToString())
            {
                readvcproject.RotationStl("m_CLAMP_ADAPTER.stl", "I", 90);
            }
            else if (machine == MachineEnum.HSTM300HD.ToString())
            {
                readvcproject.RotationStl("m_CLAMP_ADAPTER.stl", "K", 180);
            }
            else
            {
                Console.WriteLine("Rotate stl is not need for this machine");
            }
        }
        private static void RotateStlIfNeedIfNc(NcVCProjectService readvcproject, string mainProgram)
        {
            NcMainProgramService _mainProgramService = new NcMainProgramService(mainProgram);
            var machine = _mainProgramService.GetMachine();
            if (machine == MachineEnum.HSTM300.ToString() || machine == MachineEnum.HSTM500M.ToString())
            {
                readvcproject.RotationStl("m_CLAMP_ADAPTER.stl", "K", 180);
            }
            else if (machine == MachineEnum.HX151.ToString())
            {
                readvcproject.RotationStl("m_CLAMP_ADAPTER.stl", "I", 90);
            }
            else if (machine == MachineEnum.HSTM300HD.ToString())
            {
                readvcproject.RotationStl("m_CLAMP_ADAPTER.stl", "K", 180);
            }
            else
            {
                Console.WriteLine("Rotate stl is not need for this machine");
            }
        }
    }
}
