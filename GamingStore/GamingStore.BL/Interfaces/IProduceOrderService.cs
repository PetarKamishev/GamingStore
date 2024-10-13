using GamingStore.GamingStore.Models.Models;

namespace GamingStore.GamingStore.BL.Interfaces
{
    public interface IProduceOrderService
    {
        Task ProduceOrder(Orders orders);
    }
}
