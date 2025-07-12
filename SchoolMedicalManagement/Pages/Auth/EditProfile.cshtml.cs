using BusinessLogic.DTOs;
using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace SchoolMedicalManagement.Pages.Auth
{
    public class EditProfileModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditProfileModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }
        [BindProperty]
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
        public async Task<IActionResult> OnPostEditProfileAsync()
        {
            var token = Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync("user/me", Profile);

            var result = await response.Content.ReadFromJsonAsync<ServiceResult>();

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                TempData["Success"] = result.Message;
                return RedirectToPage();
            }

            TempData["Error"] = result?.Message ?? "Failed to update profile.";
            return Page();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (Profile.NewPassword != Profile.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, " Passwords do not match.");
                TempData["Error"] = "Passwords do not match.";
                return RedirectToPage();
            }

            var token = Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new
            {
                CurrentPassword = Profile.CurrentPassword,
                NewPassword = Profile.NewPassword
            };

            var response = await _httpClient.PostAsJsonAsync("user/change-password", payload);
            var result = await response.Content.ReadFromJsonAsync<ServiceResult>();

            if (!response.IsSuccessStatusCode || result?.Success == false)
            {
                var errorMessage = result?.Message ?? "Failed to update password.";
                ModelState.AddModelError(string.Empty, errorMessage);
                TempData["Error"] = result?.Message ?? "Failed to update password.";
                return Page(); 
            }

            TempData["Success"] = result?.Message ?? "Password updated successfully!";
            return RedirectToPage();
        }
    }
}
