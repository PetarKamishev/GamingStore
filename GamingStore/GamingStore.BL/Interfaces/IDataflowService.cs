using GamingStore.GamingStore.Models.Models;

namespace GamingStore.GamingStore.BL.Interfaces
{
    public interface IDataflowService
    {
        Task SendAsync(Orders order);
    }
}
