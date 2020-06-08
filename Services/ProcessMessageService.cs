using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StreamApp.Models;
using StreamApp.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamApp.Services
{
    public class ProcessMessageService : IProcessMessageService
    {


        private readonly ConsumerConfig consumerConfig;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextA;
        private HttpContext httpContext;

         private WebSocket socket;
         TaskCompletionSource<object> socketFinishedTcs;




        public ProcessMessageService(ConsumerConfig consumerConfig,
            IHttpContextAccessor httpContextA,
            IConfiguration configuration)
        {
            this.consumerConfig = consumerConfig;
            this.configuration = configuration;
            this.httpContextA = httpContextA;
             socketFinishedTcs = new TaskCompletionSource<object>();

        }


        public async Task DoWork(CancellationToken stoppingToken)
        {

            while (httpContext == null)
            {
                httpContext = httpContextA.HttpContext;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                
                
                try
                {
                   
                        socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                        var topic = configuration["topic"];
                        var consumerHelper = new ConsumerWrapper(consumerConfig, topic);

                        string chatMessage = consumerHelper.readMessage();
                        ChatMessage message = JsonConvert.DeserializeObject<ChatMessage>(chatMessage);
                        var encoded = Encoding.UTF8.GetBytes(chatMessage);


                        //var buffer = new byte[1024 * 4];
                        //WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        //if(message.sender == )
                        await socket.SendAsync(new ArraySegment<byte>(encoded, 0, encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        //socketFinishedTcs.TrySetResult(result);

                        Debug.WriteLine(chatMessage);
                   
                    await SendSocketMsgToClient(chatMessage);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine("There was an error");
                }

                await socketFinishedTcs.Task;
                await Task.Delay(10000, stoppingToken);
            }


        }

        private async Task SendSocketMsgToClient(string msg)
        {
            var encoded = Encoding.UTF8.GetBytes(msg);
            WebSocket webSocket = await _httpContextAccessor.HttpContext.WebSockets.AcceptWebSocketAsync();
            await webSocket.SendAsync(new ArraySegment<byte>(encoded, 0, encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

    }
}
