using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace BladeMill.ConsoleApp
{
    class Program
    {
        private const int MinimizeSizeConsoleWindow = 170;
        static void Main(string[] args)
        {
            //
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (Console.BufferWidth < MinimizeSizeConsoleWindow)
            {
                Console.SetWindowSize(MinimizeSizeConsoleWindow, 40);
            }

            //czytanie seriloga appsetings
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            ILogger logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .CreateLogger();

            logger.Information("App starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(logger);
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<AppStarting>(host.Services);
            svc.Run(args);
        }
        static void BuildConfig(IConfigurationBuilder builder)//TODO to trzeba dodac!
        {
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                ;
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            string text = $"{e.ExceptionObject}";
            File.WriteAllText("C:/temp/error.txt", text);
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
