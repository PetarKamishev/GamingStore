namespace GamingStore.GamingStore.Models.Models.Users
{
    public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser
    {       
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string? Password { get; set; } 

        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
