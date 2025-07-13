using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace SchoolMedicalManagement.Pages.Auth
{
    public class ProfileModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ProfileModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }
        public UserProfileDto Profile { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync("user/me");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            Profile = await response.Content.ReadFromJsonAsync<UserProfileDto>() ?? new();
            return Page();
        }
    }
}
