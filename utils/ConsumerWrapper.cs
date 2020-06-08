using Confluent.Kafka;
using System;
using System.Diagnostics;
using System.Threading;

namespace StreamApp.utils
{
    
    public class ConsumerWrapper
    {
        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private IConsumer<string,string> _consumer;
        private static readonly Random rand = new Random();
        public ConsumerWrapper(ConsumerConfig config,string topicName)
        {
            this._topicName = topicName;
            this._consumerConfig = config;
            this._consumer = new ConsumerBuilder<string,string>(this._consumerConfig).Build();
            this._consumer.Subscribe(topicName);
        }
        public string readMessage(){
            var consumeResult = this._consumer.Consume();
            Debug.WriteLine("psps");
            return consumeResult?.Message?.Value;
        }
    }
}