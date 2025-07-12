using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Auth
{
    public class SignUpModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public SignUpModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }
        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public class RegisterInputModel
        {
            [Required]
            public string Username { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required(ErrorMessage = "You must agree to the terms")]
            [Display(Name = "Agree to Terms")]
            public bool AgreeTerms { get; set; }
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var json = JsonSerializer.Serialize(Input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("User/register", content);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Account created successfully!";
                return RedirectToPage("Login"); 
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ErrorMessage = "Failed to register: " + error;
                return Page();
            }
        }
    }

}
