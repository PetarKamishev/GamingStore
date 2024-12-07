using Confluent.Kafka;
using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Repositories;
using GamingStore.GamingStore.Models.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Storage;
using System.Configuration;
using System.Threading;

namespace GamingStore.GamingStore.DL.Kafka
{
    public class KafkaConsumer
    {
        private IConsumer<Guid, Orders> _consumer;
        private readonly IOrdersService _ordersService;
        public static List<Orders> _orders = new List<Orders>();
        public KafkaConsumer(IConsumer<Guid, Orders> consumer, IOrdersService ordersService)
        {
            _consumer = consumer;
            _ordersService = ordersService;
        }
        public KafkaConsumer(IOrdersService ordersService)
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = $"{Guid.NewGuid()}",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Guid, Orders>(config)
                .SetKeyDeserializer(new MessagePackDeserializer<Guid>())
                .SetValueDeserializer(new MessagePackDeserializer<Orders>())
                .Build();
            _ordersService = ordersService;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var topics = new List<string>()
            {
                "gamingstoresales-events"
            };

            _consumer.Subscribe(topics);

            var ordersCount = await _ordersService.GetOrdersCount();
            var ordersList = await _ordersService.GetAllOrders();

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken);
                await Console.Out.WriteLineAsync(result.Message.Value.ToString());
                if (result != null)
                {
                    var order = result.Message.Value;
                    if (ordersList.Contains(order))
                        _orders.Add(order);
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
            _consumer.Close();
        }


        public async Task<List<Orders>> GetOrdersAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            await ExecuteAsync(cancellationToken);
            cancellationTokenSource.Cancel();
            return _orders;
        }


    }
}
