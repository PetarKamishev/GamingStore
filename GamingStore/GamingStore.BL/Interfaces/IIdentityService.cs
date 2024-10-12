using GamingStore.GamingStore.Models.Models.Users;
using GamingStore.GamingStore.Models.Responses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace GamingStore.GamingStore.BL.Interfaces
{
    public interface IIdentityService
    {
        Task<Microsoft.AspNetCore.Identity.IdentityResult> RegisterAsync(Models.Models.Users.IdentityUser identityUser);
        Task<Microsoft.AspNetCore.Identity.IdentityResult> LoginAsync(string userName, string password);

        Task<Models.Models.Users.IdentityUser?> CheckUserAndPassword(string userName, string password);
        Task<IEnumerable<string>> GetRoles(Models.Models.Users.IdentityUser identityUser);
        Task LogOut();
    }
}
