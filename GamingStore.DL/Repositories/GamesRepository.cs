using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;

namespace GamingStore.GamingStore.DL.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        public async Task AddGame(Games game)
        {
            InMemoryDb.InMemoryDb.GamesData.Add(game);
        }

        public List<Games> GetAllGames()
        {
            return InMemoryDb.InMemoryDb.GamesData;
        }

        public Games GetGame(int id)
        {
           return InMemoryDb.InMemoryDb.GamesData.First(x => x.Id == id);
        }

        public Games GetGame(string title)
        {
            return InMemoryDb.InMemoryDb.GamesData.First(x=>x.Title.ToLower().Contains(title.ToLower()));
        }
        
        public Task RemoveGame(int id)
        {
            var game = GetGame(id);
            InMemoryDb.InMemoryDb.GamesData.Remove(game);
            return Task.CompletedTask;
        }

        public List<Games> SearchByTag(string GameTag)
        {
            var result= new List<Games>();
            List<Games> allGames = GetAllGames();
            for (int i = 0; i < allGames.Count(); i++)
            {
                for (int j = 0; j < allGames[i].GameTags.Count(); j++)
                if (allGames[i].GameTags[j].ToLower().Contains(GameTag.ToLower()))
                        result.Add(allGames[i]);
            }
            return result;
        }
    }
}
