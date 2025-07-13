using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace SchoolMedicalManagement.Pages.Nurses.VaccinationPlans
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("API");
        }

        public List<VaccinationPlanDto> Plans { get; set; } = new();

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync("vaccination/nurse/plans");
            if (response.IsSuccessStatusCode)
            {
                Plans = await response.Content.ReadFromJsonAsync<List<VaccinationPlanDto>>() ?? new();
            }
        }
    }
}
