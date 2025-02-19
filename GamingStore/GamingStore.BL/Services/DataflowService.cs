using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.Models.Models;
using System.Threading.Tasks.Dataflow;

namespace GamingStore.GamingStore.BL.Services
{
    public class DataflowService : IDataflowService
    {
        private readonly IGetGameTitlesByClientNameService _clientNameService;

        public static List<string> Games { get; set; } = new List<string>();
        public static TransformBlock<string, List<string>>? transformBlock;
        public static ActionBlock<List<string>>? actionBlock;
        private string clientName = "default";

        public DataflowService(IGetGameTitlesByClientNameService clientNameService)
        {
            _clientNameService = clientNameService;
            transformBlock = new TransformBlock<string, List<string>>(async clientName =>
           {
               return await _clientNameService.GetGameTitlesByClientName(clientName);
           }, new ExecutionDataflowBlockOptions
           {
               MaxDegreeOfParallelism = Environment.ProcessorCount,
               SingleProducerConstrained = true
           });
            actionBlock = new ActionBlock<List<string>>(gamesList =>
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
        }

        public List<string> getGames() { return Games; }
        public Task SendAsync(Orders order)
        {
            clientName = order.ClientName;
            if (transformBlock != null && actionBlock != null)
            {
                transformBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });
                transformBlock.Post(clientName);
                transformBlock.Complete();
                actionBlock.Completion.Wait();
            }
            return Task.CompletedTask;
        }
    }
}