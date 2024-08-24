using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models.Models;

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
                var result = await _gamesRepository.GetAllGames();
                return result; 
        }          
        

        public async Task<Games> GetGame(int id)
        {
           var result = await (_gamesRepository.GetGame(id));
            return result;
        }


        public async Task<Games> GetGame(string title)
        {
           var result = await (_gamesRepository.GetGame(title));
            return result;
        }

        public async Task RemoveGame(int id)
        {
            await _gamesRepository.RemoveGame(id);
        }

        public async Task<List<Games>> SearchByTag(string GameTag)
        {

            var result = await (_gamesRepository.SearchByTag(GameTag));
            return result;
        }

    }
}
