using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Contains("--dev"))
            {
                CreateDevHostBuilder(args).Build().Run();
            }
            else
            {
                CreateHostBuilder(args).Build().Run();
            }
           
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static IHostBuilder CreateDevHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    HostConfig.CertificateFileLocation = context.Configuration["CertificateFileLocation"];
                    HostConfig.CertificatePassword = context.Configuration["CertPassword"];
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(opt =>
                    {
                        opt.ListenAnyIP(5001, listenOpt =>
                        {
                            listenOpt.UseHttps(HostConfig.CertificateFileLocation, HostConfig.CertificatePassword);
                        });
                        opt.ListenAnyIP(5000);
                    });

                    webBuilder.UseStartup<Startup>();
                });


    }

    public static class HostConfig
    {
        public static string CertificateFileLocation { get; set; }
        public static string CertificatePassword { get; set; }
    }
}
