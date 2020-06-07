using Confluent.Kafka;
using System;

namespace StreamApp.utils
{
    public class Producer
    {
        public string BootstrapServers = "localhost:9092";
        public string Topic = "NetCoreStreamTest";

        public async void sendMessage(string message)
        {

            using (var p = new ProducerBuilder<Null, string>(GetConfig()).Build())
            {
                try
                {
                   await p.ProduceAsync(Topic, new Message<Null, string> { Value = message });
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine(e.Error.Reason);
                }
            }
        }

        public ProducerConfig GetConfig()
        {
            return new ProducerConfig { BootstrapServers = BootstrapServers };
        }
    }
}
