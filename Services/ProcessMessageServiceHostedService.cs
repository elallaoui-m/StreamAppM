using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace StreamApp.Services
{
    public class ProcessMessageServiceHostedService : IProcessMessage
    {
        public TaskCompletionSource<object> taskCompletionSource;
        public IProcessMessageService processMessage;
        public WebSocket socket;

        public ProcessMessageServiceHostedService(IProcessMessageService processMessage)
        {
            this.processMessage = processMessage;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
             await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {



            processMessage.AddSocket(socket, new TaskCompletionSource<object>());
            await processMessage.DoWork(stoppingToken);
            //using (var scope = Services.CreateScope())
            //{
            //    var scopedProcessingService =
            //        scope.ServiceProvider
            //            .GetRequiredService<IProcessMessageService>();



            //    await scopedProcessingService.DoWork(stoppingToken);
            //}
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override void AddSocket(WebSocket socket)
        {
            this.socket = socket;
        }
    }
}
