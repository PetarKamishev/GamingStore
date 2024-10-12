using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.Models.Configurations.Identity;
using GamingStore.GamingStore.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamingStore.GamingStore.BL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<Models.Models.Users.IdentityUser> _userManager;
        private readonly SignInManager<Models.Models.Users.IdentityUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordHasher<Models.Models.Users.IdentityUser> _passwordHasher;


        public IdentityService(UserManager<Models.Models.Users.IdentityUser> userManager,
            SignInManager<Models.Models.Users.IdentityUser> signInManager,
            JwtSettings jwtSettings,
            IPasswordHasher<Models.Models.Users.IdentityUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _passwordHasher = passwordHasher;
        }
        public async Task<IdentityResult> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return IdentityResult.Failed();

            }
            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
            {
                return IdentityResult.Failed();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return IdentityResult.Success;
        }




        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(Models.Models.Users.IdentityUser identityUser)
        {
            var userName = identityUser.UserName;
            var email = identityUser.Email;
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser == null)
            {
                await _userManager.CreateAsync(identityUser);
                return IdentityResult.Success;
            }
            else return IdentityResult.Failed();

        }
        public async Task<IEnumerable<string>> GetRoles(Models.Models.Users.IdentityUser identityUser)
        {
            return await _userManager.GetRolesAsync(identityUser);
        }

        public async Task<Models.Models.Users.IdentityUser?> CheckUserAndPassword(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }
            else return null;
        }
    }
}
