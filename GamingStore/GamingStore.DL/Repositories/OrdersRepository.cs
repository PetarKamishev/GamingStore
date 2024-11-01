using Dapper;
using GamingStore.Controllers;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GamingStore.GamingStore.DL.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IConfiguration _configuration;
        

        public OrdersRepository(IConfiguration configuration)
        {
            _configuration = configuration;           
        }
        public async Task AddOrder(Orders orders)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();

                var query = ("INSERT INTO orders ( GameId, ClientName, ClientEmail, OrderDate) VALUES( @GameId, @ClientName, @ClientEmail, @OrderDate)");

                var orderQuery = new {  GameId = orders.GameId, ClientName = orders.ClientName, ClientEmail = orders.ClientEmail, OrderDate = orders.OrderDate };

                var result = await connect.ExecuteAsync(query, orderQuery);

            }
        }

        public async Task<List<Orders>> GetAllOrders()
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();

                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders");
                return orders.ToList();
            }
        }

        public async Task<List<Orders>> GetOrdersByClientName(string clientName)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();

                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders WHERE ClientName = @clientName", new { ClientName = clientName });
                return orders.ToList();
            }
        }

        public async Task<Orders> GetOrdersByOrderId(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();

                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders WHERE OrderId = @Id", new { Id = id });
                return orders.FirstOrDefault();
            }
        }

        public async Task<List<Orders>> GetOrdersByGameId(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();

                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders WHERE GameId = @Id", new { Id = id });
                return orders.ToList();
            }
        }

        public async Task<List<Orders>> GetSpecificGameOrders(string gameTitle)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();

                var game = await connect.QueryAsync<Games>($"SELECT * FROM Games WHERE LOWER(TITLE) LIKE LOWER(@Title)", new { Title = $"%{gameTitle}%" });
                var foundGame = game.FirstOrDefault(x => x.Title.Contains(gameTitle));
                if (foundGame != null)
                {
                    var orders = await GetOrdersByGameId(foundGame.Id);
                    return orders.ToList();
                }
                else return new List<Orders>();
            }
        }

        public async Task<List<string>> GetGameTitlesByClientName(string clientName)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var gameTitles = new List<string>();
                var gamesFound = new List<Games>();
                var orders = await connect.QueryAsync<Orders>($"SELECT * FROM Orders WHERE LOWER(ClientName) LIKE LOWER(@clientName)", new { ClientName = $"{clientName}" });
                foreach(var order in orders)
                {
                    var id = order.GameId;
                    gamesFound.Add((Games)await connect.QueryAsync<Games>($"SELECT * FROM Games WHERE Id == @id", new { Id = id }));
                }                
                if (gamesFound != null)
                {
                    foreach(var game  in gamesFound)
                    {                       
                        gameTitles.Add(game.Title);
                    }
                    return gameTitles;
                }
                else return new List<string>();
            }
        }

        public async Task RemoveOrder(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();
                var query = connect.ExecuteAsync("DELETE FROM orders WHERE OrderId = @Id", new { Id = id });
            }
        }
    }
}
