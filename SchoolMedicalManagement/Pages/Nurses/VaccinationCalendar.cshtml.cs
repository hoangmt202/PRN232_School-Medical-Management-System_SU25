using BusinessLogic.DTOs;
using BusinessLogic.DTOs.SchoolNurse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace SchoolMedicalManagement.Pages.Nurses
{
    public class VaccinationCalendarModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public VaccinationCalendarModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public List<VaccinationPlanDto> Plans { get; set; } = new();
        public string Message { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync($"vaccination/nurse/plans");

            if (response.IsSuccessStatusCode)
            {
                Plans = await response.Content.ReadFromJsonAsync<List<VaccinationPlanDto>>() ?? new();
            }
        }

        public async Task<IActionResult> OnPostSendNoticesAsync(int planId)
        {
            var response = await _httpClient.PostAsync($"vaccination/send-notices?planId={planId}", null);

            if (response.IsSuccessStatusCode)
            {
                Message = "Notices sent successfully.";
            }
            else
            {
                Message = "Failed to send notices.";
            }

            return RedirectToPage();
        }
    }
}
