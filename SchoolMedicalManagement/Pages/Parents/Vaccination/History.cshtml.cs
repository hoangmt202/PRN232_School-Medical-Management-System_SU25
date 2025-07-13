using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace SchoolMedicalManagement.Pages.Parents.Vaccination
{
    public class HistoryModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public HistoryModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("API");
        }

        public List<VaccinationRecordDto> Records { get; set; } = new();

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync($"vaccination/parent/history");
            if (response.IsSuccessStatusCode)
            {
                Records = await response.Content.ReadFromJsonAsync<List<VaccinationRecordDto>>() ?? new();
            }
        }
    }
}
