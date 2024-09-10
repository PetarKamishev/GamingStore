using GamingStore.GamingStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.GamingStore.BL.Interfaces
{
    public interface IOrdersService
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
