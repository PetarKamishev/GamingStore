using GamingStore.GamingStore.Models.Models;

namespace GamingStore.GamingStore.BL.Interfaces
{
    public interface IConsumeOrderService
    {
        Task<Orders> ConsumeOrder();
    }
}
