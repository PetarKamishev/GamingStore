using Dapper;

using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models.Models;
using Microsoft.Data.SqlClient;


namespace GamingStore.GamingStore.DL.Repositories
{
    public class SQLGamesRepository : IGamesRepository
    {

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

                var query = ("INSERT INTO Games (TITLE, ReleaseDate, Price, GameTags) VALUES( @Title, @ReleaseDate, @Price, @GameTags)");

                var gameQuery = new { Title = game.Title, ReleaseDate = game.ReleaseDate, Price = game.Price, GameTags = game.GameTags };

                var result = await connect.ExecuteAsync(query, gameQuery);

            }
        }

        public async Task<Games> AddGameTag(string title, string gameTag)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var game = await GetGame(title);
                if (!game.GameTags.Contains(gameTag))
                {
                var query = ("UPDATE Games SET GameTags = GameTags + ', '+@GameTag WHERE LOWER(TITLE) LIKE LOWER(@Title)");

                var gameQuery = new { Title = title, GameTag = gameTag };

                var result = await connect.ExecuteAsync(query, gameQuery);
                }            
                return game;
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
                var game = await connect.QueryAsync<Games>($"SELECT * FROM Games WHERE LOWER(TITLE) LIKE LOWER(@Title)", new { Title = $"%{title}%" });
                return game.FirstOrDefault();
            }
        }

        public async Task RemoveGame(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var query = connect.ExecuteAsync("DELETE FROM Games WHERE Id = @Id", new { Id = id });

            }
        }

        public async Task<Games> RemoveGameTag(string title, string gameTag)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var game = await GetGame(title);
                if (game.GameTags.Contains(gameTag)) 
                {
                var index = game.GameTags.IndexOf(gameTag);
                if (index == 0)
                {
                    game.GameTags = game.GameTags.Replace($"{gameTag}, ", "");
                }
                else
                {
                    game.GameTags = game.GameTags.Replace($", {gameTag}", "");
                }
                var query = ("UPDATE Games SET GameTags = @GameTag WHERE LOWER(TITLE) LIKE LOWER(@Title)");
                var gameQuery = new { Title = title, GameTag = game.GameTags };

                var result = await connect.ExecuteAsync(query, gameQuery);
                }
                return game;             
            }
        }

        public async Task<List<Games>> SearchByTag(string GameTag)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var result = await connect.QueryAsync<Games>($"SELECT * FROM Games WHERE LOWER(GameTags) LIKE LOWER(@GameTag)", new { GameTag = $"%{GameTag}%" });
                return result.ToList();
            }

        }
    }
}
