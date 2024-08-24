using GamingStore.GamingStore.Models.Models;

namespace GamingStore.GamingStore.DL.Interfaces
{
    public interface IGamesRepository
    {
        Task <List<Games>> GetAllGames();
        Task <Games> GetGame(int id);
        Task <Games> GetGame(string title);

        public Task AddGame(Games game);
        public Task RemoveGame(int id);

        Task <List<Games>> SearchByTag(string GameTag);
    }
}
