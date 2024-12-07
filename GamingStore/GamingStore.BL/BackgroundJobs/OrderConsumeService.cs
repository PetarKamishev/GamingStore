using Confluent.Kafka;
using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.DL.Kafka;
using GamingStore.GamingStore.Models.Models;

namespace GamingStore.GamingStore.BL.BackgroundJobs
{
    public class OrderConsumeService : IHostedService
    {
        public IConsumer<Guid, Orders> _consumer;
        public static List<Orders> _orders = new List<Orders>();
        private IDataflowService _dataflowService;      

        public OrderConsumeService(IDataflowService dataflowService)
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = $"{Guid.NewGuid()}",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Guid, Orders>(config).SetKeyDeserializer(new MessagePackDeserializer<Guid>())
                .SetValueDeserializer(new MessagePackDeserializer<Orders>())
                .Build();       
            _dataflowService = dataflowService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                Console.WriteLine("Consuming Begins!");
                _consumer.Subscribe("gamingstoresales-events");
                while(!cancellationToken.IsCancellationRequested)
                {
                    var result =  _consumer.Consume();
                    Console.WriteLine(result.Message.Value.ClientName);
                    if (result != null)
                    {
                        _orders.Add(result.Message.Value);
                        _dataflowService.SendAsync(result.Message.Value);
                    }
                }            
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public  List<Orders> GetOrders() { return _orders; }
        
    }
}
