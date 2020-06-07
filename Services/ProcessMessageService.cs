using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StreamApp.Models;
using StreamApp.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StreamApp.Services
{
    public class ProcessMessageService : BackgroundService
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string chatMessage = "";

            while (!stoppingToken.IsCancellationRequested)
            {
               
                    var topic = configuration["topic"];
                    var consumerHelper = new ConsumerWrapper(consumerConfig, topic);
                     try 
                    {

                          chatMessage = consumerHelper.readMessage();
                    }
                    catch (Exception)
                    {

                    }
                    //Deserilaize 
                    ChatMessage message = JsonConvert.DeserializeObject<ChatMessage>(chatMessage);

                    //TODO:: Process Order
                    //Console.WriteLine($"Info: OrderHandler => Processing the order for {order.productname}");
                    //order.status = OrderStatus.COMPLETED;

                    //Write to ReadyToShip Queue

                    var producerWrapper = new ProducerWrapper(producerConfig, topic);
                    await producerWrapper.writeMessage(JsonConvert.SerializeObject(message));
            }   
        }
    }
}
