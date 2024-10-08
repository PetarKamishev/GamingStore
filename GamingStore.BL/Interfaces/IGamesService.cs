﻿using GamingStore.GamingStore.Models.Models;

namespace GamingStore.GamingStore.BL.Interfaces
{
    public interface IGamesService
    {
        Task<List<Games>> GetAllGames();
        Task<Games> GetGame(int id);
        Task<Games> GetGame(string title);

        Task<Games> AddGameTag(string title, string gameTag);

        Task<Games> RemoveGameTag(string title, string gameTag);
        Task AddGame(Games game);
        Task RemoveGame(int id);


        Task<List<Games>> SearchByTag(string GameTag);

    }
}
