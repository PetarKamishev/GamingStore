using Confluent.Kafka;
using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.DL.Kafka;
using GamingStore.GamingStore.Models.Models;

namespace GamingStore.GamingStore.BL.Services
{
    public class ProduceOrderService : IProduceOrderService
    {
        private KafkaProducer _producer;        

        public ProduceOrderService(KafkaProducer producer)
        {
            _producer = producer;
        }       

        public async Task ProduceOrder(Orders orders)
        {
            await _producer.ProduceSale("gamingstoresales-events", new Message<Guid, Orders>
            {
                Key = Guid.NewGuid(),
                Value = orders
            }) ;
        }
    }
}
