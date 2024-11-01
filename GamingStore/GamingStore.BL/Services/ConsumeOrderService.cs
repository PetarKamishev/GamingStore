using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.DL.Kafka;

namespace GamingStore.GamingStore.BL.Services
{
    public class ConsumeOrderService : IConsumeOrderService
    {
        private readonly KafkaConsumer _consumer;

        public ConsumeOrderService(KafkaConsumer consumer)
        {
            _consumer = consumer;
        }
        public async Task<Models.Models.Orders> ConsumeOrder()
        {
           var result = await _consumer.ExecuteAsync();
            return result;
        }
    }
}
