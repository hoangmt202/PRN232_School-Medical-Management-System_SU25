using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Auth
{
    public class ProfileModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public UserProfileDto Profile { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = Request.Cookies["AuthToken"];
                
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }
                
                var response = await client.GetAsync("http://localhost:5234/api/user/me");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Profile = JsonSerializer.Deserialize<UserProfileDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new();
                }
                else
                {
                    // If API fails, use session data as fallback
                    Profile = new UserProfileDto
                    {
                        Username = HttpContext.Session.GetString("Username") ?? "",
                        Email = HttpContext.Session.GetString("UserEmail") ?? "",
                        RoleName = HttpContext.Session.GetString("UserRole") ?? ""
                    };
                }
                
                return Page();
            }
            catch (Exception ex)
            {
                // Fallback to session data
                Profile = new UserProfileDto
                {
                    Username = HttpContext.Session.GetString("Username") ?? "",
                    Email = HttpContext.Session.GetString("UserEmail") ?? "",
                    RoleName = HttpContext.Session.GetString("UserRole") ?? ""
                };
                return Page();
            }
        }
    }
}
