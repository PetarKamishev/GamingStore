using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.Models.Configurations.Identity;
using GamingStore.GamingStore.Models.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IConfiguration _configuration;


        public IdentityService(UserManager<Models.Models.Users.IdentityUser> userManager,
            SignInManager<Models.Models.Users.IdentityUser> signInManager,
            JwtSettings jwtSettings,
            IPasswordHasher<Models.Models.Users.IdentityUser> passwordHasher,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }
        public async Task<string> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return "User does not exist!";
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
            {
                return "Invalid username or password!";
            }
            var userRoles = await GetRoles(user);
            var claims = new List<Claim>
                {
                    new Claim("UserId" , user.UserId.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Email" , user.Email)
                };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signIn);

        
            await _signInManager.SignInAsync(user, isPersistent: false);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
