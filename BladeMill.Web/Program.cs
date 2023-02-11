using BladeMill.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Filters;
using System.IO;

namespace BladeMill.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            //TODO z appsetting.json for serilog
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();

            ////manual setting for serilog
            //Log.Logger = new LoggerConfiguration()
            //    //.MinimumLevel.Warning()
            //    //.WriteTo.File("log.txt").Filter.ByIncludingOnly(Matching.FromSource<ProgramExeController>()).MinimumLevel.Warning()
            //    //.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Warning)
            //    .Enrich.With(new ThreadIdEnricher())
            //    .WriteTo.Console(
            //        outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
            //    .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseSerilog()//TODO uzycie seriloga
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
