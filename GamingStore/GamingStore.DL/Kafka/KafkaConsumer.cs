using Confluent.Kafka;
using GamingStore.GamingStore.Models.Models;

namespace GamingStore.GamingStore.DL.Kafka
{
    public class KafkaConsumer
    {
        private static IConsumer<Guid, Orders> _consumer;
        
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Guid, Orders>(config)
                .SetKeyDeserializer(new MessagePackDeserializer<Guid>())
                .SetValueDeserializer(new MessagePackDeserializer<Orders>())
                .Build();

            var topics = new List<string>()
            {
                "gamingstoresales-events"
            };

            _consumer.Subscribe(topics);

            Console.WriteLine($"{_consumer.Consume()}"); 
            return Task.CompletedTask;
        }
    }
}
