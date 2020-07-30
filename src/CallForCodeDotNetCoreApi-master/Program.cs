using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CallForCodeApi
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });


        //public static void Main(string[] args)
        //    {
        //        var config = new ConfigurationBuilder()
        //                         .AddCommandLine(args) 
        //                         .Build();
        //        var host = new WebHostBuilder() 
        //                       .UseKestrel() 
        //                       .UseConfiguration(config) 
        //                       .UseStartup<Startup>()
        //                       .Build();
        //        host.Run();
        //    }
        protected Program() { }
        public static void Main(string[] args)
        {
            IWebHost host = CreateHostBuilder(args).Build();
            ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("From Program running host now..");
            host.Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                //.UseKestrel()
                .UseStartup<Startup>()
                .ConfigureLogging((hostingcontext, logger) =>
                {
                    logger.AddConsole();
                });

        }

    }
}