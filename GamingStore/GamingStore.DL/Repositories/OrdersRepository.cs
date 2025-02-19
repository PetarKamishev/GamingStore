using Dapper;
using GamingStore.Controllers;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models;
using GamingStore.GamingStore.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace GamingStore.GamingStore.DL.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IConfiguration _configuration;
        private SQLConfiguration _sqlConfiguration = new SQLConfiguration();
        public OrdersRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task AddOrder(Orders orders)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                var query = ("INSERT INTO orders ( GameId, ClientName, ClientEmail, OrderDate) VALUES( @GameId, @ClientName, @ClientEmail, @OrderDate)");

                var orderQuery = new { GameId = orders.GameId, ClientName = orders.ClientName, ClientEmail = orders.ClientEmail, OrderDate = orders.OrderDate };

                var result = await connect.ExecuteAsync(query, orderQuery);
                connect.Close();
            }
        }

        public async Task<List<Orders>> GetAllOrders()
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders");
                connect.Close();
                return orders.ToList();
            }
        }

        public async Task<int> GetOrdersCount()
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();
                var count = 0;
                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders");
                foreach(var order in orders)
                {
                    count++;
                }
                connect.Close();
                return count;
            }
        }

        public async Task<List<Orders>> GetOrdersByClientName(string clientName)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders WHERE ClientName = @clientName", new { ClientName = clientName });
                connect.Close();
                return orders.ToList();
            }
        }

        public async Task<Orders> GetOrdersByOrderId(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders WHERE OrderId = @Id", new { Id = id });
                connect.Close();
                return orders.FirstOrDefault();
            }
        }

        public async Task<List<Orders>> GetOrdersByGameId(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                var orders = await connect.QueryAsync<Orders>("SELECT * FROM orders WHERE GameId = @Id", new { Id = id });
                connect.Close();
                return orders.ToList();
            }
        }

        public async Task<List<Orders>> GetSpecificGameOrders(string gameTitle)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                var game = await connect.QueryAsync<Games>($"SELECT * FROM Games WHERE LOWER(TITLE) LIKE LOWER(@Title)", new { Title = $"%{gameTitle}%" });
                var foundGame = game.FirstOrDefault(x => x.Title.Contains(gameTitle));
                if (foundGame != null)
                {
                    var orders = await GetOrdersByGameId(foundGame.Id);
                    connect.Close();
                    return orders.ToList();
                }
                else
                {
                    connect.Close();
                    return new List<Orders>();
                }
            }
        }

        public async Task<List<string>> GetGameTitlesByClientName(string clientName)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();
                var gameTitles = new List<string>();
                
                var orders = await connect.QueryAsync<Orders>($"SELECT * FROM Orders WHERE LOWER(ClientName) LIKE LOWER(@clientName)", new { ClientName = $"{clientName}" });
                foreach (var order in orders)
                {
                    var id = order.GameId;
                   var  gamesFound=await connect.QueryAsync<string>($"SELECT TITLE FROM Games WHERE Id = @id", new { Id = id });
                    return gamesFound.ToList();
                }
               return gameTitles;
            }
        }



        public async Task RemoveOrder(int id)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();
                var query = connect.ExecuteAsync("DELETE FROM orders WHERE OrderId = @Id", new { Id = id });
                connect.Close();
            }
        }
    }
}
