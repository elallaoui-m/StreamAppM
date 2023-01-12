using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StreamApp.middlewares;
using StreamApp.Models;
using StreamApp.Services;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson();
            //services.AddControllers()

            var producerConfig = new ProducerConfig();
            var consumerConfig = new ConsumerConfig();
            Configuration.Bind("producer", producerConfig);
            Configuration.Bind("consumer", consumerConfig);

            services.AddSingleton<ProducerConfig>(producerConfig);
            services.AddSingleton<ConsumerConfig>(consumerConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(30),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<ManageSendingMessages>();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            

            //app.UseWebSockets(webSocketOptions);

            //app.Map("/ws", builder =>
            //{
            //    builder.Use(async (context, next) =>
            //    {
            //        if (context.WebSockets.IsWebSocketRequest)
            //        {
            //            //var socket = await context.WebSockets.AcceptWebSocketAsync();
            //            var socketFinishedTcs = new TaskCompletionSource<object>();
            //            //ChatMessage msg = new ChatMessage()
            //            //{
            //            //    content = "received",
            //            //    sender = "host"
            //            //};
            //            //var str = JsonConvert.SerializeObject(msg);
            //            //var encoded = Encoding.UTF8.GetBytes(str);
            //            //await socket.SendAsync(new ArraySegment<byte>(encoded, 0, encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            //            //ProcessMessageService.AddSocket(socket, socketFinishedTcs);
            //            if(context != null)
            //            ProcessMessageService.AddContext(context);
            //            await socketFinishedTcs.Task;
            //        }
            //        else
            //        {
            //            await next();
            //        }

            //    });
            //});

            








        }
    }
}
