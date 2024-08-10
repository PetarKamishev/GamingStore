using Dapper;
using GamingStore.GamingStore.DL.GamesData;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;


namespace GamingStore.GamingStore.DL.Repositories
{
    public class SQLGamesRepository : IGamesRepository
    {
        private readonly GamesDataContext _games;
        private readonly IConfiguration _configuration;
        public SQLGamesRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public async Task AddGame(Games game)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                string query = "INSERT INTO Games (TITLE,ReleaseDate,Price,GameTags) VALUES(@Title, @ReleaseDate, @Price, @GameTags)";
                var result = await connect.ExecuteAsync(query, game);
                
            }
        }

        public async Task<List<Games>> GetAllGames()
        {

            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {

                await connect.OpenAsync();
                var games = await connect.QueryAsync<Games>("SELECT * FROM Games");             
                return games.ToList();

            }
        }

        public async Task<Games> GetGame(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var game = await connect.QueryAsync<Games>("SELECT * FROM Games WHERE ID = @Id", new { Id = id });
                return game.FirstOrDefault();
            }
        }

        public async Task<Games> GetGame(string title)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var game = await connect.QueryAsync<Games>($"SELECT * FROM Games WHERE LOWER(TITLE) LIKE LOWER('%{title}%')") ;
                return game.FirstOrDefault();
            }
        }

        public async Task RemoveGame(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var query = "DELETE FROM Games WHERE Id = @Id";
                var result = await connect.ExecuteAsync(query, new { Id = id });

            }
        }

        public async Task<List<Games>> SearchByTag(string GameTag)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();               
                var result = await connect.QueryAsync<Games>($"SELECT * FROM Games WHERE LOWER(GameTags) LIKE LOWER('%{GameTag}%')") ;
                return result.ToList();
            }

        }
    }
}
