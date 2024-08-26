using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;

namespace GamingStore.GamingStore.DL.Repositories
{
   /*public class GamesRepository : IGamesRepository
    {
       public async Task AddGame(Games game)
        {
            InMemoryDb.InMemoryDb.GamesData.Add(game);
        }

        public async Task<List<Games>> GetAllGames()
        {
            return InMemoryDb.InMemoryDb.GamesData;
        }

        public async Task <Games> GetGame(int id)
        {
           return InMemoryDb.InMemoryDb.GamesData.First(x => x.Id == id);
        }

        public async Task<Games> GetGame(string title)
        {
            return InMemoryDb.InMemoryDb.GamesData.First(x=>x.Title.ToLower().Contains(title.ToLower()));
        }
        
        public Task RemoveGame(int id)
        {
            var game = GetGame(id);
            InMemoryDb.InMemoryDb.GamesData.Remove(game.Result);
            return Task.CompletedTask;
        }

        public async Task<List<Games>> SearchByTag(string GameTag)
        {
            var result= new List<Games>();
            List<Games> allGames = await GetAllGames();
            for (int i = 0; i < allGames.Count(); i++)
            {
                for (int j = 0; j < allGames[i].GameTags.Count(); j++)
                if (allGames[i].GameTags[j].ToString().ToLower().Contains(GameTag.ToLower()))
                        result.Add(allGames[i]);
            }
            return result;
        }
    } */
}
