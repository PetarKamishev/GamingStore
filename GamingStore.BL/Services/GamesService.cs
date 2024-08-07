using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.DL.InMemoryDb;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models;

namespace GamingStore.GamingStore.BL.Services
{
    public class GamesService : IGamesService
    {
        private readonly IGamesRepository _gamesRepository;

        public GamesService(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async Task AddGame(Games game)
        {
            await _gamesRepository.AddGame(game);
        }

        public async Task<List<Games>> GetAllGames()
        {
            return  _gamesRepository.GetAllGames();
        }

        public async Task<Games> GetGame(int id)
        {
            return  _gamesRepository.GetGame(id);
        }


        public async Task<Games> GetGame(string title)
        {
            return  _gamesRepository.GetGame(title);
        }

        public async Task RemoveGame(int id)
        {
            await _gamesRepository.RemoveGame(id);
        }



        public async Task<List<Games>> SearchByTag(string GameTag)
        {
            return  _gamesRepository.SearchByTag(GameTag);
        }

    }
}
