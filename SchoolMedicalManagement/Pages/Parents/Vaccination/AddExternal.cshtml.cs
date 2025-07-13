using BusinessLogic.DTOs;
using BusinessLogic.DTOs.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace SchoolMedicalManagement.Pages.Parents.Vaccination
{
    public class AddExternalModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public AddExternalModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("API");
        }

        [BindProperty]
        public ExternalVaccinationDto Input { get; set; } = new();

        [BindProperty]
        public List<StudentDto> Children { get; set; } = new();

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var studentRes = await _httpClient.GetAsync("student/by-parent");
            if (studentRes.IsSuccessStatusCode)
            {
                Children = await studentRes.Content.ReadFromJsonAsync<List<StudentDto>>() ?? new();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("vaccination/external", Input);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Parents/Vaccination/History");
            }

            ModelState.AddModelError("", "Failed to save vaccination record.");
            return Page();
        }
    }
}
