using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StreamApp.Services;

namespace StreamApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices(services =>
                {
                    // for the service worker
                  //  services.AddHostedService<ProcessMessageServiceHostedService>();
                    services.AddScoped<IProcessMessageService, ProcessMessageService>();
                    services.AddScoped<IProcessMessage, ProcessMessageServiceHostedService>();

                });
    }
}
