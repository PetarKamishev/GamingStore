using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.DL.Kafka;
using GamingStore.GamingStore.DL.Repositories;
using GamingStore.GamingStore.Models.Models;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Threading.Tasks.Dataflow;

namespace GamingStore.GamingStore.BL.Services
{
    public class DataflowService : IDataflowService
    {
        private readonly IGetGameTitlesByClientNameService _clientNameService;

        public static List<string> Games { get; set; } = new List<string>();    

        public DataflowService(IGetGameTitlesByClientNameService clientNameService)
        {
            _clientNameService = clientNameService;
        }
    

        public List<string> getGames() { return Games; }
        public Task SendAsync(Orders order)
        {                 
                var clientName = order.ClientName;
                var transformBlock = new TransformBlock<string, List<string>>(async clientName =>
                {                  
                    return  await _clientNameService.GetGameTitlesByClientName(clientName);
                }, new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    SingleProducerConstrained = true
                });

                var actionBlock = new ActionBlock<List<string>>(gamesList =>
                {
                    foreach (var game in gamesList)
                    {
                        Games.Add(game);
                    Console.WriteLine($" Client: {clientName} | Game title: {game}\n");
                    }
                }, new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    SingleProducerConstrained = true
                });
                transformBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });
                transformBlock.Post(clientName);
                transformBlock.Complete();
                actionBlock.Completion.Wait();
            
            return Task.CompletedTask;
        }
    }
}