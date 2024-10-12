using Microsoft.AspNetCore.Identity;

namespace GamingStore.GamingStore.Models.Models.Users
{
    public class IdentityUserRoles : IdentityRole
    {
        public int UserId { get; set; }
    }
}
