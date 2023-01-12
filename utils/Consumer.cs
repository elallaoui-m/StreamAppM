
using Confluent.Kafka;
using System;
using System.Net.WebSockets;
using System.Threading;

namespace StreamApp.utils
{
    public class Consumer
    {
        public string Topic = "NetCoreStreamTest";
        public string BootstrapServers = "localhost:9092";
        public string GroupId = "NetCoreStreamTest";

        public void retriveMessages()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(GetConfig()).Build())
            {
                consumer.Subscribe(Topic);
                CancellationTokenSource cts = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        try
                        {
                            consumer.Consume(cts.Token);
                            // sync with sockets
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine(e.Error.Reason);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    consumer.Close();
                }
            }
        }

        public ConsumerConfig GetConfig()
        {
            return new ConsumerConfig
            {
                GroupId = GroupId,
                BootstrapServers = BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }
    }

   
}
