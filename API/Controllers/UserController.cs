using BusinessLogic.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest loginDTO)
        {
            var account = await _userService.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                return Unauthorized("Invalid email or password");
            }
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var claims = new List<Claim>
            {
                new Claim("Username", account.Username.ToString()),
                new Claim("Role", account.Role.ToString()),
                new Claim("Id", account.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var preparedToken = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(preparedToken);
            var role = account.Role.ToString(); //0:Admin 1:Staff 2:Manager
            var Id = account.Id;
            return Ok(new LoginResponseDTO
            {
                Role = role,
                Token = token,
                Id = Id,
            });
        }
    }
}
