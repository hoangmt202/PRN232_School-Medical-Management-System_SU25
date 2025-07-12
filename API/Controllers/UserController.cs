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
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "API is working", timestamp = DateTime.UtcNow });
        }

        [HttpGet("test-db")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                var userDetails = users.Select(u => new { 
                    Id = u.Id, 
                    Username = u.Username, 
                    Email = u.Email, 
                    Role = u.Role,
                    PasswordHash = u.PasswordHash 
                }).ToList();
                
                return Ok(new { 
                    message = "Database connection successful", 
                    userCount = users.Count,
                    users = userDetails
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database test failed");
                return StatusCode(500, new { message = "Database connection failed", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest loginDTO)
        {
            try
            {
                _logger.LogInformation($"Login attempt for email: {loginDTO.Email}");
                var account = await _userService.Login(loginDTO.Email, loginDTO.Password);
                if (account == null)
                {
                    _logger.LogWarning($"Login failed for email: {loginDTO.Email} - Invalid credentials");
                    return Unauthorized(new { message = "Invalid email or password" });
                }
                _logger.LogInformation($"Login successful for user: {account.Username} with role: {account.Role}");
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var claims = new List<Claim>
            {
                new Claim("Username", account.Username.ToString()),
                new Claim("Role", account.Role.ToString()),
                new Claim("Id", account.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var preparedToken = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
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
                UserName = account.Username
            });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during login for email: {loginDTO.Email}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
