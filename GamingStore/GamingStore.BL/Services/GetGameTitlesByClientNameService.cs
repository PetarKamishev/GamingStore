using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.DL.Interfaces;

namespace GamingStore.GamingStore.BL.Services
{
    public class GetGameTitlesByClientNameService : IGetGameTitlesByClientNameService
    {
        private readonly IOrdersRepository _ordersRepository;

        public GetGameTitlesByClientNameService()
        {
        }

        public GetGameTitlesByClientNameService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }
        public async Task<List<string>> GetGameTitlesByClientName(string gameTitle)
        {
            return await _ordersRepository.GetGameTitlesByClientName(gameTitle);
        }
    }
}
