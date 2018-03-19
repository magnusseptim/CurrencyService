using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using Serilog;
using System.Diagnostics;
using CurrencyService.Services;
using CurrencyService.Services.Interfaces;

namespace CurrencyService
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                      .Build();

            Log.Logger = new LoggerConfiguration().ReadFrom
                                                  .Configuration(Configuration)
                                                  .CreateLogger();
            Serilog.Debugging.SelfLog.Enable(msg =>Console.WriteLine(msg));

            try
            {
                Log.Information("It is alive, buhahahah...");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseStartup<Startup>()
                   .UseApplicationInsights()
                   .UseConfiguration(Configuration)
                   .UseSerilog()
                   .Build();
    }
}
