using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.DL.Repositories;

namespace GamingStore.GamingStore.BL.Services
{
    public class GetGameTitlesByClientNameService : IGetGameTitlesByClientNameService
    {
        private readonly IOrdersRepository _ordersRepository;


        public GetGameTitlesByClientNameService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }
        public async Task<List<string>> GetGameTitlesByClientName(string clientName)
        {
            var result = await _ordersRepository.GetGameTitlesByClientName(clientName);
            return result;
        }
    }
}
