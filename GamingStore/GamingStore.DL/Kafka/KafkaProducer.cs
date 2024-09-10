using Confluent.Kafka;
using GamingStore.GamingStore.Models.Models;
using GamingStore.GamingStore.Models.Requests;
using MessagePack;

namespace GamingStore.GamingStore.DL.Kafka
{
    public class KafkaProducer
    {
        private readonly IProducer<Guid, Orders> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092", 
                
            };

            _producer = new ProducerBuilder<Guid, Orders>(config)
                .SetKeySerializer(new MessagePackSerializer<Guid>())
                .SetValueSerializer(new MessagePackSerializer<Orders>())
                .Build();                 
        }

        public async Task ProduceSale(string topic, Message<Guid, Orders> message)
        {
            await _producer.ProduceAsync("gamingstoresales-events", message);
        }
    }
}
