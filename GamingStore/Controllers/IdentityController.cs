using AutoMapper;
using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.Models.Configurations.Identity;
using GamingStore.GamingStore.Models.Models.Users;
using GamingStore.GamingStore.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamingStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<GamingStore.Models.Models.Users.IdentityUser> _userManager;
        public IdentityController(IIdentityService identityService, IMapper mapper, IConfiguration configuration, UserManager<GamingStore.Models.Models.Users.IdentityUser> userManager)
        {
            _identityService = identityService;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;

        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(request.userName) || string.IsNullOrEmpty(request.password))
            {
                return BadRequest();
            }
            var userToAdd = _mapper.Map<GamingStore.Models.Models.Users.IdentityUser>(request);
            var authenticationResult = await _identityService.RegisterAsync(userToAdd);
            return Ok(authenticationResult);
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            if (request == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest();
            }
            var user = await _identityService.CheckUserAndPassword(request.UserName, request.Password);

            if(user == null) return BadRequest();
            else
            {
                var userRoles = await _identityService.GetRoles(user);
                var claims = new List<Claim>
                {
                    new Claim("UserId" , user.UserId.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Email" , user.Email)
                };

                foreach(var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"],
                    _configuration["JwtSettings:Audience"],
                    claims,
                    expires:DateTime.Now.AddMinutes(15),
                    signingCredentials:signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
           
        }
        [HttpGet("GetRoles")]

        public async Task<IEnumerable<string>> GetRoles(string userId)
        {
                var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _identityService.GetRoles(user);
            }
            else return null;
        }

        [HttpPost("LogOut")]

        public async Task LogOut()
        {
            await _identityService.LogOut();
        }
    }
}
