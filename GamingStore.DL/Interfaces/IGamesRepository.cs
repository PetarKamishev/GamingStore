using GamingStore.GamingStore.Models;

namespace GamingStore.GamingStore.DL.Interfaces
{
    public interface IGamesRepository
    {
        List<Games> GetAllGames();
        Games GetGame(int id);
        Games GetGame(string title);

        public Task AddGame(Games game);
        public Task RemoveGame(int id);

        List<Games> SearchByTag(string GameTag);
    }
}
