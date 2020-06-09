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

         private WebSocket socket;
         TaskCompletionSource<object> socketFinishedTcs;





        public ProcessMessageService(ConsumerConfig consumerConfig,
            IConfiguration configuration)
        {
            this.consumerConfig = consumerConfig;
            this.configuration = configuration;
        }


        public async Task DoWork(CancellationToken stoppingToken)
        {
            
            while (!stoppingToken.IsCancellationRequested)
            {
                    try
                    {
                        
                        var topic = configuration["topic"];
                        var consumerHelper = new ConsumerWrapper(consumerConfig, topic);

                        string chatMessage = consumerHelper.readMessage();
                        ChatMessage message = JsonConvert.DeserializeObject<ChatMessage>(chatMessage);
                        var encoded = Encoding.UTF8.GetBytes(chatMessage);

                        
                        await socket.SendAsync(new ArraySegment<byte>(encoded, 0, encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        Debug.WriteLine("There was an error");
                    }
               // await Task.Delay(10000, stoppingToken);
            }
        }

         public void AddSocket(WebSocket _socket, TaskCompletionSource<object> _socketFinishedTcs)
        {
            socket = _socket;
            socketFinishedTcs = _socketFinishedTcs;
        }

        

    }
}
