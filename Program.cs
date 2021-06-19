using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Dotnet5.Authentication;
using System;
using dotnet5.Migrations;

namespace dotnet5
{
    public class Program
    {
        [LoaderOptimization(LoaderOptimization.SingleDomain)]
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().MigrateDatabase<ApplicationDbContext>().Run();
            // CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((context, logging) =>
            {
                if (context.HostingEnvironment.IsProduction())
                {
                    logging.ClearProviders();
                    logging.AddJsonConsole();
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}