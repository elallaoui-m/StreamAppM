using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StreamApp.Models;
using StreamApp.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StreamApp.Services
{
    public class ProcessMessageService : IProcessMessageService
    {


        private readonly ConsumerConfig consumerConfig;
        private readonly ProducerConfig producerConfig;
        private readonly IConfiguration configuration;
        public ProcessMessageService(ConsumerConfig consumerConfig, 
            ProducerConfig producerConfig, 
            IConfiguration configuration)
        {
            this.producerConfig = producerConfig;
            this.consumerConfig = consumerConfig;
            this.configuration = configuration;
        }


        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var topic = configuration["topic"];
                var consumerHelper = new ConsumerWrapper(consumerConfig, topic);

                try
                {
                    string  chatMessage = consumerHelper.readMessage(); 
                    ChatMessage message = JsonConvert.DeserializeObject<ChatMessage>(chatMessage);

                    var producerWrapper = new ProducerWrapper(producerConfig, topic);
                    await producerWrapper.writeMessage(JsonConvert.SerializeObject(message));
                }
                catch (Exception)
                {
                    Debug.WriteLine("There was an error");
                }

               
                await Task.Delay(10000, stoppingToken);
            }

        }



    }
}
