using GamingStore.GamingStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.GamingStore.DL.Interfaces
{
    public interface IOrdersRepository
    {
        Task<List<Orders>> GetAllOrders();

        Task<List<Orders>> GetOrdersByGameId(int gameId);

        Task<Orders> GetOrdersByOrderId(int orderId);

        Task<List<Orders>> GetOrdersByClientName(string clientName);

        Task<List<Orders>> GetSpecificGameOrders(string gameTitle);
        Task AddOrder(Orders orders);

        Task RemoveOrder(int id);
    }
}
