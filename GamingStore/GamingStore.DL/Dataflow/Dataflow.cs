using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Kafka;
using GamingStore.GamingStore.Models.Models;
using System.Threading.Tasks.Dataflow;

namespace GamingStore.GamingStore.DL.Dataflow
{
    public class DataflowConsumeAndProduce
    {
        private ISourceBlock<List<string>> _sourceBlock;

        public DataflowConsumeAndProduce(ISourceBlock<List<string>> sourceBlock)
        {
            _sourceBlock = sourceBlock;
        }

        public DataflowConsumeAndProduce()
        {
            
        }
        public async Task DataflowConsume()
        {
            var consumer = new ConsumeOrderService(new KafkaConsumer());
            var consumedObject = await consumer.ConsumeOrder();
            var clientName = consumedObject.ClientName;
            var service = new GetGameTitlesByClientNameService();
            var transformBlock = new TransformBlock<string, List<string>>(clientName => service.GetGameTitlesByClientName(clientName));
            transformBlock.Post(clientName);
            var sourceBlock = _sourceBlock;
            transformBlock.LinkTo((ITargetBlock<List<string>>)sourceBlock, new DataflowLinkOptions());
            sourceBlock.Receive();          
            await Console.Out.WriteLineAsync($"{sourceBlock.Receive()}");
            transformBlock.Complete();
            sourceBlock.Complete();
        }
    }
}
